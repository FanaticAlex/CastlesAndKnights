using Carcassone.Core.Calculation;
using Carcassone.Core.Calculation.Objects;
using Carcassone.Core.Cards;
using Carcassone.Core.Fields;
using Carcassone.Core.Players;
using Carcassone.Core.Players.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Carcassone.Core
{
    /// <summary>
    /// Store all game data.
    /// </summary>
    public class GameRoom
    {
        private CardPool _cardsPool;
        private ScoreCalculator _scoreCalculator;
        private FieldBoard _fieldBoard;
        private PlayersPool _playersPool;

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
            _cardsPool = new CardPool();
            _scoreCalculator = new ScoreCalculator();
            _fieldBoard = new FieldBoard();
            _playersPool = new PlayersPool();
        }

        /// <summary>
        /// Start the game. Set first card.
        /// </summary>
        public void Start()
        {
            IsStarted = true;

            // инициализирующий ход
            var firstCard = _cardsPool.GetCurrentCard(_fieldBoard);
            if (firstCard == null)
                throw new Exception("Ошибка. В колоде нет карт!");

            var firstField = _fieldBoard.GetCenter();
            PutCardInField(firstCard, firstField);

            _playersPool.MoveNextPlayer(this);
        }

        public Player AddHumanPlayer(string playerName) => _playersPool.AddHumanPlayer(playerName);
        public void AddAIPlayer() => _playersPool.AddAIPlayerEasy();
        public List<BasePlayer> GetPlayers() => _playersPool.Players;
        public BasePlayer? GetPlayer(string playerName) => _playersPool.GetPlayer(playerName);
        public void DeletePlayer(string playerName) => _playersPool.DeletePlayer(playerName);
        public BasePlayer GetCurrentPlayer() => _playersPool.CurrentPlayer;


        public List<Road> GetRoads() => _scoreCalculator.Roads;
        public List<Castle> GetCastles() => _scoreCalculator.Castles;
        public List<Cornfield> GetCornfields() => _scoreCalculator.Cornfields;
        public List<Church> GetChurches() => _scoreCalculator.Churches;
        public PlayerScore GetPlayerScore(BasePlayer player) => _scoreCalculator.GetPlayerScore(player, this);


        public List<Card> GetAllCards() => _cardsPool.GetAllCards();
        public int GetCardsRemain() => _cardsPool.GetCardsRemainInPool().Count();
        public Card? GetCurrentCard() => _cardsPool.GetCurrentCard(_fieldBoard);
        public void RotateCard(string cardName) => GetCard(cardName).RotateCard();
        public Card GetCard(string cardName) => _cardsPool.GetCard(cardName);
        public bool CanPutCard(string fieldId, string cardId) => GetField(fieldId).CanPutCardInThisField(GetCard(cardId));


        public Field GetField(string fieldId) => _fieldBoard.GetField(fieldId);
        public List<Field> GetFields() => _fieldBoard.GetAllFields();


        public List<Field> GetAvailableFields(string cardName)
        {
            var list = new List<Field>();
            if (cardName == null)
                return list;

            var card = GetCard(cardName);
            var fields = GetFields();
            foreach (var field in fields)
            {
                if (field.CanPutCardInThisFieldWithRotation(card))
                {
                    list.Add(field);
                }
            }

            return list;
        }

        public List<Field> GetNotAvailableFields()
        {
            var list = new List<Field>();
            var fields = GetFields();
            foreach (var field in fields)
            {
                var canPut = false;
                foreach (var card in _cardsPool.GetCardsRemainInPool())
                {
                    if (field.CanPutCardInThisFieldWithRotation(card))
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
            var list = card.Parts.Where(p => !p.IsOwned).ToList();
            return list;
        }

        public void PutCardInField(Card card, Field field)
        {
            _fieldBoard.PutCard(card, field);
            _scoreCalculator.AddCard(card);
        }

        public void PutChipInCard(ObjectPart partObject, BasePlayer player)
        {
            partObject.Chip = player.TakeChip();
        }

        public void EndTurn()
        {
            _scoreCalculator.CloseObjectsAndReturnChips();

            var card = GetCurrentCard();
            if (card == null)
            {
                IsFinished = true;
                return;
            }

            _playersPool.MoveNextPlayer(this);
        }
    }
}
