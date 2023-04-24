using System.Linq;
using UnityEngine;
using Carcassone.ApiClient;
using UnityEngine.UI;

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

        public bool DoubleClick { get; set; }

        public bool Rotated { get; set; }
        public bool TurnEnded { get; set; }

        public bool StateChanged { get; set; }

        private string _playerName;
        private FieldsController _fieldsController;
        private CardsController _cardsController;

        private GameObject EndTurnButton { get; set; }

        public HumanPlayerController(string player, FieldsController fieldsController, CardsController cardsController)
        {
            _playerName = player;
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
                EndTurnButton.GetComponent<Button>().interactable = false;
                PlayerHoldCardProcess(_playerName);
                return;
            }

            if (PlayerState == PlayerState.PlayerHoldChip)
            {
                EndTurnButton.GetComponent<Button>().interactable = true;
                HoldChipProcess(_playerName);
                return;
            }
        }

        private void PlayerHoldCardProcess(string playerName)
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
                {
                    Logger.Info("No field selected");
                    return;
                }

                // клик на поле левой кнопкой помещает в него карту
                var canPutCard = GameManager.Instance.RoomService.CanPutCard(_selectedFieldId, _currentCard.CardId);
                if (canPutCard)
                {
                    GameManager.Instance.RoomService.PutCard(_selectedFieldId, _currentCard.CardId, playerName);
                    var player = GameManager.Instance.RoomService.GetPlayer(playerName);

                    var parts = GameManager.Instance.RoomService.GetAvailableObjectParts(_currentCard.CardId);
                    if (!parts.Any())
                    {
                        // если нет вариантов установки фишки сразу завершаем ход
                        EndTurn(playerName);
                        return;
                    }

                    if (player.ChipCount == 0)
                    {
                        // если фишек нет сразу завершаем ход
                        EndTurn(playerName);
                        return;
                    }

                    _cardsController.ShowCardMarks(_currentCard.CardId);
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
                GameManager.Instance.RoomService.RotateCard(_currentCard.CardId);
                _cardsController.ReloadCurrentCard();
            }
        }

        private void HoldChipProcess(string playerName)
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
                    var partObject = GetSelectedPartUI(_currentCard.CardId);
                    if (partObject != null)
                    {
                        GameManager.Instance.RoomService.PutChip(_currentCard.CardId, partObject, playerName);
                        EndTurn(playerName);
                    }
                }
            }

            // клик на поле правой кнопкой означает что игрок не хочет устанавливать фишку.
            if (TurnEnded)
            {
                TurnEnded = false;
                EndTurn(playerName);
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

        private void EndTurn(string userName)
        {
            GameManager.Instance.RoomService.EndTurn(userName);
            _cardsController.HideCardMarks(_currentCard.CardId);
            _currentCard = null;
            PlayerState = PlayerState.PlayerWait;
        }
    }
}
