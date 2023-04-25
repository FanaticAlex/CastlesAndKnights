using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    internal class PlayersController
    {
        public Dictionary<string, GameObject> _playerToMarkers = new Dictionary<string, GameObject>();

        public HumanPlayerController _humanPlayerController;
        private CardsController _cardsController;

        private GameObject _waitingSpinner;

        public PlayersController(
            FieldsController fieldsController,
            CardsController cardsController)
        {
            var name = GameManager.Instance.RoomService.User;
            _humanPlayerController = new HumanPlayerController(name, fieldsController, cardsController);

            _cardsController = cardsController;

            _waitingSpinner = GameObject.Find("WaitingSpinner");
        }

        public void UpdatePlayersView()
        {
            UpdatePlayersLastMoveMarkerUI();
        }

        public void HandlePlayerActions()
        {
            _humanPlayerController.StartMyTurn();
            _humanPlayerController.MakingMove();
            HideWaitingSpinner();
        }

        public void ShowWaitingSpinner()
        {
            _waitingSpinner.SetActive(true);
        }

        public void HideWaitingSpinner()
        {
            _waitingSpinner.SetActive(false);
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
    }
}
