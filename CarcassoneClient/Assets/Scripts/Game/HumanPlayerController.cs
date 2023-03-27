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
        private PlayerState _playerState;
        public PlayerState PlayerState
        {
            get { return _playerState; }
            private set { _playerState = value; StateChanged = true; }
        }
        private string _selectedFieldId;
        private Card _currentCard;

        public bool LeftButton { get; set; }
        public bool DoubleClick { get; set; }
        public bool RighrButtonClick { get; set; }

        public bool Rotated { get; set; }
        public bool TurnEnded { get; set; }

        public bool StateChanged { get; set; }

        private BasePlayer _player;
        private FieldsController _fieldsController;
        private CardsController _cardsController;

        private GameObject EndTurnButton { get; set; }

        public HumanPlayerController(BasePlayer player, FieldsController fieldsController, CardsController cardsController)
        {
            _player = player;
            _fieldsController = fieldsController;
            _cardsController = cardsController;

            EndTurnButton = GameObject.Find("EndTurnBtn");
        }

        public void StartMyTurn()
        {
            if (PlayerState == PlayerState.PlayerWait)
            {
                PlayerState = PlayerState.PlayerHoldCard;
            }
        }

        public void MakingMove()
        {
            if (PlayerState == PlayerState.PlayerHoldCard)
            {
                EndTurnButton.SetActive(false);
                PlayerHoldCardProcess(_player);
                return;
            }

            if (PlayerState == PlayerState.PlayerHoldChip)
            {
                EndTurnButton.SetActive(true);
                HoldChipProcess(_player);
                return;
            }
        }

        private void PlayerHoldCardProcess(BasePlayer player)
        {
            // в начале хода тянем карту, если ее нет
            if (_currentCard == null)
            {
                _currentCard = GameManager.Instance.RoomService.GetCurrentCard();
                if (_currentCard == null)
                    return;
            }

            if (DoubleClick)
            {
                DoubleClick = false;

                _selectedFieldId = _fieldsController.GetSelectedFieldId();
                if (_selectedFieldId == null)
                    return;

                // клик на поле левой кнопкой помещает в него карту
                var canPutCard = GameManager.Instance.RoomService.CanPutCard(_selectedFieldId, _currentCard.CardName);
                if (canPutCard)
                {
                    GameManager.Instance.RoomService.PutCard(_selectedFieldId, _currentCard.CardName, player.Name);
                    var player1 = GameManager.Instance.RoomService.GetPlayer(player.Name);
                    if (player1.ChipCount != 0)
                    {
                        _cardsController.ShowCardMarks(_currentCard.CardName);
                    }

                    PlayerState = PlayerState.PlayerHoldChip;
                }
            }
            else if (RighrButtonClick || Rotated)
            {
                RighrButtonClick = false;
                Rotated = false;
                // поворот поля если нажата правая кнопка поворачиваем карту и кладем на поле
                GameManager.Instance.RoomService.RotateCard(_currentCard.CardName);
                _cardsController.ReloadCurrentCard();
            }
        }

        private void HoldChipProcess(BasePlayer player)
        {
            HilightSelectedCardMark(_currentCard.CardName);

            // клик на поле левой кнопкой помещает в него Фишку
            if (LeftButton)
            {
                LeftButton = false;

                var playerHaveChip = GameManager.Instance.RoomService.GetPlayer(player.Name);
                if (playerHaveChip.ChipCount != 0)
                {
                    // установка фишки
                    var partObject = GetSelectedPartUI(_currentCard.CardName);
                    if (partObject != null)
                    {
                        GameManager.Instance.RoomService.PutChip(_currentCard.CardName, partObject, player.Name);
                        EndTurn(player.Name);
                    }
                }
            }

            // клик на поле правой кнопкой означает что игрок не хочет устанавливать фишку.
            if (RighrButtonClick || TurnEnded)
            {
                RighrButtonClick = false;
                TurnEnded = false;
                EndTurn(player.Name);
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

        private void EndTurn(string userName)
        {
            GameManager.Instance.RoomService.EndTurn(userName);
            _cardsController.HideCardMarks(_currentCard.CardName);
            _currentCard = null;
            PlayerState = PlayerState.PlayerWait;
        }
    }
}
