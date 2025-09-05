using Carcassone.Core;
using Carcassone.Core.Cards;
using Carcassone.Core.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    /// <summary>
    /// This class is responsible for tiles instantiate and view.
    /// </summary>
    internal class CardsController
    {
        /// <summary>
        /// Collection of game tiles
        /// </summary>
        private Dictionary<string, GameObject> _cardsToGameObject = new();
        private Dictionary<string, GameObject> _cardsBordersToGameObject = new();

        public PartsController _partsController;
        public GameObject currentCardImageGO;
        public ParticleSystem placeCardEffect;

        private GameRoom _room;

        public CardsController(GameRoom room)
        {
            _room = room;
            _partsController = new PartsController();

            CreateCardsView();
            currentCardImageGO = GameObject.Find("CurrentCardImage");
            placeCardEffect = GameObject.Find("PlaceCardEffectParticles").GetComponent<ParticleSystem>();
        }

        public Vector3 GetCardPosition(string cardId)
        {
            return _cardsToGameObject[cardId].transform.position;
        }

        public void ResetSetPositionRotation(string cardId)
        {

        }

        public void SetCardPositionRotation(string cardId, Vector3 position, int rotationsCount)
        {
            var cardGO = _cardsToGameObject[cardId];
            cardGO.transform.position = position + new Vector3(0, 0, -1);
            cardGO.transform.rotation = Quaternion.Euler(0, 0, -90 * rotationsCount);
            
            var border = _cardsBordersToGameObject[cardId];
            border.transform.position = position + new Vector3(0, 0, -1.1f);

            placeCardEffect.transform.position = position;
            placeCardEffect.Play();
        }

        /// <summary>
        /// Обновление UI текущей карты
        /// </summary>
        public void ReloadCurrentCard()
        {
            var currentCard = _room.CardsPool.CurrentCard; 
            if (currentCard != null)
            {

                // изображение на панели
                var cardGO = _cardsToGameObject[currentCard.Id];
                currentCardImageGO.GetComponent<Image>().sprite = cardGO.GetComponent<SpriteRenderer>().sprite;
                currentCardImageGO.transform.localRotation = Quaternion.Euler(0, 0, currentCard.RotationsCount * -90);
            }
        }

        public void UpdateCardRemainView()
        {
            var cardsRemain = _room.CardsPool.CardsDeck.Count;
            var cardsRemainText = GameObject.Find("CardsRemain").GetComponent<Text>();
            cardsRemainText.text = $"{cardsRemain}";
        }

        public void ShowPartMark(ObjectPart part)
        {
            if (part == null) return;

            var partGameObject = _partsController._partToGameObject[part.PartId];
            partGameObject.SetActive(true);
        }

        public void HideAllCardMarks(Card card)
        {
            foreach (var part in card.Parts)
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
            foreach (var card in _room.CardsPool.GetAllCards())
            {
                GameObject prefab = GetCardPrefab(card);
                var cardObject = GameObject.Instantiate(prefab) ?? throw new Exception();
                _cardsToGameObject.Add(card.Id, cardObject);
                cardObject.GetComponent<BoxCollider>().enabled = false;

                // Parts on cards
                foreach (var part in card.Parts)
                {
                    var partGameObject = cardObject.transform.Find(part.PartName).gameObject;
                    partGameObject.SetActive(false);
                    _partsController._partToGameObject.Add(part.PartId, partGameObject);
                }

                // Border
                GameObject borderPrefab = (GameObject)Resources.Load("Cards/Tile_border", typeof(GameObject));
                var cardBorderObject = GameObject.Instantiate(borderPrefab) ?? throw new Exception();
                _cardsBordersToGameObject.Add(card.Id, cardBorderObject);
            }

            UpdateCardRemainView();
        }

        private static GameObject GetCardPrefab(Card card)
        {
            var subfolderCards = "Cards/";
            var prefab = (GameObject)Resources.Load(subfolderCards + card.CardType, typeof(GameObject));
            if (prefab == null)
                prefab = (GameObject)Resources.Load(subfolderCards + "River/" + card.CardType, typeof(GameObject));
            return prefab;
        }
    }
}
