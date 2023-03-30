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

        public Card ReloadCurrentCard()
        {
            try
            {
                CurrentCard = GameManager.Instance.RoomService.GetCurrentCard();
                return CurrentCard;
            }
            catch (AggregateException ex)
            {
                if (((ApiException)ex.InnerException).StatusCode == 204)
                {
                    return null;
                }
                else
                {
                    throw ex;
                }
            }
        }

        public void CreateCardsView()
        {
            var subfolderCards = "Cards/";
            var cards = GameManager.Instance.RoomService.GetCards();
            foreach (var card in cards)
            {
                var prefab = (GameObject)Resources.Load(subfolderCards + card.CardName.Substring(0, card.CardName.Length - 2), typeof(GameObject));
                if (prefab == null)
                    prefab = (GameObject)Resources.Load(subfolderCards + "River/" + card.CardName.Substring(0, card.CardName.Length - 2), typeof(GameObject));

                var cardObject = GameObject.Instantiate(prefab);
                if (cardObject == null)
                    throw new Exception();

                _cardsToGameObject.Add(card.CardName, cardObject);
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
        /// Обновление расстановки выложенных фишек на игровой доске
        /// </summary>
        public void UpdateChipsView()
        {
            //var cards = GameManager.Instance.RoomService.GetCards();
            foreach (var cardName in _cardsToGameObject.Keys)
            {
                var card = GameManager.Instance.RoomService.GetCard(cardName);
                foreach (var part in card.Parts)
                {
                    // если фишку снали убираем ее
                    if (part.Chip == null)
                    {
                        if (_chipsToGameObject.ContainsKey(part.PartId))
                        {
                            GameObject.Destroy(_chipsToGameObject[part.PartId]);
                            _chipsToGameObject.Remove(part.PartId);
                        }

                        continue;
                    }

                    // если фишка уже установлена, то пропускаем
                    if (_chipsToGameObject.ContainsKey(part.PartId))
                        continue;

                    GameObject chipPrefab = null;
                    if (part.Chip.Type == ChipType._0)
                        chipPrefab = Constants.Chip["Knight"];

                    if (part.Chip.Type == ChipType._1)
                        chipPrefab = Constants.Chip["Priest"];

                    if (part.Chip.Type == ChipType._2)
                        chipPrefab = Constants.Chip["Thief"];

                    if (part.Chip.Type == ChipType._3)
                        chipPrefab = Constants.Chip["Peasant"];

                    if (chipPrefab != null)
                    {
                        var chipObject = GameObject.Instantiate(chipPrefab);
                        chipObject.transform.position = _partsController._partToGameObject[part.PartId].transform.position + new Vector3(0, 0, -0.2f);
                        Color userColor;
                        ColorUtility.TryParseHtmlString(part.Chip.Owner.Color, out userColor);
                        chipObject.GetComponent<MeshRenderer>().material.color = userColor;
                        _chipsToGameObject.Add(part.PartId, chipObject);
                    }
                }
            }
        }

        /// <summary>
        /// Устанавливает правильную позицию карты и поворот.
        /// - обновление карт после хода игроков
        /// </summary>
        public void UpdateCardsView()
        {
            foreach (var item in _cardsToGameObject)
            {
                var cardName = item.Key;
                var cardGameObject = item.Value;

                // поворот карты в нужную позицию
                var card = GameManager.Instance.RoomService.GetCard(cardName);
                cardGameObject.transform.rotation = Quaternion.Euler(0, 0, -90 * card.RotationsCount);

                var fieldId = card.Field?.Id;
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

            var currentCardGameObject = _cardsToGameObject[card.CardName];
            currentCardGameObject.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 7);
        }

        public void UpdateCardRotationUI(Card card)
        {
            if (card == null)
                return;

            var currentCardGameObject = _cardsToGameObject[card.CardName];
            var currentCard = GameManager.Instance.RoomService.GetCard(card.CardName);
            currentCardGameObject.transform.rotation = Quaternion.Euler(0, 0, -90 * currentCard.RotationsCount);
        }
    }
}
