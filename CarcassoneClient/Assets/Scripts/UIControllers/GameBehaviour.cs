using Carcassone.Core;
using Carcassone.Core.Cards;
using Carcassone.Core.Fields;
using Carcassone.Core.Players;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
    /// <summary>
    /// Содержит данные об игре и управляет ходами одного игрока, а так же обновлением игры
    /// </summary>
    public class GameBehaviour : MonoBehaviour
    {
        private FieldsController _fieldsController;
        private CardsController _cardsController;
        private PlayerController _playerController;
        private ScoreController _scoreController;

        private Timer _updateTimer;

        public GameObject SelectPartPanel;
        public GameObject FinalScoreUIPanel;
        public GameObject FinalScoreUIPanelText;
        public GameObject PlayerDetailScorePanel;

        private GameRoom _room;

        private Field _selectedField;
        private ObjectPart _selectedPart;

        private GameRoom _preliminaryGameRoomWithNewCard;

        /// <summary>
        /// Инициализация сцены комнаты игры.
        /// - создание игрового поля
        /// - инициализация счета игроков 
        /// </summary>
        void Start()
        {
            _room = GameManager.Instance.RoomService.Room;

            _fieldsController = new FieldsController(_room);
            _cardsController = new CardsController(_room);
            _scoreController = new ScoreController(FinalScoreUIPanel, FinalScoreUIPanelText, PlayerDetailScorePanel);
            _playerController = new PlayerController(_cardsController);

            VisualizeTurn(_room.Moves[0]);

            _updateTimer = new Timer(0.5f);
            _updateTimer.Elapsed += async (s, e) => await UpdateSpecial();

            SelectPartPanel.SetActive(false);
        }

        public void VisualizeTurn(GameMove gameMove)
        {
            // отображаем ход
            var player = _room.PlayersPool.GetPlayer(gameMove.PlayerName);
            var field = _room.FieldBoard.GetField(gameMove.FieldId);
            var card = _room.CardsPool.GetCard(gameMove.CardId);
            var part = card.GetPart(gameMove.PartName);

            PutCardInField_OnlyUI(card, field, gameMove.CardRotation);
            _playerController.UpdatePlayerLastMoveMarkerUI(card, player);
            _cardsController._partsController.ShowFlagsAndChips();

            _cardsController.ReloadCurrentCard();
            _cardsController.UpdateCardRemainView();
            _fieldsController.ShowAvailableFields(_room.GetCurrentCard());

            _scoreController.UpdateScore();
            _scoreController.UpdateCurrentPlayerMark(player);
            _scoreController.UpdateWaitingSpinners(player);
        }

        /// <summary>
        /// Основоной цикл игры.
        /// Производит ход локального игрока и обновляет UI компоненты игры.
        /// </summary>
        async void Update()
        {
            // если наш ход
            var currentPlayer = _room.PlayersPool.GetCurrentPlayer();
            switch (currentPlayer.PlayerType)
            {
                case PlayerType.Human:
                    if (Input.GetMouseButtonUp(0)) TryToPutCardInField(currentPlayer.Name, _cardsController);
                    break;
                case PlayerType.AI_Easy:
                case PlayerType.AI_Normal:
                case PlayerType.AI_Hard:
                    currentPlayer.ProcessMove(_room);
                    VisualizeTurn(_room.Moves.Last());
                    break;
                case PlayerType.NetworkPlayer:
                    // ждать сетевых игроков 
                    break;
            }


            // клик по обьекту для просмотра владельцев
        }

        // для сетевой игры
        private async Task UpdateSpecial()
        {
            // окончание игры
            var isFinished = GameManager.Instance.RoomService.Room.IsFinished;
            if (isFinished)
            {
                var currentPlayer = GameManager.Instance.RoomService.GetCurrentPlayer();
                this.enabled = false;
                _scoreController.ShowEndGameWindow();
               return;
            }
        }

        public void OnRotateButonClick()
        {
            var currentCard = _room.GetCurrentCard();
            PutCardInField_Preliminary(currentCard, _selectedField);
            _cardsController.ReloadCurrentCard();
        }

        public void OnPartSelected()
        {
            var currentCard = _room.GetCurrentCard();
            // у всех частей обьекта анимацию убираем
            _cardsController.HideAllCardMarks(currentCard);

            // у выбранного включаем
            _selectedPart = GetSelectedPart(currentCard);
            _cardsController.ShowPartMark(_selectedPart);
        }

        public void OnPutCardCancel()
        {
            // TODO: при отмене карта убирается из поля
        }

        public void OnEndTurnButonClick()
        {
            var currentCard = _room.GetCurrentCard();
            var currentPlayer = GameManager.Instance.RoomService.GetCurrentPlayer();

            _selectedPart = GetSelectedPart(currentCard);

            var gameMove = new GameMove()
            {
                PlayerName = currentPlayer.Name,
                CardId = currentCard.Id,
                CardRotation = currentCard.RotationsCount,
                FieldId = _selectedField.Id,
                PartName = _selectedPart?.PartName
            };
            _room.MakeMove(gameMove);
            VisualizeTurn(gameMove);

            // TODO: отправить ход на сервер для сетевой игры

            // ход окончен
            _cardsController.HideAllCardMarks(currentCard);
            _selectedField = null;
            _selectedPart = null;
            SelectPartPanel.SetActive(false);

        }

        public void OnShowPlayerDetailedScore(Text playerNamePanel)
        {
            _scoreController.ShowDetailedScore(playerNamePanel);
        }

        public void OnClosePlayerDetailedScore()
        {
            _scoreController.HideDetailedScore();
        }

        public void OnEndGameBtn()
        {
            SceneManager.LoadScene("CreateRoom", LoadSceneMode.Single);
        }

        private void TryToPutCardInField(string playerName, CardsController cardsController)
        {
            if (_selectedField != null) // если мы в состоянии выбора чати то не кликам на поля
                return;

            // устанавливаем карту в поле
            var hittedGO = GetHitedGameObject();
            var selectedFieldId = _fieldsController.GetFieldByGameObject(hittedGO);
            if (selectedFieldId == null)
            {
                Logger.Info("No field selected");
                return;
            }
            _selectedField = _room.FieldBoard.GetField(selectedFieldId);

            var currentCard = _room.GetCurrentCard();
            var canPutCard = _room.CanPutCardInFieldWithRotation(_selectedField, currentCard);
            if (!canPutCard)
            {
                Logger.Info("Cant put a card!");
                return;
            }

            PutCardInField_Preliminary(currentCard, _selectedField);
        }

        private void PutCardInField_Preliminary(Card card, Field field)
        {
            // клик по полю для установки карты
            // при нажатии карта предварительно ставится в поле
            // камера приближается к карте
            // отображается UI для установки фишек/поворота карты

            _room.RotateCardTilFit(field, card);

            _preliminaryGameRoomWithNewCard = new GameRoom();
            _preliminaryGameRoomWithNewCard.Load(_room.Save());

            var fieldGO = _fieldsController.GetFieldGameObject(field.Id);
            var field_Temp = _preliminaryGameRoomWithNewCard.FieldBoard.GetField(field.Id);
            var card_Temp = _preliminaryGameRoomWithNewCard.CardsPool.GetCard(card.Id);
            _preliminaryGameRoomWithNewCard.RotateCardTilFit(field_Temp, card_Temp);
            var cardGO = _cardsController.GetCardGO(card.Id);
            cardGO.transform.position = fieldGO.transform.position + new Vector3(0, 0, -1);
            cardGO.transform.rotation = Quaternion.Euler(0, 0, -90 * card.RotationsCount);
            _preliminaryGameRoomWithNewCard.PutCardInField(card, field_Temp);

            ShowSelectPartPanel();
        }

        private void PutCardInField_OnlyUI(Card card, Field field, int rotation)
        {
            _fieldsController.CreateFieldsIfNotExistView();
            var fieldGO = _fieldsController.GetFieldGameObject(field.Id);
            card.RotateCard(rotation);
            var cardGO = _cardsController.GetCardGO(card.Id);
            cardGO.transform.position = fieldGO.transform.position + new Vector3(0, 0, -1);
            cardGO.transform.rotation = Quaternion.Euler(0, 0, -90 * card.RotationsCount);
            _fieldsController.CreateFieldsIfNotExistView();
        }

        private GameObject GetHitedGameObject()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out RaycastHit hit, 100.0F);
            return hit.collider?.gameObject;
        }

        private void ShowSelectPartPanel()
        {
            // дальше устанавливаем part
            var currentCard = _room.GetCurrentCard();
            var parts = _preliminaryGameRoomWithNewCard.GetAvailableParts(currentCard.Id);
            if (!parts.Any())
            {
                Logger.Info("Card has no free parts!");
                OnEndTurnButonClick();
                return;
            }

            var currentPlayer = GameManager.Instance.RoomService.GetCurrentPlayer();
            if (currentPlayer.ChipCount == 0)
            {
                Logger.Info("Player has no chip!");
                OnEndTurnButonClick();
                return;
            }

            InitToggles(parts);
            SelectPartPanel.SetActive(true);
        }

        private void InitToggles(List<ObjectPart> parts)
        {
            var toggleGroup = SelectPartPanel.transform.Find("ToggleGroup");
            for (int i = 0; i < 9; i++)
            {
                var toggleName = "Toggle" + i;
                var toggle = toggleGroup.Find(toggleName);
                var toggleLabel = toggle.Find("Label");

                if (i < parts.Count())
                {
                    toggle.gameObject.SetActive(true);
                    toggleLabel.GetComponent<Text>().text = parts[i].PartName;
                }
                else
                {
                    toggle.gameObject.SetActive(false);
                }
            }

            var noneToggle = toggleGroup.Find("Toggle_None");
            var noneToggleComponent = noneToggle.GetComponent<Toggle>();
            noneToggleComponent.isOn = true;
        }

        private ObjectPart GetSelectedPart(Card card)
        {
            var toggleGroup = SelectPartPanel.transform.Find("ToggleGroup");

            // не устанавливать фишку
            var noneToggle = toggleGroup.Find("Toggle_None");
            var noneToggleComponent = noneToggle.GetComponent<Toggle>();
            if (noneToggleComponent.isOn)
                return null;

            // установить фишку в части обьекта
            for (int i = 0; i < 9; i++)
            {
                var toggleName = "Toggle" + i;
                var toggle = toggleGroup.Find(toggleName);
                var toggleComponent = toggle.GetComponent<Toggle>();
                var toggleLabel = toggle.Find("Label").GetComponent<Text>();

                if (toggleComponent.isOn)
                    return card.Parts.Single(p => p.PartName == toggleLabel.text);
            }

            return null;
        }
    }
}
