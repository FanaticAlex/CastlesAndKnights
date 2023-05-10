using Carcassone.ApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    internal class CardsController
    {
        public Dictionary<string, GameObject> _cardsToGameObject = new Dictionary<string, GameObject>();

        public PartsController _partsController;

        public Card? CurrentCard { get; set; }

        public CardsController(FieldsController fieldsController)
        {
            _partsController = new PartsController();

            CreateCardsView();
            UpdateCardsView(fieldsController);
        }

        public void ReloadCurrentCard()
        {
            CurrentCard = GameManager.Instance.RoomService.GetCurrentCard();
        }

        public void CreateCardsView()
        {
            var subfolderCards = "Cards/";
            var cards = GameManager.Instance.RoomService.GetCards();
            foreach (var card in cards)
            {
                var prefab = (GameObject)Resources.Load(subfolderCards + card.CardType, typeof(GameObject));
                if (prefab == null)
                    prefab = (GameObject)Resources.Load(subfolderCards + "River/" + card.CardType, typeof(GameObject));

                var cardObject = GameObject.Instantiate(prefab);
                if (cardObject == null)
                    throw new Exception();

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

        public void UpdateCardRemainView()
        {
            var cardsRemain = GameManager.Instance.RoomService.GetCardsRemain();
            var cardsRemainText = GameObject.Find("CardsRemain").GetComponent<Text>();
            cardsRemainText.text = $"{cardsRemain}";
        }

        public void ShowCardMarks(string cardId)
        {
            var parts = GameManager.Instance.RoomService.GetAvailableObjectParts(cardId);
            foreach (var part in parts)
            {
                var partGameObject = _partsController._partToGameObject[part.PartId];
                partGameObject.SetActive(true);
            }
        }

        public void HideCardMarks(string cardId)
        {
            var cardGameObject = _cardsToGameObject[cardId];
            var partsGameObjects = cardGameObject.GetComponentsInChildren<Transform>().Select(child => child.gameObject);
            foreach (var partGameObject in partsGameObjects)
            {
                if (partGameObject.gameObject == cardGameObject) // с самой картой ничего делать не надо
                    continue;

                partGameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Устанавливает правильную позицию карты и поворот.
        /// - обновление карт после хода игроков
        /// </summary>
        public void UpdateCardsView(FieldsController fieldsController)
        {
            foreach (var field in fieldsController.FieldsCache)
            {
                if (field.CardName == null)
                    continue;

                var cardGameObject = _cardsToGameObject[field.CardName];

                // поворот карты в нужную позицию
                var card = GameManager.Instance.RoomService.GetCard(field.CardName);
                cardGameObject.transform.rotation = Quaternion.Euler(0, 0, -90 * card.RotationsCount);

                var fieldId = field.Id;
                if (string.IsNullOrEmpty(fieldId))
                    continue;

                var fieldGameObject = fieldsController._fieldsToGameObject[fieldId];
                cardGameObject.transform.position = fieldGameObject.transform.position + new Vector3(0, 0, -1);
            }

            _partsController.UpdatePartsOwnersUI();
        }

        /// <summary>
        /// Перемещает карту в руке игрока (игрок держит карту).
        /// </summary>
        /// <param name="player"></param>
        public void UpdateCardPositionByCursor(Card card)
        {
            if (card == null)
                return;

            var currentCardGameObject = _cardsToGameObject[card.Id];
            currentCardGameObject.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 7);
        }

        public void UpdateCardRotationUI(Card card)
        {
            if (card == null)
                return;

            var currentCardGameObject = _cardsToGameObject[card.Id];
            var currentCard = GameManager.Instance.RoomService.GetCard(card.Id);
            currentCardGameObject.transform.rotation = Quaternion.Euler(0, 0, -90 * currentCard.RotationsCount);
        }
    }
}
