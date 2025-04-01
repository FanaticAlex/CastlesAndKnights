using Carcassone.Core;
using Carcassone.Core.Cards;
using Carcassone.Core.Fields;
using Carcassone.Core.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
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
        private CameraBehaviour _cameraBehaviour;

        /// <summary>
        /// Инициализация сцены комнаты игры.
        /// - создание игрового поля
        /// - инициализация счета игроков 
        /// </summary>
        void Start()
        {
            _cameraBehaviour = Camera.main.GetComponent<CameraBehaviour>();
            _room = GameManager.Instance.Room;
            _room.Finished += _room_Finished;

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
            var time = DateTime.Now;

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
            _fieldsController.ShowAvailableFields(_room.CardsPool.CurrentCard);

            _scoreController.UpdateScore();

            var currentPlayer = _room.PlayersPool.GetCurrentPlayer();
            _scoreController.UpdateCurrentPlayerMark(currentPlayer);

            _cameraBehaviour.MoveCameraAtCard(_cardsController.GetCardGO(card.Id)); // анимация

            Logger.Info("Время отрисовки хода: " + (DateTime.Now - time).TotalSeconds);
        }

        /// <summary>
        /// Основоной цикл игры.
        /// Производит ход локального игрока и обновляет UI компоненты игры.
        /// </summary>
        async void Update()
        {
            // TODO: клик по обьекту для просмотра владельцев

            if (_room.IsFinished) return;

            if (_cameraBehaviour.State == TouchState.ZoomToCard) return;

            // если наш ход
            var currentPlayer = _room.PlayersPool.GetCurrentPlayer();
            if (currentPlayer.IsAIProcessing) return;

            switch (currentPlayer.PlayerType)
            {
                case PlayerType.Human:
                    if (Input.GetMouseButtonUp(0)) TryToPutCardInField(currentPlayer.Name, _cardsController);
                    break;
                case PlayerType.AI_Easy:
                case PlayerType.AI_Normal:
                case PlayerType.AI_Hard:
                    await Task.Run(() => currentPlayer.ProcessMove(_room));
                    VisualizeTurn(_room.Moves.Last());
                    break;
                case PlayerType.NetworkPlayer:
                    // ждать сетевых игроков 
                    break;
            }
        }

        // для сетевой игры
        private async Task UpdateSpecial()
        {
            // окончание игры
            var isFinished = GameManager.Instance.Room.IsFinished;
            if (isFinished)
            {
                var currentPlayer = GameManager.Instance.Room.PlayersPool.GetCurrentPlayer();
                this.enabled = false;
                _scoreController.ShowEndGameWindow();
               return;
            }
        }

        public void OnRotateButonClick()
        {
            var currentCard = _room.CardsPool.CurrentCard;
            PutCardInField_Preliminary(currentCard, _selectedField);
            _cardsController.ReloadCurrentCard();
        }

        public void OnPartSelected()
        {
            var currentCard = _room.CardsPool.CurrentCard;
            // у всех частей обьекта анимацию убираем
            _cardsController.HideAllCardMarks(currentCard);

            // у выбранного включаем
            _selectedPart = GetSelectedPart(currentCard);
            _cardsController.ShowPartMark(_selectedPart);
        }

        public void OnPutCardCancel()
        {
            // при отмене карта убирается из поля
            _selectedField = null;
            ResetCardGOTransform(_room.CardsPool.CurrentCard);
            SelectPartPanel.SetActive(false);
        }

        public void OnEndTurnButonClick()
        {
            var currentCard = _room.CardsPool.CurrentCard;
            var currentPlayer = GameManager.Instance.Room.PlayersPool.GetCurrentPlayer();

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
            GameManager.Instance.SaveScore();
            GameManager.Instance.ResetGame();
        }

        private void TryToPutCardInField(string playerName, CardsController cardsController)
        {
            if (SelectPartPanel.activeSelf) // если мы в состоянии выбора чати то не кликам на поля
                return;

            // находим поле по которому был клик
            var hittedGO = GetHitedGameObject();
            var selectedFieldId = _fieldsController.GetFieldByGameObject(hittedGO);
            if (selectedFieldId == null)
            {
                Logger.Info("No field selected");
                return;
            }
            _selectedField = _room.FieldBoard.GetField(selectedFieldId);

            // можно ли в это поле ставить текущую карту
            var currentCard = _room.CardsPool.CurrentCard;
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
            _room.RotateCardTilFit(field, card);
            SetCardGOTransform(field, card);

            // сохраняем временную(предварительную) позицию игры с установленной картой для определния того куда ставить фишки
            _preliminaryGameRoomWithNewCard = new GameRoom();
            _preliminaryGameRoomWithNewCard.Load(_room.Save());
            var field_Temp = _preliminaryGameRoomWithNewCard.FieldBoard.GetField(field.Id);
            var card_Temp = _preliminaryGameRoomWithNewCard.CardsPool.GetCard(card.Id);
            _preliminaryGameRoomWithNewCard.PutCardInField(card_Temp, field_Temp);

            ShowSelectPartPanel();
        }

        private void PutCardInField_OnlyUI(Card card, Field field, int rotation)
        {
            card.RotateCard(rotation);
            _fieldsController.CreateFieldsIfNotExistView();
            SetCardGOTransform(field, card);
            _fieldsController.CreateFieldsIfNotExistView();
        }

        private void SetCardGOTransform(Field field, Card card_Temp)
        {
            var fieldGO = _fieldsController.GetFieldGameObject(field.Id);
            var cardGO = _cardsController.GetCardGO(card_Temp.Id);
            cardGO.transform.position = fieldGO.transform.position + new Vector3(0, 0, -1);
            cardGO.transform.rotation = Quaternion.Euler(0, 0, -90 * card_Temp.RotationsCount);
        }

        private void ResetCardGOTransform(Card card)
        {
            var cardGO = _cardsController.GetCardGO(card.Id);
            cardGO.transform.position = new Vector3(0, 0, 0);
            cardGO.transform.rotation = Quaternion.Euler(0, 0, 0);
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
            var currentCard = _room.CardsPool.CurrentCard;
            var parts = _preliminaryGameRoomWithNewCard.GetAvailableParts(currentCard.Id);
            if (!parts.Any())
            {
                Logger.Info("Card has no free parts!");
                OnEndTurnButonClick();
                return;
            }

            var currentPlayer = GameManager.Instance.Room.PlayersPool.GetCurrentPlayer();
            if (currentPlayer.ChipCount == 0)
            {
                Logger.Info("Player has no chip!");
                OnEndTurnButonClick();
                return;
            }

            _cameraBehaviour.MoveCameraAtCard(_cardsController.GetCardGO(currentCard.Id)); // анимация

            SelectPartPanel.SetActive(true);
            InitToggles(parts);
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

        private void _room_Finished(object sender, EventArgs e)
        {
            _scoreController.ShowEndGameWindow();
        }

        private ObjectPart GetSelectedPart(Card card)
        {
            if (!SelectPartPanel.activeSelf) return null; // окно не активно потому что нет вариантов для установки фишки

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
