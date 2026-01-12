using Carcassone.Core.Calculation;
using Carcassone.Core.Tiles;
using Carcassone.Core.Extensions;
using Carcassone.Core.Board;
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
        public Stack CardsPool { get; set; }
        public ScoreCalculator ScoreCalculator { get; set; }
        public Grid GameGrid { get; set; }
        public GamePlayersPool PlayersPool { get; set; }

        public string Id { get; }
        public bool IsStarted { get; set; }
        public bool IsFinished { get; set; }

        public event EventHandler Finished;

        public GameRoom()
        {
            Id = Guid.NewGuid().ToString();

            ExtensionsManager = new ExtensionsManager(true);

            CardsPool = new Stack(ExtensionsManager);
            ScoreCalculator = new ScoreCalculator();
            GameGrid = new Grid();
            PlayersPool = new GamePlayersPool();
        }

        /// <summary>
        /// Start the game. Set first card.
        /// </summary>
        public void Start()
        {
            IsStarted = true;

            // инициализирующий ход
            var firstCard = GetNextCard() ?? throw new Exception("Ошибка. В колоде нет карт!");
            var firstField = GameGrid.GetField(0, 0);
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
            GameGrid = room.GameGrid;
            PlayersPool = room.PlayersPool;
        }

        public PlayerScore GetPlayerScore(string playerName) =>
            ScoreCalculator.GetPlayerScore(playerName, PlayersPool, CardsPool);

        public Tile GetCard(string cardId) => CardsPool.GetCard(cardId);

        public List<Cell> GetFieldsToPutCard(string cardId)
        {
            var list = new List<Cell>();
            if (cardId == null)
                return list;

            var card = GetCard(cardId);
            var fields = GameGrid.GetAvailableCells();
            foreach (var field in fields)
            {
                if (CanPutCardInFieldWithRotation(field, card))
                    list.Add(field);
            }

            return list;
        }

        public List<Cell> RecalculateNotAvailableFields()
        {
            var emptyFields = GameGrid.GetEmptyFields();
            foreach (var field in emptyFields)
            {
                var canPut = false;
                foreach (var card in CardsPool.GetRemainTiles())
                {
                    if (CanPutCardInFieldWithRotation(field, card))
                    {
                        canPut = true;
                        break;
                    }
                }

                if (!canPut)
                    field.NotAvailable = true;
            }

            return GameGrid.GetUnavailableFields();
        }

        public List<ObjectPart> GetAvailableParts(string cardName)
        {
            var card = GetCard(cardName);
            var list = card.Parts.Where(p => !p.IsPartOfOwnedObject).ToList();
            return list;
        }

        public void PutCardInField(Tile card, Cell field)
        {
            if (card == null)
                throw new Exception("Card can't be null");

            if (field == null)
                throw new Exception("Field can't be null");

            if (!CanPutCardInField(field, card))
                throw new Exception("Card can't be put");

            GameGrid.PutCard(card, field);
            ScoreCalculator.AddCard(card, field, GameGrid, CardsPool);
        }

        public void PutChipInCard(ObjectPart partObject, string playerName)
        {
            var player = PlayersPool.GetPlayer(playerName);
            if (player == null) throw new NullReferenceException("Player not found: " + playerName);

            partObject.Chip = player.TakeChip();
        }

        public void MakeMove(GameMove gameMove)
        {
            if (gameMove == null) throw new ArgumentNullException("Move obj can not be null");

            var field = GameGrid.GetCell(gameMove.FieldId);
            var card = CardsPool.GetCard(gameMove.CardId);
            card.RotateCard(gameMove.CardRotation);
            PutCardInField(card, field);

            if ((gameMove.PlayerName != null) && (gameMove.PartName != null))
            {
                var part = card.GetPart(gameMove.PartName);
                PutChipInCard(part, gameMove.PlayerName);
            }

            Moves.Add(gameMove);

            // расчеты
            ScoreCalculator.CloseObjectsAndReturnChips(PlayersPool, CardsPool);

            CardsPool.DiscardCard(card);
            CardsPool.CurrentCard = GetNextCard();

            if (CardsPool.CurrentCard == null)
            {
                IsFinished = true;
                Finished?.Invoke(this, null);
            }

            if (gameMove.PlayerName != null)
                PlayersPool.MoveToNextPlayer();
        }

        public List<Tile> GetActiveCards()
        {
            return GameGrid.Cells
                .Where(f => f.IsContainsCard())
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

        private Tile? GetNextCard()
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

        private bool CanPlayCard(Tile? card)
        {
            if (card == null) return false;

            List<Cell> emptyFields = GameGrid.GetEmptyFields();
            // проверяем можно ли эту карту сыграть, если нет берем следующую
            foreach (var field in emptyFields)
            {
                if (CanPutCardInFieldWithRotation(field, card))
                    return true;
            }

            return false;
        }

        public bool CanPutCardInField(Cell field, Tile card)
        {
            if (field.IsContainsCard()) return false;

            var neighbourTopCardName = GameGrid.GetNeighbour(field, CellSide.top)?.CardName;
            Tile? neighbourTopCard = neighbourTopCardName != null ? CardsPool.GetCard(neighbourTopCardName) : null;
            var neighbourLeftCardName = GameGrid.GetNeighbour(field, CellSide.left)?.CardName;
            Tile? neighbourLeftCard = neighbourLeftCardName != null ? CardsPool.GetCard(neighbourLeftCardName) : null;
            var neighbourBottomCardName = GameGrid.GetNeighbour(field, CellSide.bottom)?.CardName;
            Tile? neighbourBottomCard = neighbourBottomCardName != null ? CardsPool.GetCard(neighbourBottomCardName) : null;
            var neighbourRightCardName = GameGrid.GetNeighbour(field, CellSide.right)?.CardName;
            Tile? neighbourRightCard = neighbourRightCardName != null ? CardsPool.GetCard(neighbourRightCardName) : null;

            // если есть граничные карты то границы должны совпадать иначе карту присоединить нельзя
            var isRiverCard = card.Id.Contains("W");
            if (isRiverCard)
            {
                bool isTopFree = neighbourTopCard == null;
                bool isLeftFree = neighbourLeftCard == null;
                bool isBottomFree = neighbourBottomCard == null;
                bool isRightFree = neighbourRightCard == null;

                // проверям направление реки
                bool isWaterDirectionTop = (isTopFree && card.TopEdgeType == CardEdgeType.Water);
                bool isWaterDirectionRight = (isRightFree && card.RightEdgeType == CardEdgeType.Water);

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
                    !isWaterDirectionTop &&
                    !isWaterDirectionRight)
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

        public bool RotateCardTilFit(Cell field, Tile card)
        {
            for (int i = 0; i < 4; i++) // можно сделать до 4х поворотов 4й - исходное положение (в конце)
            {
                card.RotateCard();
                if (CanPutCardInField(field, card))
                    return true;
            }

            return false;
        }

        public bool CanPutCardInFieldWithRotation(Cell field, Tile? card)
        {
            if (field.IsContainsCard()) return false;

            if (card == null) return false;

            // чтобы не поворачивать оригинальную карту поворачиваем копию
            var type = card.GetType();
            var copy = (Tile)Activator.CreateInstance(type, card.CardType, card.CardNumber);
            copy.TopEdgeType = card.TopEdgeType;
            copy.LeftEdgeType = card.LeftEdgeType;
            copy.BottomEdgeType = card.BottomEdgeType;
            copy.RightEdgeType = card.RightEdgeType;

            return RotateCardTilFit(field, copy);
        }
    }
}
