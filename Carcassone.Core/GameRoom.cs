using Carcassone.Core.Board;
using Carcassone.Core.Calculation;
using Carcassone.Core.Calculation.Base;
using Carcassone.Core.Calculation.RiverExtension;
using Carcassone.Core.Players;
using Carcassone.Core.Tiles;
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

        /// <summary>
        /// Extensions contains rules of the game.
        /// </summary>
        private List<IGameExtension> Extensions { get; } = new List<IGameExtension>();

        public TileStack TileStack { get; set; }
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

            Extensions.Add(new BaseRules());
            Extensions.Add(new RiverExtension());

            // compose stack from all tiles of all extensions
            TileStack = new Tiles.TileStack();
            foreach (var extension in Extensions)
                extension.AddTiles(TileStack);

            TileStack.Shaffle();

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
            TileStack = room.TileStack;
            ScoreCalculator = room.ScoreCalculator;
            GameGrid = room.GameGrid;
            PlayersPool = room.PlayersPool;
        }

        public PlayerScore GetPlayerScore(string playerName) =>
            ScoreCalculator.GetPlayerScore(playerName, PlayersPool, TileStack);

        public Tile GetCard(string cardId) => TileStack.GetCard(cardId);

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
                foreach (var card in TileStack.GetRemainTiles())
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

        public void PutTileInCell(Tile card, Cell field)
        {
            if (card == null)
                throw new Exception("Card can't be null");

            if (field == null)
                throw new Exception("Field can't be null");

            if (!CanPutCardInField(field, card))
                throw new Exception("Card can't be put");

            GameGrid.PutCard(card, field);
            ScoreCalculator.AddCard(card, field, GameGrid, TileStack);
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
            var card = TileStack.GetCard(gameMove.CardId);
            card.RotateCard(gameMove.CardRotation);
            PutTileInCell(card, field);

            if ((gameMove.PlayerName != null) && (gameMove.PartName != null))
            {
                var part = card.GetPart(gameMove.PartName);
                PutChipInCard(part, gameMove.PlayerName);
            }

            Moves.Add(gameMove);

            // расчеты
            ScoreCalculator.CloseObjectsAndReturnChips(PlayersPool, TileStack);

            TileStack.DiscardCard(card);
            TileStack.CurrentCard = GetNextCard();

            if (TileStack.CurrentCard == null)
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
                .Select(f => TileStack.GetCard(f.CardName))
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
                var topCard = TileStack.GetTopCard();
                if (CanPlayCard(topCard))
                    return topCard;
                else
                    TileStack.DiscardCard(topCard);
            } 
            while (!TileStack.IsEmpty());

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

        private bool CanPutCardInField(Cell cell, Tile tile)
        {
            var result = true;
            foreach(var extension in Extensions)
                result &= extension.CanPutCardInField(cell, tile, GameGrid, TileStack);

            return result;
        }
    }
}
