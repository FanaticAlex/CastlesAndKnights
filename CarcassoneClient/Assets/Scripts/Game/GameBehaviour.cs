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
    /// Содержит данные об игре и управляет ходами одного игрока, а так же обновлением игры
    /// </summary>
    public class GameBehaviour : MonoBehaviour
    {
        private FieldsController _fieldsController;
        private CardsController _cardsController;
        private PlayerController _playerController;
        private ScoreController _scoreController;

        private string _currentPlayerName;

        private Timer _updateTimer;
        private DoubleClickController _doubleClickController;

        public GameObject FinalScoreUIPanel;
        public GameObject FinalScoreUIPanelText;
        public GameObject PlayerDetailScorePanel;
        public GameObject currentCardImageGO;

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

            _updateTimer = new Timer(0.5f);
            _updateTimer.Elapsed += async (s, e) => await UpdateSpecial();

            _doubleClickController = new DoubleClickController();
        }

        /// <summary>
        /// Основоной цикл игры.
        /// Производит ход локального игрока и обновляет UI компоненты игры.
        /// </summary>
        async void Update()
        {
            try
            {
                _playerController.DoubleClick |= _doubleClickController.IsDoubleClick();
                await _updateTimer.Update(Time.deltaTime); // длительные операции, запросы к серверу
            }
            catch (Exception ex)
            {
                Logger.Info(ex.ToString());
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
