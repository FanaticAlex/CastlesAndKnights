using Carcassone.ApiClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading.Tasks;
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

        float _timer;
        float _delta = 0.5f;

        public GameObject FinalScoreUIPanel;
        public GameObject FinalScoreUIPanelText;

        /// <summary>
        /// Инициализация сцены комнаты игры.
        /// - создание игрового поля
        /// - инициализация счета игроков 
        /// </summary>
        void Start()
        {
            FinalScoreUIPanel.SetActive(false);

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

            _timer += Time.deltaTime;
            if (_timer > _delta) // тут помещаем длительные операции, которые невозможно производить каждый upadate
            {
                _timer = 0;
                UpdateGameViews();
            }

            // тут операции, которые необходимо делать каждый update для нормального отображения
            // карта перемещается вместе с курсором
            var isPlayerHoldCard = _playersController._playerController._playerState == PlayerState.PlayerHoldCard;
            if (isPlayerHoldCard)
                _cardsController.UpdateCardPositionByCursor(_currentCard);
        }

        public void OnRotateButonClick()
        {
            _playersController._playerController.Rotated = true;
        }

        private void UpdateGameViews()
        {
            // проверяем окончание хода
            var card = _cardsController.GetCurrentPlayerCard();
            var isTurnChanged = (card?.CardName != _currentCard?.CardName);
            _currentCard = card;

            // установка рисунка карты в контрол текущей карты
            var cardGO = _cardsController._cardsToGameObject[card.CardName];
            GameObject.Find("CurrentCardImage").GetComponent<Image>().sprite = cardGO.GetComponent<SpriteRenderer>().sprite;
            GameObject.Find("CurrentCardImage").transform.localRotation = Quaternion.Euler(0, 0, card.RotationsCount * -90);

            // это обновляем только при смене хода, для оптимизации
            if (isTurnChanged)
            {
                _fieldsController.UpdateFieldsView(_currentCard);
                _cardsController.UpdateCardsView();
            }

            _cardsController.UpdateChipsView();
            _playersController.UpdatePlayersView();
            // обновление поворота карты (нужно ли это вообще?)
            _cardsController.UpdateCardRotationUI(_currentCard);

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
