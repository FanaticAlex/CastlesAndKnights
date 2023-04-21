using Carcassone.Core.Calculation;
using Carcassone.Core.Cards;
using Carcassone.Core.Extensions;
using Carcassone.Core.Fields;
using Carcassone.Core.Players;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carcassone.Core
{
    /// <summary>
    /// Store all game data.
    /// </summary>
    public class GameRoom
    {
        public ExtensionsManager ExtensionsManager { get; set; }
        public CardPool CardsPool { get; set; }
        public ScoreCalculator ScoreCalculator { get; set; }
        public FieldBoard FieldBoard { get; set; }
        public PlayersPool PlayersPool { get; set; }

        public string Id { get; }
        public bool IsStarted { get; set; }

        private bool _isFinished;
        public bool IsFinished
        {
            get
            {
                return _isFinished;
            }
            set
            {
                _isFinished = value;
                Finished?.Invoke(null, this);
            }
        }

        public event EventHandler<GameRoom>? Finished;

        public GameRoom()
        {
            Id = Guid.NewGuid().ToString();

            ExtensionsManager = new ExtensionsManager(true);

            CardsPool = new CardPool(ExtensionsManager);
            ScoreCalculator = new ScoreCalculator();
            FieldBoard = new FieldBoard();
            PlayersPool = new PlayersPool();
        }

        /// <summary>
        /// Start the game. Set first card.
        /// </summary>
        public void Start()
        {
            IsStarted = true;

            // инициализирующий ход
            var firstCard = GetCurrentCard() ?? throw new Exception("Ошибка. В колоде нет карт!");
            var firstField = FieldBoard.GetCenter();
            PutCardInField(firstCard, firstField);

            PlayersPool.MoveNextPlayer(this);
        }

        public string Save()
        {
            return JsonConvert.SerializeObject(this);
        }

        public void Load(string save)
        {
            var room = JsonConvert.DeserializeObject<GameRoom>(save);
            ExtensionsManager = room.ExtensionsManager;
            CardsPool = room.CardsPool;
            ScoreCalculator = room.ScoreCalculator;
            FieldBoard = room.FieldBoard;
            PlayersPool = room.PlayersPool;
        }

        public PlayerScore GetPlayerScore(BasePlayer player) =>
            ScoreCalculator.GetPlayerScore(player, PlayersPool, CardsPool);

        public int GetCardsRemain() => GetCardsRemainInPool().Count();
        public void RotateCard(string cardId) => GetCard(cardId).RotateCard();
        public Card GetCard(string cardId) => CardsPool.GetCard(cardId);
        public bool CanPutCard(string fieldId, string cardId)
        {
            var field = FieldBoard.GetField(fieldId);
            var card = GetCard(cardId);
            return field.CanPutCardInThisField(card, FieldBoard, CardsPool);
        }

        public List<Field> GetAvailableFields(string cardName)
        {
            var list = new List<Field>();
            if (cardName == null)
                return list;

            var card = GetCard(cardName);
            var fields = FieldBoard.Fields;
            foreach (var field in fields)
            {
                if (field.CanPutCardInThisFieldWithRotation(card, FieldBoard, CardsPool))
                {
                    list.Add(field);
                }
            }

            return list;
        }

        public List<Field> GetNotAvailableFields()
        {
            var list = new List<Field>();
            var fields = FieldBoard.Fields;
            foreach (var field in fields)
            {
                var canPut = false;
                foreach (var card in GetCardsRemainInPool())
                {
                    if (field.CanPutCardInThisFieldWithRotation(card, FieldBoard, CardsPool))
                    {
                        canPut = true;
                    }
                }

                if (!canPut)
                    list.Add(field);
            }

            return list;
        }

        public List<ObjectPart> GetAvailableParts(string cardName)
        {
            var card = GetCard(cardName);
            var list = card.Parts.Where(p => !p.IsPartOfOwnedObject).ToList();
            return list;
        }

        public void PutCardInField(Card card, Field field)
        {
            FieldBoard.PutCard(card, field);
            ScoreCalculator.AddCard(card, field, FieldBoard, CardsPool);
        }

        public void PutChipInCard(ObjectPart partObject, string playerName)
        {
            var player = PlayersPool.GetPlayer(playerName);
            partObject.Chip = player.TakeChip();
        }

        public void EndTurn()
        {
            ScoreCalculator.CloseObjectsAndReturnChips(PlayersPool, CardsPool);

            var card = GetCurrentCard();
            if (card == null)
                IsFinished = true;

            PlayersPool.MoveNextPlayer(this);
        }

        public List<Card> GetActiveCards()
        {
            return FieldBoard.Fields
                .Where(f => f.CardName != null)
                .Select(f => CardsPool.GetCard(f.CardName))
                .ToList();
        }

        /// <summary>
        /// Return card from top if the card pool
        /// </summary>
        /// <returns></returns>
        public Card? GetCurrentCard()
        {
            List<Field> fields = FieldBoard.GetAvailableFields();
            var cardsRemainInPool = GetCardsRemainInPool();
            foreach (var card in cardsRemainInPool)
            {
                // проверяем можно ли эту карту сыграть, если нет берем следующую
                foreach (var field in fields)
                {
                    if (field.CanPutCardInThisFieldWithRotation(card, FieldBoard, CardsPool))
                        return card;
                }
            }

            return null;
        }

        private List<Card> GetCardsRemainInPool()
        {
            var activeCardsNames = GetActiveCards().Select(c => c.CardId);
            return CardsPool.AllCards.Where(c => !activeCardsNames.Contains(c.CardId)).ToList();
        }
    }
}
