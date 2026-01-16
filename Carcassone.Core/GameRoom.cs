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
            var firstTile = GetNextTile() ?? throw new Exception("Ошибка. В колоде нет карт!");
            var firstCell = GameGrid.GetCell(0, 0);
            var initMove = new GameMove()
            {
                PlayerName = null,
                TileId = firstTile.Id,
                TileRotation = 0,
                CellId = firstCell.Id,
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

        public Tile GetTile(string cardId) => TileStack.GetTile(cardId);

        public List<Cell> GetCellsToPutCard(string tileId)
        {
            var list = new List<Cell>();
            if (tileId == null)
                return list;

            var tile = GetTile(tileId);
            var cells = GameGrid.GetAvailableCells();
            foreach (var cell in cells)
            {
                if (CanPutTileInCellWithRotation(cell, tile))
                    list.Add(cell);
            }

            return list;
        }

        public List<Cell> RecalculateNotAvailableCells()
        {
            var emptyCells = GameGrid.GetEmptyCells();
            foreach (var cell in emptyCells)
            {
                var canPut = false;
                foreach (var tile in TileStack.GetRemainTiles())
                {
                    if (CanPutTileInCellWithRotation(cell, tile))
                    {
                        canPut = true;
                        break;
                    }
                }

                if (!canPut)
                    cell.NotAvailable = true;
            }

            return GameGrid.GetUnavailableCells();
        }

        public List<ObjectPart> GetAvailableParts(string tileName)
        {
            var tile = GetTile(tileName);
            var list = tile.Parts.Where(p => !p.IsPartOfOwnedObject).ToList();
            return list;
        }

        public void PutTileInCell(Tile tile, Cell cell)
        {
            if (tile == null)
                throw new Exception("Card can't be null");

            if (cell == null)
                throw new Exception("Cell can't be null");

            if (!CanPutTileInCell(cell, tile))
                throw new Exception("Card can't be put");

            GameGrid.PutTile(tile, cell);
            tile.ConnectCell(cell, GameGrid);
            ScoreCalculator.AddTile(tile, cell, GameGrid, TileStack);
        }

        public void PutChipOnTile(ObjectPart partObject, string playerName)
        {
            var player = PlayersPool.GetPlayer(playerName);
            if (player == null) throw new NullReferenceException("Player not found: " + playerName);

            partObject.Chip = player.TakeChip();
        }

        public void MakeMove(GameMove gameMove)
        {
            if (gameMove == null) throw new ArgumentNullException("Move obj can not be null");

            var cell = GameGrid.GetCell(gameMove.CellId);
            var tile = TileStack.GetTile(gameMove.TileId);
            tile.RotateCard(gameMove.TileRotation);
            PutTileInCell(tile, cell);

            if ((gameMove.PlayerName != null) && (gameMove.PartName != null))
            {
                var part = tile.GetPart(gameMove.PartName);
                PutChipOnTile(part, gameMove.PlayerName);
            }

            Moves.Add(gameMove);

            // расчеты
            ScoreCalculator.CloseObjectsAndReturnChips(PlayersPool, TileStack);

            TileStack.DiscardTile(tile);
            TileStack.CurrentCard = GetNextTile();

            if (TileStack.CurrentCard == null)
            {
                IsFinished = true;
                Finished?.Invoke(this, null);
            }

            if (gameMove.PlayerName != null)
                PlayersPool.MoveToNextPlayer();
        }

        public List<Tile> GetActiveTiles()
        {
            return GameGrid.Cells
                .Where(c => c.IsContainingTile())
                .Select(c => TileStack.GetTile(c.CardName))
                .ToList();
        }

        public List<ObjectPart> GetActiveParts()
        {
            return GetActiveTiles()
                .SelectMany(t => t.Parts)
                .Where(p => p.Chip != null || p.Flag != null)
                .ToList();
        }

        private Tile? GetNextTile()
        {
            do
            {
                var topTile = TileStack.GetTopTile();
                if (CanPlayTile(topTile))
                    return topTile;
                else
                    TileStack.DiscardTile(topTile);
            } 
            while (!TileStack.IsEmpty());

            return null;
        }

        private bool CanPlayTile(Tile? tile)
        {
            if (tile == null) return false;

            List<Cell> emptyCells = GameGrid.GetEmptyCells();
            // проверяем можно ли эту карту сыграть, если нет берем следующую
            foreach (var cell in emptyCells)
            {
                if (CanPutTileInCellWithRotation(cell, tile))
                    return true;
            }

            return false;
        }

        public bool CanPutTileInCellWithRotation(Cell cell, Tile? tile)
        {
            if (cell.IsContainingTile()) return false;

            if (tile == null) return false;

            // чтобы не поворачивать оригинальную карту поворачиваем копию
            var type = tile.GetType();
            var copy = (Tile)Activator.CreateInstance(type, tile.CardType, tile.CardNumber);
            copy.TopEdgeType = tile.TopEdgeType;
            copy.LeftEdgeType = tile.LeftEdgeType;
            copy.BottomEdgeType = tile.BottomEdgeType;
            copy.RightEdgeType = tile.RightEdgeType;

            return RotateTileTilFit(cell, copy);
        }

        public bool RotateTileTilFit(Cell cell, Tile tile)
        {
            for (int i = 0; i < 4; i++) // можно сделать до 4х поворотов 4й - исходное положение (в конце)
            {
                tile.RotateCard();
                if (CanPutTileInCell(cell, tile))
                    return true;
            }

            return false;
        }

        private bool CanPutTileInCell(Cell cell, Tile tile)
        {
            var result = true;
            foreach(var extension in Extensions)
                result &= extension.CanPutTileInCell(cell, tile, GameGrid, TileStack);

            return result;
        }
    }
}
