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
        public Dictionary<string, GameObject> _cardsToGameObject = new();

        public PartsController _partsController;
        public FieldsController _fieldsController;

        public Card CurrentCard { get; set; }
        public GameObject currentCardImageGO;

        public CardsController(FieldsController fieldsController)
        {
            _partsController = new PartsController();
            _fieldsController = fieldsController;

            CreateCardsView();
            PutAllCardsInFields(); // установка стартовой карты на поле
            currentCardImageGO = GameObject.Find("CurrentCardImage");
        }

        public void ReloadCurrentCard()
        {
            CurrentCard = GameManager.Instance.RoomService.GetCurrentCard();
            
            // поворот текущей карты игрока 
            if (CurrentCard != null)
            {
                var cardGO = _cardsToGameObject[CurrentCard.Id];
                currentCardImageGO.GetComponent<Image>().sprite = cardGO.GetComponent<SpriteRenderer>().sprite;
                currentCardImageGO.transform.localRotation = Quaternion.Euler(0, 0, CurrentCard.RotationsCount * -90);
            }
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

        public void UpdateCardRemainView()
        {
            var cardsRemain = GameManager.Instance.RoomService.GetCardsRemain();
            var cardsRemainText = GameObject.Find("CardsRemain").GetComponent<Text>();
            cardsRemainText.text = $"{cardsRemain}";
        }

        public void ShowCardMarks()
        {
            var parts = GameManager.Instance.RoomService.GetAvailableObjectParts(CurrentCard.Id);
            foreach (var part in parts)
            {
                var partGameObject = _partsController._partToGameObject[part.PartId];
                partGameObject.SetActive(true);
            }
        }

        public void HideCardMarks()
        {
            foreach (var part in _partsController._partToGameObject)
                part.Value.SetActive(false);
        }

        /// <summary>
        /// Устанавливает правильную позицию карты и поворот.
        /// - обновление карт после хода игроков
        /// </summary>
        public void PutAllCardsInFields()
        {
            foreach (var field in _fieldsController.FieldsCache)
            {
                if (field.CardName == null)
                    continue;

                var cardGameObject = _cardsToGameObject[field.CardName];

                // поворот 
                var card = GameManager.Instance.RoomService.GetCard(field.CardName);
                cardGameObject.transform.rotation = Quaternion.Euler(0, 0, -90 * card.RotationsCount);
                // позиция
                var fieldGameObject = _fieldsController._fieldsToGameObject[field.Id];
                cardGameObject.transform.position = fieldGameObject.transform.position + new Vector3(0, 0, -1);
            }
        }

        public void PutCardInField(Card card)
        {
            foreach (var field in _fieldsController.FieldsCache)
            {
                if (field.CardName == null)
                    continue;

                if (field.CardName != card.Id)
                    continue;

                var cardGameObject = _cardsToGameObject[field.CardName];
                var fieldGameObject = _fieldsController._fieldsToGameObject[field.Id];
                cardGameObject.transform.position = fieldGameObject.transform.position + new Vector3(0, 0, -1);
                cardGameObject.transform.rotation = Quaternion.Euler(0, 0, -90 * card.RotationsCount);
            }
        }
    }
}
