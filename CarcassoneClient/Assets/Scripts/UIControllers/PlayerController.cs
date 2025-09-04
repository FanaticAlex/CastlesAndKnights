using Carcassone.Core.Cards;
using Carcassone.Core.Players;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    internal class PlayerController
    {
        public Dictionary<string, GameObject> _playerToMarkers = new();

        private readonly CardsController _cardsController;

        public PlayerController(CardsController cardsController)
        {
            _cardsController = cardsController;
        }

        public void UpdatePlayerLastMoveMarkerUI(Card card, GamePlayer player)
        {
            if (player == null)
                return;

            var playerHaveMark = _playerToMarkers.ContainsKey(player.Name);
            if (!playerHaveMark)
            {
                var marksPrefab = Constants.Marks[player.Color];
                _playerToMarkers[player.Name] = GameObject.Instantiate(marksPrefab);
            }
            
            var markObject = _playerToMarkers[player.Name];
            var cardPosition = _cardsController.GetCardPosition(card.Id);
            markObject.transform.position = cardPosition + new Vector3(0, 0, -1.3f);
        }
    }
}
