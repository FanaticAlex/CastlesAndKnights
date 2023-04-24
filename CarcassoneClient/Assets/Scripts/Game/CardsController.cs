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
        public Dictionary<string, GameObject> _chipsToGameObject = new Dictionary<string, GameObject>();

        private PartsController _partsController;
        private FieldsController _fieldsController;

        public Card? CurrentCard { get; set; }

        public CardsController(FieldsController fieldsController)
        {
            _partsController = new PartsController();
            _fieldsController = fieldsController;

            CreateCardsView();
            UpdateCardsView();
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

                _cardsToGameObject.Add(card.CardId, cardObject);
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
                partGameObject.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
            }
        }

        public void HideCardMarks(string cardId)
        {
            var cardGameObject = _cardsToGameObject[cardId];
            var partsGameObjects = cardGameObject.GetComponentsInChildren<Transform>().Select(child => child.gameObject);
            foreach (var partGameObject in partsGameObjects)
            {
                if (partGameObject.gameObject == cardGameObject)
                {
                    continue;
                }

                partGameObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            }
        }

        /// <summary>
        /// Устанавливает правильную позицию карты и поворот.
        /// - обновление карт после хода игроков
        /// </summary>
        public void UpdateCardsView()
        {
            var activeFields = GameManager.Instance.RoomService.GetFields().Where(f => f.CardName != null);
            foreach (var field in activeFields)
            {
                var cardGameObject = _cardsToGameObject[field.CardName];

                // поворот карты в нужную позицию
                var card = GameManager.Instance.RoomService.GetCard(field.CardName);
                cardGameObject.transform.rotation = Quaternion.Euler(0, 0, -90 * card.RotationsCount);

                var fieldId = field.Id;
                if (string.IsNullOrEmpty(fieldId))
                    continue;

                var fieldGameObject = _fieldsController._fieldsToGameObject[fieldId];
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

            var currentCardGameObject = _cardsToGameObject[card.CardId];
            currentCardGameObject.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 7);
        }

        public void UpdateCardRotationUI(Card card)
        {
            if (card == null)
                return;

            var currentCardGameObject = _cardsToGameObject[card.CardId];
            var currentCard = GameManager.Instance.RoomService.GetCard(card.CardId);
            currentCardGameObject.transform.rotation = Quaternion.Euler(0, 0, -90 * currentCard.RotationsCount);
        }
    }
}
