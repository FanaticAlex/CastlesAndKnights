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
        public Dictionary<string, GameObject> _playerToMarkers = new Dictionary<string, GameObject>();

        private CardsController _cardsController;

        // это не надо хранить, это надо получить с сервера
        private PlayerState _playerState;
        public PlayerState PlayerState
        {
            get { return _playerState; }
            private set { _playerState = value; StateChanged = true; }
        }

        private string _selectedFieldId;

        public bool DoubleClick { get; set; }

        public bool Rotated { get; set; }
        public bool TurnEnded { get; set; }

        public bool StateChanged { get; set; }

        private string _playerName;
        private FieldsController _fieldsController;

        private GameObject EndTurnButton { get; set; }

        public PlayerController(
            FieldsController fieldsController,
            CardsController cardsController)
        {
            _playerName = GameManager.Instance.RoomService.User;
            _cardsController = cardsController;
            _fieldsController = fieldsController;

            EndTurnButton = GameObject.Find("EndTurnBtn");
        }

        public void UpdatePlayersView()
        {
            UpdatePlayersLastMoveMarkerUI();
        }

        public void HandlePlayerActions(CardsController cardsController)
        {
            StartMyTurn();
            MakingMove(cardsController);
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

        public void StartMyTurn()
        {
            if (PlayerState == PlayerState.PlayerWait)
            {
                PlayerState = PlayerState.PlayerHoldCard;
            }
        }

        public void MakingMove(CardsController cardsController)
        {
            if (PlayerState == PlayerState.PlayerHoldCard)
            {
                EndTurnButton.GetComponent<Button>().interactable = false;
                PlayerHoldCardProcess(_playerName, cardsController);
                return;
            }

            if (PlayerState == PlayerState.PlayerHoldChip)
            {
                EndTurnButton.GetComponent<Button>().interactable = true;
                HoldChipProcess(_playerName, cardsController);
                return;
            }
        }

        private void PlayerHoldCardProcess(string playerName, CardsController cardsController)
        {
            if (cardsController.CurrentCard == null)
                return;

            if (DoubleClick)
            {
                DoubleClick = false;

                _selectedFieldId = _fieldsController.GetSelectedFieldId();
                if (_selectedFieldId == null)
                {
                    Logger.Info("No field selected");
                    return;
                }

                // клик на поле левой кнопкой помещает в него карту
                var canPutCard = GameManager.Instance.RoomService.CanPutCard(_selectedFieldId, cardsController.CurrentCard.Id);
                if (canPutCard)
                {
                    GameManager.Instance.RoomService.PutCard(_selectedFieldId, cardsController.CurrentCard.Id, playerName);
                    var player = GameManager.Instance.RoomService.GetPlayer(playerName);

                    var parts = GameManager.Instance.RoomService.GetAvailableObjectParts(cardsController.CurrentCard.Id);
                    if (!parts.Any())
                    {
                        // если нет вариантов установки фишки сразу завершаем ход
                        EndTurn(playerName, cardsController);
                        return;
                    }

                    if (player.ChipCount == 0)
                    {
                        // если фишек нет сразу завершаем ход
                        EndTurn(playerName, cardsController);
                        return;
                    }

                    _cardsController.ShowCardMarks(cardsController.CurrentCard.Id);
                    PlayerState = PlayerState.PlayerHoldChip;
                }
                else
                {
                    Logger.Info("Cant put card!");
                }
            }
            else if (Rotated)
            {
                Rotated = false;
                // поворот поля если нажата правая кнопка поворачиваем карту и кладем на поле
                GameManager.Instance.RoomService.RotateCard(cardsController.CurrentCard.Id);
                _cardsController.ReloadCurrentCard();
            }
        }

        private void HoldChipProcess(string playerName, CardsController cardsController)
        {
            //HilightSelectedCardMark(_currentCard.CardName);

            // клик на поле левой кнопкой помещает в него Фишку
            if (DoubleClick)
            {
                DoubleClick = false;

                var playerHaveChip = GameManager.Instance.RoomService.GetPlayer(playerName);
                if (playerHaveChip.ChipCount != 0)
                {
                    // установка фишки
                    var partObject = GetSelectedPartUI(cardsController.CurrentCard.Id);
                    if (partObject != null)
                    {
                        GameManager.Instance.RoomService.PutChip(cardsController.CurrentCard.Id, partObject, playerName);
                        EndTurn(playerName, cardsController);
                    }
                }
            }

            // клик на поле правой кнопкой означает что игрок не хочет устанавливать фишку.
            if (TurnEnded)
            {
                TurnEnded = false;
                EndTurn(playerName, cardsController);
            }
        }

        private Collider GetHitCollider()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, 100.0F);
            return hit.collider;
        }

        /*private void HilightSelectedCardMark(string cardId)
        {
            var collider = GetHitCollider();

            var cardGameObject = _cardsController._cardsToGameObject[cardId];
            var partsGameObjects = cardGameObject.GetComponentsInChildren<Transform>().Select(child => child.gameObject);
            foreach (var partGameObject in partsGameObjects)
            {
                if (partGameObject.gameObject == cardGameObject)
                {
                    continue;
                }

                if (collider != null && collider.gameObject == partGameObject)
                {
                    partGameObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                }
                else
                {
                    partGameObject.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                }
            }
        }*/

        private string GetSelectedPartUI(string cardId)
        {
            var collider = GetHitCollider();

            var cardGameObject = _cardsController._cardsToGameObject[cardId];
            var partsGameObjects = cardGameObject.GetComponentsInChildren<Transform>().Select(child => child.gameObject);
            foreach (var partGameObject in partsGameObjects)
            {
                if (partGameObject.gameObject == cardGameObject)
                {
                    continue;
                }

                if (collider != null && collider.gameObject == partGameObject)
                {
                    return cardId + partGameObject.name;
                }
            }

            return null;
        }

        private void EndTurn(string userName, CardsController cardsController)
        {
            GameManager.Instance.RoomService.EndTurn(userName);
            _cardsController.HideCardMarks(cardsController.CurrentCard.Id);
            PlayerState = PlayerState.PlayerWait;
            EndTurnButton.GetComponent<Button>().interactable = false;
        }
    }
}
