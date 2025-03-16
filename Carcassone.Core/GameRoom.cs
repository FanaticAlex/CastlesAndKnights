using Carcassone.Core.Calculation;
using Carcassone.Core.Cards;
using Carcassone.Core.Extensions;
using Carcassone.Core.Fields;
using Carcassone.Core.Players;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Carcassone.Core
{

    /// <summary>
    /// Store all single game data.
    /// </summary>
    public class GameRoom
    {
        public List<GameMove> Moves { get; set; } = new List<GameMove>();

        public ExtensionsManager ExtensionsManager { get; set; }
        public CardPool CardsPool { get; set; }
        public ScoreCalculator ScoreCalculator { get; set; }
        public FieldBoard FieldBoard { get; set; }
        public PlayersPool PlayersPool { get; set; }

        public string Id { get; }
        public bool IsStarted { get; set; }
        public bool IsFinished { get; set; }

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
            var firstField = FieldBoard.GetField(0, 0);
            var initMove = new GameMove()
            {
                PlayerName = null,
                CardId = firstCard.Id,
                CardRotation = 0,
                FieldId = firstField.Id,
                PartName = null,
            };

            MakeMove(initMove);
        }

        public string Save()
        {
            return JsonConvert.SerializeObject(this);
        }

        public void Load(string save)
        {
            var room = JsonConvert.DeserializeObject<GameRoom>(save) ?? throw new Exception($"Can't load the game from save {save}");
            ExtensionsManager = room.ExtensionsManager;
            CardsPool = room.CardsPool;
            ScoreCalculator = room.ScoreCalculator;
            FieldBoard = room.FieldBoard;
            PlayersPool = room.PlayersPool;
        }

        public PlayerScore GetPlayerScore(BasePlayer player) =>
            ScoreCalculator.GetPlayerScore(player, PlayersPool, CardsPool);

        public Card GetCard(string cardId) => CardsPool.GetCard(cardId);

        /// <summary>
        /// Возвращает поля пригодные для установки карты
        /// </summary>
        /// <param name="cardName"></param>
        /// <returns></returns>
        public List<Field> GetAvailableFields(string cardName)
        {
            var list = new List<Field>();
            if (cardName == null)
                return list;

            var card = GetCard(cardName);
            var fields = FieldBoard.Fields;
            foreach (var field in fields)
            {
                if (CanPutCardInFieldWithRotation(field, card))
                {
                    list.Add(field);
                }
            }

            return list;
        }

        /// <summary>
        /// Возвращает поля в которые уже не могут быть поставлены карты по ходу игры.
        /// </summary>
        /// <returns></returns>
        public List<Field> GetNotAvailableFields()
        {
            var notAvailableCards = new List<Field>();
            var fields = FieldBoard.GetAvailableFields();
            foreach (var field in fields)
            {
                var canPut = false;
                foreach (var card in CardsPool.CardsDeck)
                {
                    if (CanPutCardInFieldWithRotation(field, card))
                        canPut = true;
                }

                if (!canPut)
                    notAvailableCards.Add(field);
            }

            return notAvailableCards;
        }

        public List<ObjectPart> GetAvailableParts(string cardName)
        {
            var card = GetCard(cardName);
            var list = card.Parts.Where(p => !p.IsPartOfOwnedObject).ToList();
            return list;
        }

        public void PutCardInField(Card card, Field field)
        {
            if (card == null)
                throw new Exception("Card can't be null");

            if (field == null)
                throw new Exception("Field can't be null");

            if (!CanPutCardInField(field, card))
                throw new Exception("Card can't be put");

            FieldBoard.PutCard(card, field);
            ScoreCalculator.AddCard(card, field, FieldBoard, CardsPool);
        }

        public void PutChipInCard(ObjectPart partObject, string playerName)
        {
            var player = PlayersPool.GetPlayer(playerName);
            partObject.Chip = player.TakeChip();
        }

        public void MakeMove(GameMove gameMove)
        {
            var field = FieldBoard.GetField(gameMove.FieldId);
            var card = CardsPool.GetCard(gameMove.CardId);
            card.RotateCard(gameMove.CardRotation);
            PutCardInField(card, field);

            var player = PlayersPool.GetPlayer(gameMove.PlayerName);
            var part = card.GetPart(gameMove.PartName);
            if ((player != null) && (part != null))
              PutChipInCard(part, gameMove.PlayerName);

            Moves.Add(gameMove);

            // расчеты
            ScoreCalculator.CloseObjectsAndReturnChips(PlayersPool, CardsPool);

            CardsPool.DiscardCard(card);
            IsFinished = (GetCurrentCard() == null);
            PlayersPool.MoveToNextPlayer();
        }

        public List<Card> GetActiveCards()
        {
            return FieldBoard.Fields
                .Where(f => f.CardName != null)
                .Select(f => CardsPool.GetCard(f.CardName))
                .ToList();
        }

        public List<ObjectPart> GetActiveParts()
        {
            return GetActiveCards()
                .SelectMany(c => c.Parts)
                .Where(p => p.Chip != null || p.Flag != null)
                .ToList();
        }

        /// <summary>
        /// Return card from top if the card pool
        /// </summary>
        /// <returns></returns>
        public Card? GetCurrentCard()
        {
            do
            {
                var topCard = CardsPool.GetTopCard();
                if (CanPlayCard(topCard))
                    return topCard;
                else
                    CardsPool.DiscardCard(topCard);
            } 
            while (!CardsPool.IsEmpty());

            return null;
        }

        private bool CanPlayCard(Card? card)
        {
            if (card == null) return false;

            List<Field> fields = FieldBoard.GetAvailableFields();
            // проверяем можно ли эту карту сыграть, если нет берем следующую
            foreach (var field in fields)
            {
                if (CanPutCardInFieldWithRotation(field, card))
                    return true;
            }

            return false;
        }

        public bool CanPutCardInField(Field field, Card card)
        {
            // if there is another card in field then false
            if (!string.IsNullOrEmpty(field.CardName))
                return false;

            var neighbourTopCardName = FieldBoard.GetNeighbour(field, FieldSide.top)?.CardName;
            Card? neighbourTopCard = neighbourTopCardName != null ? CardsPool.GetCard(neighbourTopCardName) : null;
            var neighbourLeftCardName = FieldBoard.GetNeighbour(field, FieldSide.left)?.CardName;
            Card? neighbourLeftCard = neighbourLeftCardName != null ? CardsPool.GetCard(neighbourLeftCardName) : null;
            var neighbourBottomCardName = FieldBoard.GetNeighbour(field, FieldSide.bottom)?.CardName;
            Card? neighbourBottomCard = neighbourBottomCardName != null ? CardsPool.GetCard(neighbourBottomCardName) : null;
            var neighbourRightCardName = FieldBoard.GetNeighbour(field, FieldSide.right)?.CardName;
            Card? neighbourRightCard = neighbourRightCardName != null ? CardsPool.GetCard(neighbourRightCardName) : null;

            // если есть граничные карты то границы должны совпадать иначе карту присоединить нельзя
            var isRiverCard = card.Id.Contains("W");
            if (isRiverCard)
            {
                bool isTopFree = neighbourTopCard == null;
                bool isLeftFree = neighbourLeftCard == null;
                bool isBottomFree = neighbourBottomCard == null;
                bool isRightFree = neighbourRightCard == null;

                // направелние реки должно быть строго сверху вниз, поворачивать реку наверх нельзя
                bool isWaterDirectionCorrect = !(isTopFree && card.TopEdgeType == CardEdgeType.Water);

                bool connectWithTopWaterCard = (neighbourTopCard?.BottomEdgeType == card.TopEdgeType && card.TopEdgeType == CardEdgeType.Water);
                bool connectWithLeftWaterCard = (neighbourLeftCard?.RightEdgeType == card.LeftEdgeType && card.LeftEdgeType == CardEdgeType.Water);
                bool connectWithBottomWaterCard = (neighbourBottomCard?.TopEdgeType == card.BottomEdgeType && card.BottomEdgeType == CardEdgeType.Water);
                bool connectWithRightWaterCard = (neighbourRightCard?.LeftEdgeType == card.RightEdgeType && card.RightEdgeType == CardEdgeType.Water);

                // водную карту можно положить в поле, если в соседних с полем областях либо нет карт
                // либо водные границы соседних карт совпадают
                if ((isTopFree || connectWithTopWaterCard) &&
                    (isLeftFree || connectWithLeftWaterCard) &&
                    (isBottomFree || connectWithBottomWaterCard) &&
                    (isRightFree || connectWithRightWaterCard) &&
                    isWaterDirectionCorrect)
                {
                    return true;
                }
            }
            else
            {
                // карту можно положить в поле, если в соседних с полем областях либо нет карт
                // либо границы карты которую кладем и соседней карты совпадают
                if ((neighbourTopCard == null || neighbourTopCard.BottomEdgeType == card.TopEdgeType) &&
                    (neighbourLeftCard == null || neighbourLeftCard.RightEdgeType == card.LeftEdgeType) &&
                    (neighbourBottomCard == null || neighbourBottomCard.TopEdgeType == card.BottomEdgeType) &&
                    (neighbourRightCard == null || neighbourRightCard.LeftEdgeType == card.RightEdgeType))
                {
                    return true;
                }
            }

            return false;
        }

        public bool RotateCardTilFit(Field field, Card card)
        {
            for (int i = 1; i < 4; i++) // можно сделать до 3х поворотов 4й - исходное положение
            {
                card.RotateCard();
                if (CanPutCardInField(field, card))
                    return true;
            }

            // доворачиваем до исходного положения если не подходит
            card.RotateCard();
            return false;
        }

        public bool CanPutCardInFieldWithRotation(Field field, Card? card)
        {
            if (!string.IsNullOrEmpty(field.CardName))
                return false;

            if (card == null)
                return false;

            var type = card.GetType();
            var copy = (Card)Activator.CreateInstance(type, card.CardType, card.CardNumber);
            copy.TopEdgeType = card.TopEdgeType;
            copy.LeftEdgeType = card.LeftEdgeType;
            copy.BottomEdgeType = card.BottomEdgeType;
            copy.RightEdgeType = card.RightEdgeType;

            return RotateCardTilFit(field, copy);
        }
    }
}
