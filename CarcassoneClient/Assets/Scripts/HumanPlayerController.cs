using System.Linq;
using UnityEngine;
using Carcassone.ApiClient;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public enum PlayerState
    {
        PlayerWait,
        PlayerHoldCard,
        PlayerHoldChip
    }

    internal class HumanPlayerController
    {
        public PlayerState _playerState;
        private string _selectedFieldId;
        private Card _currentCard;

        public bool MouseButton0 { get; set; }
        public bool MouseButton1 { get; set; }

        private Player _player;
        private FieldsController _fieldsController;
        private CardsController _cardsController;

        public HumanPlayerController(Player player, FieldsController fieldsController, CardsController cardsController)
        {
            _player = player;
            _fieldsController = fieldsController;
            _cardsController = cardsController;
        }

        public void StartMyTurn()
        {
            if (_playerState == PlayerState.PlayerWait)
                _playerState = PlayerState.PlayerHoldCard;
        }

        public void MakingMove()
        {
            if (_playerState == PlayerState.PlayerHoldCard)
            {
                PlayerHoldCardProcess(_player);
                return;
            }

            if (_playerState == PlayerState.PlayerHoldChip)
            {
                HoldChipProcess(_player);
                return;
            }
        }

        private void PlayerHoldCardProcess(Player player)
        {
            // в начале хода тянем карту, если ее нет
            if (_currentCard == null)
            {
                _currentCard = RoomService.Instance.Client.Current2Async(RoomService.Instance.RoomId).Result;
                if (_currentCard == null)
                    return;
            }

            _selectedFieldId = _fieldsController.GetSelectedFieldId();

            if (MouseButton0)
            {
                MouseButton0 = false;
                // клик на поле левой кнопкой помещает в него карту
                var canPutCard = RoomService.Instance.Client.CanPutCardAsync(RoomService.Instance.RoomId, _selectedFieldId, _currentCard.CardName).Result;
                if (canPutCard)
                {
                    RoomService.Instance.Client.PutCardInFieldAsync(RoomService.Instance.RoomId, _selectedFieldId, _currentCard.CardName).Wait();
                    var player1 = RoomService.Instance.Client.PlayerGETAsync(RoomService.Instance.RoomId, player.Name).Result;
                    if (player1.ChipCount != 0)
                    {
                        _cardsController.ShowCardMarks(_currentCard.CardName);
                    }

                    _playerState = PlayerState.PlayerHoldChip;
                }
            }
            else if (MouseButton1)
            {
                MouseButton1 = false;
                // поворот поля если нажата правая кнопка поворачиваем карту и кладем на поле
                RoomService.Instance.Client.RotateCardAsync(RoomService.Instance.RoomId, _currentCard.CardName).Wait();
            }
        }

        private void HoldChipProcess(Player player)
        {
            HilightSelectedCardMark(_currentCard.CardName);

            // клик на поле левой кнопкой помещает в него Фишку
            if (MouseButton0)
            {
                MouseButton0 = false;

                var playerHaveChip = RoomService.Instance.Client.PlayerGETAsync(RoomService.Instance.RoomId, player.Name).Result;
                if (playerHaveChip.ChipCount != 0)
                {
                    // установка фишки
                    var partObject = GetSelectedPartUI(_currentCard.CardName);
                    if (partObject != null)
                    {
                        RoomService.Instance.Client.PutChipInCardAsync(RoomService.Instance.RoomId, _currentCard.CardName, partObject, player.Name);
                        EndTurn();
                    }
                }
            }

            // клик на поле правой кнопкой означает что игрок не хочет устанавливать фишку.
            if (MouseButton1)
            {
                MouseButton1 = false;
                EndTurn();
            }
        }

        private Collider GetHitCollider()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, 100.0F);
            return hit.collider;
        }

        private void HilightSelectedCardMark(string cardId)
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
        }

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

        private void EndTurn()
        {
            RoomService.Instance.Client.EndTurnAsync(RoomService.Instance.RoomId).Wait();
            _cardsController.HideCardMarks(_currentCard.CardName);
            _currentCard = null;
            _playerState = PlayerState.PlayerWait;
        }
    }
}
