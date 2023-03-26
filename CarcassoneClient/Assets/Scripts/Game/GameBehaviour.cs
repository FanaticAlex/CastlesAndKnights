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
        private PlayersController _playersController;
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
            _playersController = new PlayersController(_fieldsController, _cardsController);

            _scoreController = new ScoreController(FinalScoreUIPanel, FinalScoreUIPanelText, PlayerDetailScorePanel);
        }

        /// <summary>
        /// Основоной цикл игры.
        /// Производит ход локального игрока и обновляет UI компоненты игры.
        /// </summary>
        void Update()
        {
            _playersController._humanPlayerController.LeftButton |= Input.GetMouseButtonDown(0);
            _playersController._humanPlayerController.DoubleClick |= IsDoubleClick();
            _playersController._humanPlayerController.RighrButtonClick |= Input.GetMouseButtonDown(1);

            _timer -= Time.deltaTime;
            if (_timer <= 0.0f) // длительные операции, запросы к серверу
            {
                _timer = 0.1f;
                var currentPlayer = GameManager.Instance.RoomService.GetCurrentPlayer();
                var isMyTurn = (currentPlayer.Name == GameManager.Instance.RoomService.User);
                if (isMyTurn)
                {
                    _playersController.HandlePlayerActions();
                }
                else
                {
                    _playersController.ShowWaitingSpinner();
                }

                // установка рисунка карты в контрол текущей карты
                if (_cardsController.CurrentCard != null)
                {
                    var cardGO = _cardsController._cardsToGameObject[_cardsController.CurrentCard.CardName];
                    var currentCardImageGO = GameObject.Find("CurrentCardImage");
                    currentCardImageGO.GetComponent<Image>().sprite = cardGO.GetComponent<SpriteRenderer>().sprite;
                    currentCardImageGO.transform.localRotation = Quaternion.Euler(0, 0, _cardsController.CurrentCard.RotationsCount * -90);
                }

                // это обновляем отображение карт только при смене хода,
                // и при смене статуса хода, для оптимизации
                var isTurnChanged = (currentPlayer?.Name != _currentPlayerName);
                if (isTurnChanged || _playersController._humanPlayerController.StateChanged)
                {
                    _playersController._humanPlayerController.StateChanged = false;
                    _currentPlayerName = currentPlayer?.Name;
                    UpdateGameViewsFromServer();
                }
            }
        }

        private float timer1 = 0;
        private bool rememberedButtonClick;
        private bool IsDoubleClick()
        {
            // first click setup timer and remember click
            if (Input.GetMouseButtonDown(0) && timer1 <= 0)
            {
                timer1 = 1;
                rememberedButtonClick = true;
                return false;
            }

            // second click is doubleclick
            if (rememberedButtonClick && Input.GetMouseButtonDown(0) && timer1 > 0)
            {
                timer1 = 0;
                rememberedButtonClick = false;
                return true;
            }

            // just waiting second click
            if (timer1 > 0)
            {
                timer1 -= Time.deltaTime;
            }
            else // timer dropped
            {
                rememberedButtonClick = false;
            }

            return false;
        }

        public void OnRotateButonClick()
        {
            _playersController._humanPlayerController.Rotated = true;
        }

        public void OnEndTurnButonClick()
        {
            _playersController._humanPlayerController.TurnEnded = true;
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

        private void UpdateGameViewsFromServer()
        {
            if (_playersController._humanPlayerController.PlayerState != PlayerState.PlayerHoldChip)
                _cardsController.ReloadCurrentCard();

            _fieldsController.UpdateAvailableFieldsView(_cardsController.CurrentCard);
            _cardsController.UpdateCardsView();
            _cardsController.UpdateCardRemainView();

            _cardsController.UpdateChipsView();
            _playersController.UpdatePlayersView();
            _scoreController.UpdateScore();
            _scoreController.UpdateCurrentPlayerMark();

            // окончание игры
            var isFinished = GameManager.Instance.RoomService.GetRoom().IsFinished;
            if (isFinished)
            {
                // дополнительное обновление
                _fieldsController.UpdateAvailableFieldsView(null);
                _cardsController.UpdateCardsView();
                _cardsController.UpdateChipsView();

                this.enabled = false;
                _scoreController.ShowEndGameWindow();
                return;
            }
        }
    }
}
