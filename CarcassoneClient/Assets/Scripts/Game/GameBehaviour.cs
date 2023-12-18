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

        private Timer _updateTimer;
        private DoubleClickController _doubleClickController;

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
            _scoreController = new ScoreController(FinalScoreUIPanel, FinalScoreUIPanelText, PlayerDetailScorePanel);
            _playerController = new PlayerController(_fieldsController, _cardsController, _scoreController);

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
                await _updateTimer.Update(Time.deltaTime); // затратные операции, запросы к серверу
            }
            catch (Exception ex)
            {
                Logger.Info(ex.ToString());
            }
        }

        private async Task UpdateSpecial()
        {
            var currentPlayer = await GameManager.Instance.RoomService.GetCurrentPlayer();

            // наш ход
            var isLocalPlayerTurn = GameManager.Instance.RoomService.HumanUser == currentPlayer?.Name;
            if (isLocalPlayerTurn)
            {
                _playerController.HandlePlayerActions(_cardsController, currentPlayer);
            }

            // окончание игры
            var isFinished = GameManager.Instance.RoomService.GetRoom().IsFinished;
            if (isFinished)
            {
                _playerController.UpdateGameViewsFromServer(currentPlayer);
                this.enabled = false;
                _scoreController.ShowEndGameWindow();
                return;
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
    }
}
