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
            var players = RoomService.Instance.Client.List2Async(RoomService.Instance.RoomId).Result;
            _scoreController = new ScoreController(players);
            var player = players.First(p => p.Name == RoomService.Instance.User.Login);
            _playerController = new HumanPlayerController(player, fieldsController, cardsController);

            _cardsController = cardsController;
        }

        public void UpdatePlayersView()
        {
            var currentPlayer = RoomService.Instance.Client.CurrentAsync(RoomService.Instance.RoomId).Result;
            var _isMyTurn = (currentPlayer.Name == RoomService.Instance.User.Login);

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
            var players = RoomService.Instance.Client.List2Async(RoomService.Instance.RoomId).Result;
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
            var players = RoomService.Instance.Client.List2Async(RoomService.Instance.RoomId).Result;
            var _playerToScore = new Dictionary<Player, PlayerScore>();
            foreach (var player in players)
            {
                var score = RoomService.Instance.Client.ScoreAsync(RoomService.Instance.RoomId, player.Name).Result;
                _playerToScore.Add(player, score);
            }

            _scoreController.UpdateScore(_playerToScore);

            var cardsRemain = RoomService.Instance.Client.RemainAsync(RoomService.Instance.RoomId).Result;
            var cardsRemainText = GameObject.Find("CardsRemain").GetComponent<Text>();
            cardsRemainText.text = "Осталось карт:" + cardsRemain;
        }
    }
}
