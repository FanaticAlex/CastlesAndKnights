using Carcassone.ApiClient;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public enum PlayerState
    {
        PlayerWait,
        PlayerHoldCard,
        PlayerHoldChip
    }

    internal class PlayerController
    {
        public Dictionary<string, GameObject> _playerToMarkers = new();

        private readonly CardsController _cardsController;
        private readonly FieldsController _fieldsController;
        private readonly ScoreController _scoreController;

        // это не надо хранить, это надо получить с сервера
        public PlayerState PlayerState { get; set; }

        public bool DoubleClick { get; set; }
        public bool Rotated { get; set; }
        public bool TurnEnded { get; set; }

        private GameObject EndTurnButton { get; set; }

        public PlayerController(
            FieldsController fieldsController,
            CardsController cardsController,
            ScoreController scoreController)
        {
            _cardsController = cardsController;
            _fieldsController = fieldsController;
            _scoreController = scoreController;

            EndTurnButton = GameObject.Find("EndTurnBtn");
        }

        public void UpdatePlayersView()
        {
            UpdatePlayersLastMoveMarkerUI();
        }

        public void HandlePlayerActions(CardsController cardsController, BasePlayer player)
        {
            if (PlayerState == PlayerState.PlayerWait)
            {
                UpdateGameViewsFromServer(player);
                PlayerState = PlayerState.PlayerHoldCard;
                return;
            }

            if (PlayerState == PlayerState.PlayerHoldCard)
            {
                PlayerHoldCardProcess(player.Name, cardsController);
                return;
            }

            if (PlayerState == PlayerState.PlayerHoldChip)
            {
                HoldChipProcess(player.Name, cardsController);
                return;
            }
        }

        public void UpdateGameViewsFromServer(BasePlayer currentPlayer)
        {
            _fieldsController.CreateFieldsIfNotExistView();
            _cardsController.PutAllCardsInFields();
            _cardsController._partsController.UpdateParts();
            _cardsController.UpdateCardRemainView();
            UpdatePlayersView();
            _scoreController.UpdateScore();
            _scoreController.UpdateCurrentPlayerMark(currentPlayer);
            _scoreController.UpdateWaitingSpinners(currentPlayer);
            _cardsController.ReloadCurrentCard();
            _fieldsController.ShowAvailableFields(_cardsController.CurrentCard);
        }

        private void PlayerHoldCardProcess(string playerName, CardsController cardsController)
        {
            EndTurnButton.GetComponent<Button>().interactable = false;

            if (cardsController.CurrentCard == null)
                return;

            if (Rotated) // действие поворот карты
            {
                Rotated = false;
                GameManager.Instance.RoomService.RotateCard(cardsController.CurrentCard.Id);
                _cardsController.ReloadCurrentCard(); // отображаем поворот
                return;
            }

            if (DoubleClick) // действие установка карты
            {
                DoubleClick = false;

                var selectedFieldId = _fieldsController.GetSelectedFieldId();
                if (selectedFieldId == null)
                {
                    Logger.Info("No field selected");
                    return;
                }

                var canPutCard = GameManager.Instance.RoomService.CanPutCard(selectedFieldId, cardsController.CurrentCard.Id);
                if (!canPutCard)
                {
                    Logger.Info("Cant put card!");
                    return;
                }

                GameManager.Instance.RoomService.PutCard(selectedFieldId, cardsController.CurrentCard.Id, playerName);

                var parts = GameManager.Instance.RoomService.GetAvailableObjectParts(cardsController.CurrentCard.Id);
                if (!parts.Any())
                {
                    Logger.Info("Card has no free parts!");
                    EndTurn(playerName);
                    return;
                }

                var player = GameManager.Instance.RoomService.GetPlayer(playerName);
                if (player.ChipCount == 0)
                {
                    Logger.Info("Player has no chip!");
                    EndTurn(playerName);
                    return;
                }

                _fieldsController.CreateFieldsIfNotExistView();
                _cardsController.PutCardInField(cardsController.CurrentCard);
                _cardsController.ShowCardMarks();
                PlayerState = PlayerState.PlayerHoldChip;
            }
        }

        private void HoldChipProcess(string playerName, CardsController cardsController)
        {
            EndTurnButton.GetComponent<Button>().interactable = true;

            if (DoubleClick)
            {
                DoubleClick = false;

                var playerHaveChip = GameManager.Instance.RoomService.GetPlayer(playerName);
                if (playerHaveChip.ChipCount == 0)
                {
                    Logger.Info("Clicked. But player has no chip!");
                    return;
                }

                // установка фишки
                var partId = GetSelectedPartName(cardsController.CurrentCard.Id);
                if (partId == null)
                {
                    Logger.Info("Clicked. But no selected part!");
                    return;
                }

                GameManager.Instance.RoomService.PutChip(cardsController.CurrentCard.Id, partId, playerName);
                _cardsController._partsController.UpdateParts();
                EndTurn(playerName);
            }

            if (TurnEnded)
            {
                TurnEnded = false;
                EndTurn(playerName);
            }
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

        private Collider GetHitCollider()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out RaycastHit hit, 100.0F);
            return hit.collider;
        }

        private string GetSelectedPartName(string cardId)
        {
            var collider = GetHitCollider();
            if (collider == null)
                return null;

            var clickedObject = collider.gameObject;
            if (clickedObject == null)
                return null;

            var cardGameObject = _cardsController._cardsToGameObject[cardId];
            var cardChildrenObjects = cardGameObject.GetComponentsInChildren<Transform>().Select(child => child.gameObject);
            foreach (var childObject in cardChildrenObjects)
            {
                if (childObject.gameObject == cardGameObject) // это обьект самой карты
                    continue;

                if (clickedObject == childObject) // кликнутый дочерний обьект карты
                    return cardId + childObject.name;
            }

            return null;
        }

        private void EndTurn(string userName)
        {
            GameManager.Instance.RoomService.EndTurn(userName);
            _cardsController.HideCardMarks();
            PlayerState = PlayerState.PlayerWait;
            EndTurnButton.GetComponent<Button>().interactable = false;
        }
    }
}
