using Carcassone.Core;
using Carcassone.Core.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    internal class CardsController
    {
        private Dictionary<string, GameObject> _cardsToGameObject = new();

        public PartsController _partsController;
        public GameObject currentCardImageGO;

        private GameRoom _room;

        public CardsController(GameRoom room)
        {
            _room = room;
            _partsController = new PartsController();

            CreateCardsView(); // создаем объекты карт
            currentCardImageGO = GameObject.Find("CurrentCardImage");
        }

        public GameObject GetCardGO(string cardId)
        {
            return _cardsToGameObject[cardId];
        }

        /// <summary>
        /// Обновление UI текущей карты
        /// </summary>
        public void ReloadCurrentCard()
        {
            var currentCard = _room.GetCurrentCard(); 
            if (currentCard != null)
            {
                var cardGO = _cardsToGameObject[currentCard.Id];
                currentCardImageGO.GetComponent<Image>().sprite = cardGO.GetComponent<SpriteRenderer>().sprite;
                currentCardImageGO.transform.localRotation = Quaternion.Euler(0, 0, currentCard.RotationsCount * -90);
            }
        }

        public void UpdateCardRemainView()
        {
            var cardsRemain = _room.GetCardsRemain();
            var cardsRemainText = GameObject.Find("CardsRemain").GetComponent<Text>();
            cardsRemainText.text = $"{cardsRemain}";
        }

        public void ShowPartMark(ObjectPart part)
        {
            if (part == null) return;

            var partGameObject = _partsController._partToGameObject[part.PartId];
            partGameObject.SetActive(true);
        }

        public void HideAllCardMarks()
        {
            foreach (var part in _room.GetCurrentCard().Parts)
                HidePartMark(part);
        }

        public void HidePartMark(ObjectPart part)
        {
            if (part == null) return;

            var partGameObject = _partsController._partToGameObject[part.PartId];
            partGameObject.SetActive(false);
        }

        private void CreateCardsView()
        {
            var subfolderCards = "Cards/";
            foreach (var card in _room.CardsPool.AllCards)
            {
                var prefab = (GameObject)Resources.Load(subfolderCards + card.CardType, typeof(GameObject));
                if (prefab == null)
                    prefab = (GameObject)Resources.Load(subfolderCards + "River/" + card.CardType, typeof(GameObject));

                var cardObject = GameObject.Instantiate(prefab) ?? throw new Exception();
                _cardsToGameObject.Add(card.Id, cardObject);
                cardObject.GetComponent<BoxCollider>().enabled = false;

                // Части
                foreach (var part in card.Parts)
                {
                    var partGameObject = cardObject.transform.Find(part.PartName).gameObject;
                    partGameObject.SetActive(false);
                    _partsController._partToGameObject.Add(part.PartId, partGameObject);
                }
            }

            UpdateCardRemainView();
        }
    }
}
