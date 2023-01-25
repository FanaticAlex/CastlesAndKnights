using Carcassone.ApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    internal class PlayersController
    {
        public Dictionary<string, GameObject> _playerToMarkers = new Dictionary<string, GameObject>();

        private ScoreController _scoreController;
        public HumanPlayerController _playerController;
        private CardsController _cardsController;

        public PlayersController(FieldsController fieldsController, CardsController cardsController)
        {
            var players = GameManager.Instance.RoomService.GetPlayers();
            _scoreController = new ScoreController(players);
            var player = players.First(p => p.Name == GameManager.Instance.RoomService.User.Login);
            _playerController = new HumanPlayerController(player, fieldsController, cardsController);

            _cardsController = cardsController;
        }

        public void UpdatePlayersView()
        {
            var currentPlayer = GameManager.Instance.RoomService.GetCurrentPlayer();
            var _isMyTurn = (currentPlayer.Name == GameManager.Instance.RoomService.User.Login);

            UpdatePlayersLastMoveMarkerUI();

            if (_isMyTurn)
            {
                _playerController.StartMyTurn();
                _playerController.MakingMove();
            }

            UpdateScore();
        }

        /// <summary>
        /// Отображаем маркер игроков (рамочку) на поле
        /// </summary>
        public void UpdatePlayersLastMoveMarkerUI()
        {
            var players = GameManager.Instance.RoomService.GetPlayers();
            foreach (var player in players)
            {
                var playerHaveMark = _playerToMarkers.ContainsKey(player.Name);
                if (!playerHaveMark)
                {
                    var marksPrefab = Constants.Marks[player.Color];
                    _playerToMarkers[player.Name] = GameObject.Instantiate(marksPrefab);
                }

                if (player.LastCardId != null)
                {
                    var markObject = _playerToMarkers[player.Name];
                    markObject.transform.position = _cardsController._cardsToGameObject[player.LastCardId].transform.position + new Vector3(0, 0, -1.3f);
                }
            }
        }

        private void UpdateScore()
        {
            // вычислить очки
            var players = GameManager.Instance.RoomService.GetPlayers();
            var _playerToScore = new Dictionary<Player, PlayerScore>();
            foreach (var player in players)
            {
                var score = GameManager.Instance.RoomService.GetScore(player.Name);
                _playerToScore.Add(player, score);
            }

            _scoreController.UpdateScore(_playerToScore);

            var cardsRemain = GameManager.Instance.RoomService.GetCardsRemain();
            var cardsRemainText = GameObject.Find("CardsRemain").GetComponent<Text>();
            cardsRemainText.text = "Осталось карт:" + cardsRemain;
        }
    }
}
