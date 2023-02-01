using System.Linq;
using UnityEngine;
using Carcassone.ApiClient;

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
        // это не надо хранить, это надо получить с сервера
        public PlayerState _playerState;
        private string _selectedFieldId;
        private Card _currentCard;

        public bool MouseButton0 { get; set; }
        public bool MouseButton1 { get; set; }

        public bool Rotated { get; set; }
        public bool TurnEnded { get; set; }

        private Player _player;
        private FieldsController _fieldsController;
        private CardsController _cardsController;

        private GameObject EndTurnButton { get; set; }

        public HumanPlayerController(Player player, FieldsController fieldsController, CardsController cardsController)
        {
            _player = player;
            _fieldsController = fieldsController;
            _cardsController = cardsController;

            EndTurnButton = GameObject.Find("EndTurnButton");
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
                EndTurnButton.SetActive(false);
                PlayerHoldCardProcess(_player);
                return;
            }

            if (_playerState == PlayerState.PlayerHoldChip)
            {
                EndTurnButton.SetActive(true);
                HoldChipProcess(_player);
                return;
            }
        }

        private void PlayerHoldCardProcess(Player player)
        {
            // в начале хода тянем карту, если ее нет
            if (_currentCard == null)
            {
                _currentCard = GameManager.Instance.RoomService.GetCurrentCard();
                if (_currentCard == null)
                    return;
            }

            if (MouseButton0)
            {
                MouseButton0 = false;

                _selectedFieldId = _fieldsController.GetSelectedFieldId();
                if (_selectedFieldId == null)
                    return;

                // клик на поле левой кнопкой помещает в него карту
                var canPutCard = GameManager.Instance.RoomService.CanPutCard(_selectedFieldId, _currentCard.CardName);
                if (canPutCard)
                {
                    GameManager.Instance.RoomService.PutCard(_selectedFieldId, _currentCard.CardName);
                    var player1 = GameManager.Instance.RoomService.GetPlayer(player.Name);
                    if (player1.ChipCount != 0)
                    {
                        _cardsController.ShowCardMarks(_currentCard.CardName);
                    }

                    _playerState = PlayerState.PlayerHoldChip;
                }
            }
            else if (MouseButton1 || Rotated)
            {
                MouseButton1 = false;
                Rotated = false;
                // поворот поля если нажата правая кнопка поворачиваем карту и кладем на поле
                GameManager.Instance.RoomService.RotateCard(_currentCard.CardName);
            }
        }

        private void HoldChipProcess(Player player)
        {
            HilightSelectedCardMark(_currentCard.CardName);

            // клик на поле левой кнопкой помещает в него Фишку
            if (MouseButton0)
            {
                MouseButton0 = false;

                var playerHaveChip = GameManager.Instance.RoomService.GetPlayer(player.Name);
                if (playerHaveChip.ChipCount != 0)
                {
                    // установка фишки
                    var partObject = GetSelectedPartUI(_currentCard.CardName);
                    if (partObject != null)
                    {
                        GameManager.Instance.RoomService.PutChip(_currentCard.CardName, partObject, player.Name);
                        EndTurn();
                    }
                }
            }

            // клик на поле правой кнопкой означает что игрок не хочет устанавливать фишку.
            if (MouseButton1 || TurnEnded)
            {
                MouseButton1 = false;
                TurnEnded = false;
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
            GameManager.Instance.RoomService.EndTurn();
            _cardsController.HideCardMarks(_currentCard.CardName);
            _currentCard = null;
            _playerState = PlayerState.PlayerWait;
        }
    }
}
