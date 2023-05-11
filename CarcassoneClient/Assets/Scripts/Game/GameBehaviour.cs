using Carcassone.ApiClient;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{

    /// <summary>
    /// Содержит данные об игре и управляет ходами одного игрока.
    /// </summary>
    public class GameBehaviour : MonoBehaviour
    {
        private FieldsController _fieldsController;
        private CardsController _cardsController;
        private PlayerController _playerController;
        private ScoreController _scoreController;

        private string _currentPlayerName;

        float _timer;

        public GameObject FinalScoreUIPanel;
        public GameObject FinalScoreUIPanelText;
        public GameObject PlayerDetailScorePanel;

        /// <summary>
        /// Инициализация сцены комнаты игры.
        /// - создание игрового поля
        /// - инициализация счета игроков 
        /// </summary>
        void Start()
        {
            _fieldsController = new FieldsController();
            _cardsController = new CardsController(_fieldsController);
            _playerController = new PlayerController(_fieldsController, _cardsController);

            _scoreController = new ScoreController(FinalScoreUIPanel, FinalScoreUIPanelText, PlayerDetailScorePanel);
        }

        /// <summary>
        /// Основоной цикл игры.
        /// Производит ход локального игрока и обновляет UI компоненты игры.
        /// </summary>
        async void Update()
        {
            _playerController.DoubleClick |= IsDoubleClick();

            _timer -= Time.deltaTime;
            if (_timer <= 0.0f) // длительные операции, запросы к серверу
            {
                _timer = 0.5f;
                await UpdateSpecial();
            }
        }

        private async Task UpdateSpecial()
        {
            var currentPlayer = await GameManager.Instance.RoomService.GetCurrentPlayer();
            var isLocalPlayerTurn = GameManager.Instance.RoomService.HumanUsers.Contains(currentPlayer?.Name);
            if (isLocalPlayerTurn)
            {
                _playerController.HandlePlayerActions(_cardsController, currentPlayer.Name);
            }

            // поворот текущей карты
            if (_cardsController.CurrentCard != null)
            {
                var cardGO = _cardsController._cardsToGameObject[_cardsController.CurrentCard.Id];
                var currentCardImageGO = GameObject.Find("CurrentCardImage");
                currentCardImageGO.GetComponent<Image>().sprite = cardGO.GetComponent<SpriteRenderer>().sprite;
                currentCardImageGO.transform.localRotation = Quaternion.Euler(0, 0, _cardsController.CurrentCard.RotationsCount * -90);
            }

            // это обновляем отображение карт только при смене хода,
            // и при смене статуса хода, для оптимизации
            var isTurnChanged = (currentPlayer?.Name != _currentPlayerName);
            if (isTurnChanged || _playerController.StateChanged)
            {
                _playerController.StateChanged = false;
                _currentPlayerName = currentPlayer?.Name;
                UpdateGameViewsFromServer(currentPlayer);
            }
        }

        private float _timer1 = 0;
        private bool _rememberedButtonClick;
        private Vector3 _rememberedCoursorPosition;

        private bool IsDoubleClick()
        {
            // first click setup timer and remember click
            if (Input.GetMouseButtonUp(0) && _timer1 <= 0)
            {
                _timer1 = 0.3f; // time to make doubleclick
                _rememberedButtonClick = true;
                _rememberedCoursorPosition = Input.mousePosition;
                return false;
            }

            // second click is doubleclick
            if (_rememberedButtonClick && Input.GetMouseButtonUp(0) && _timer1 > 0)
            {
                var isNear = Vector3.Magnitude(Input.mousePosition - _rememberedCoursorPosition) < 30;
                if (isNear)
                {
                    _timer1 = 0;
                    _rememberedButtonClick = false;
                    _rememberedCoursorPosition = Vector3.zero;
                    return true;
                }
                else
                {
                    Logger.Info("Not near second click!");
                }
            }

            // just waiting second click
            if (_timer1 > 0)
            {
                _timer1 -= Time.deltaTime;
            }
            else // timer dropped
            {
                _rememberedButtonClick = false;
                _rememberedCoursorPosition = Vector3.zero;
            }

            return false;
        }

        public void OnRotateButonClick()
        {
            _playerController.Rotated = true;
        }

        public void OnEndTurnButonClick()
        {
            _playerController.TurnEnded = true;
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

        private void UpdateGameViewsFromServer(BasePlayer currentPlayer)
        {
            if (_playerController.PlayerState != PlayerState.PlayerHoldChip)
                _cardsController.ReloadCurrentCard();

            _fieldsController.CreateFieldsIfNotExistView();

            if (_playerController.PlayerState == PlayerState.PlayerHoldCard) // оптимизируем
                _fieldsController.UpdateFieldsView(_cardsController.CurrentCard);

            _cardsController.UpdateCardsView(_fieldsController);
            _cardsController.UpdateCardRemainView();
            _playerController.UpdatePlayersView();
            _scoreController.UpdateScore();
            _scoreController.UpdateCurrentPlayerMark(currentPlayer);
            _scoreController.UpdateWaitingSpinners(currentPlayer);

            // окончание игры
            var isFinished = GameManager.Instance.RoomService.GetRoom().IsFinished;
            if (isFinished)
            {
                // дополнительное обновление
                _fieldsController.UpdateFieldsView(null); // подсвечивает все как недоступное

                this.enabled = false;
                _scoreController.ShowEndGameWindow();
                return;
            }
        }
    }
}
