using Carcassone.ApiClient;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

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

        private Card _currentCard;

        float _timer1;
        float _timer2;

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
            FinalScoreUIPanel.SetActive(false);
            PlayerDetailScorePanel.SetActive(false);

            _fieldsController = new FieldsController();
            _cardsController = new CardsController(_fieldsController);
            _playersController = new PlayersController(_fieldsController, _cardsController);
        }

        /// <summary>
        /// Основоной цикл игры.
        /// Производит ход локального игрока и обновляет UI компоненты игры.
        /// </summary>
        void Update()
        {
            _playersController._playerController.MouseButton0 |= Input.GetMouseButtonDown(0);
            _playersController._playerController.MouseButton1 |= Input.GetMouseButtonDown(1);

            _timer2 += Time.deltaTime;
            if (_timer2 > 0.1f) // тут помещаем длительные операции, которые невозможно производить каждый upadate
            {
                _timer2 = 0;
                _playersController.HandlePlayerActions();
            }

            _timer1 += Time.deltaTime;
            if (_timer1 > 0.5f) // тут помещаем длительные операции, которые невозможно производить каждый upadate
            {
                _timer1 = 0;
                UpdateGameViewsFromServer();
            }

            // тут операции, которые необходимо делать каждый update для нормального отображения
            // карта перемещается вместе с курсором
            //var isPlayerHoldCard = _playersController._playerController._playerState == PlayerState.PlayerHoldCard;
            //if (isPlayerHoldCard)
            //    _cardsController.UpdateCardPositionByCursor(_currentCard);
        }

        public void OnRotateButonClick()
        {
            _playersController._playerController.Rotated = true;
        }

        public void OnEndTurnButonClick()
        {
            _playersController._playerController.TurnEnded = true;
        }

        public void OnShowPlayerDetailedScore(Text playerNamePanel)
        {
            var playerName = playerNamePanel.text;
            if (!PlayerDetailScorePanel.activeSelf)
            {
                PlayerDetailScorePanel.SetActive(true);
                var textComp = GameObject.Find("DetailedPlayerScore").GetComponent<TMP_Text>();
                textComp.text = "Очки игрока " + playerName + "\r\n";
                var score = GameManager.Instance.RoomService.GetScore(playerName);
                textComp.text += $"Замки: {score.Castles}\r\n";
                textComp.text += $"Поля: {score.Cornfields}\r\n";
                textComp.text += $"Аббатства: {score.Churches}\r\n";
                textComp.text += $"Дороги: {score.Roads}\r\n";
            }
        }

        public void OnClosePlayerDetailedScore()
        {
            PlayerDetailScorePanel.SetActive(false);
        }

        private void UpdateGameViewsFromServer()
        {
            // проверяем окончание хода
            var card = _cardsController.GetCurrentPlayerCard();
            var isTurnChanged = (card?.CardName != _currentCard?.CardName);
            _currentCard = card;
            if (isTurnChanged)
            {
                // это обновляем только при смене хода, для оптимизации
                _fieldsController.UpdateFieldsView(_currentCard);
                _cardsController.UpdateCardsView();
            }

            // установка рисунка карты в контрол текущей карты
            if (card != null)
            {
                var cardGO = _cardsController._cardsToGameObject[card.CardName];
                GameObject.Find("CurrentCardImage").GetComponent<Image>().sprite = cardGO.GetComponent<SpriteRenderer>().sprite;
                GameObject.Find("CurrentCardImage").transform.localRotation = Quaternion.Euler(0, 0, card.RotationsCount * -90);
            }

            _cardsController.UpdateChipsView();
            _playersController.UpdatePlayersView();


            // обновление поворота карты (нужно ли это вообще?)
            //_cardsController.UpdateCardRotationUI(_currentCard);

            // окончание игры
            var isFinished = GameManager.Instance.RoomService.GetRoom().IsFinished;
            if (isFinished)
            {
                // дополнительное обновление
                _fieldsController.UpdateFieldsView(null);
                _cardsController.UpdateCardsView();
                _cardsController.UpdateChipsView();

                SetEndGameView();
                return;
            }
        }

        private void SetEndGameView()
        {
            FinalScoreUIPanel.SetActive(true);
            this.enabled = false;

            FinalScoreUIPanelText.GetComponent<TMP_Text>().text = String.Empty;
            var scores = GameManager.Instance.RoomService.GetGameScores();
            foreach (var score in scores)
            {
                FinalScoreUIPanelText.GetComponent<TMP_Text>().text += $"{score.UserName} : {score.FinalScore} \r\n"; 
            }

            GameManager.Instance.RoomService.Reset();
        }

        public void OnEndGameBtn()
        {
            SceneManager.LoadScene("CreateRoom", LoadSceneMode.Single);
        }
    }
}
