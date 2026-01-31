using Carcassone.Core.Board;
using Carcassone.Core.Calculation;
using Carcassone.Core.Calculation.Base;
using Carcassone.Core.Calculation.Base.Cities;
using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.Base.Monasteries;
using Carcassone.Core.Calculation.Base.Roads;
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
    /// Store all game data.
    /// </summary>
    public class GameRoom
    {
        private List<GameMove> Moves { get; set; } = new List<GameMove>();

        /// <summary>
        /// Extensions contain rules of the game.
        /// </summary>
        private List<IGameExtension> Extensions { get; } = new List<IGameExtension>();

        public TileStack TileStack { get; set; }
        public Grid GameGrid { get; set; }
        public GamePlayersPool PlayersPool { get; set; }

        public string Id { get; }
        public bool IsStarted { get; set; }
        public bool IsFinished { get; set; }

        public event EventHandler Finished;

        public GameRoom()
        {
            Id = Guid.NewGuid().ToString();

            GameGrid = new Grid();
            PlayersPool = new GamePlayersPool();

            Extensions.Add(new BaseRules(GameGrid));
            Extensions.Add(new RiverExtension());

            // compose stack from all tiles of all extensions
            TileStack = new Tiles.TileStack();
            foreach (var extension in Extensions)
                extension.AddTiles(TileStack);

            TileStack.Shaffle();
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
            GameGrid = room.GameGrid;
            PlayersPool = room.PlayersPool;
        }

        public IEnumerable<BaseGameObject> GetAllGameObjects()
        {
            return Extensions.SelectMany(e => e.Managers.SelectMany(m => m.GetGameObjects()));
        }

        public IEnumerable<City> GetAllCities()
        {
            return GetAllGameObjects().Where(o => o is City).Select(o => (City)o);
        }
        public IEnumerable<Road> GetAllRoads()
        {
            return GetAllGameObjects().Where(o => o is Road).Select(o => (Road)o);
        }

        public IEnumerable<Farm> GetAllFarms()
        {
            return GetAllGameObjects().Where(o => o is Farm).Select(o => (Farm)o);
        }

        public IEnumerable<Monastery> GetAllMonastery()
        {
            return GetAllGameObjects().Where(o => o is Monastery).Select(o => (Monastery)o);
        }

        public PlayerScore GetPlayerScore(string playerName)
        {
            var scores = GetPlayersScores(PlayersPool, TileStack);
            var score = scores.Single(s => s.PlayerName == playerName);
            return score;
        }

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

        public List<ObjectPart> GetAvailableParts(string tileID)
        {
            var tile = GetTile(tileID);
            var partsOfOwnedObjects = GetAllGameObjects().Where(o => HasOwnerHelper.HasOwner(o)).SelectMany(o => o.Parts);
            var list = new List<ObjectPart>();
            foreach (var part in tile.Parts)
            {
                if (!(partsOfOwnedObjects.Contains(part)))
                list.Add(part);
            }

            return list;
        }

        public void MakeMove(GameMove gameMove)
        {
            if (gameMove == null) throw new ArgumentNullException("Move obj can not be null");
            
            var cell = GameGrid.GetCell(gameMove.CellId);
            if (cell == null) throw new Exception("Cell can't be null");

            var tile = TileStack.GetTile(gameMove.TileId);
            if (tile == null) throw new Exception("Card can't be null");


            // PutChipOnTile
            if ((gameMove.PlayerName != null) && (gameMove.PartName != null))
            {
                var player = PlayersPool.GetPlayer(gameMove.PlayerName);
                if (player == null) throw new NullReferenceException("Player not found: " + gameMove.PlayerName);

                var part = tile.GetPart(gameMove.PartName);
                part.Chip = player.TakeChip();
            }


            tile.RotateCard(gameMove.TileRotation);


            if (!CanPutTileInCell(cell, tile))
                throw new Exception("Card can't be put");

            GameGrid.PutTile(tile, cell);
            AddTile(tile, cell);

            Moves.Add(gameMove);

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
                .Select(c => c.Tile)
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
                tile.RotateTile();
                if (CanPutTileInCell(cell, tile))
                    return true;
            }

            return false;
        }

        private bool CanPutTileInCell(Cell cell, Tile tile)
        {
            var result = true;
            foreach(var extension in Extensions)
                result &= extension.CanPutTileInCell(cell, tile, GameGrid);

            return result;
        }

        /// <summary>
        /// Add tile and update objects
        /// </summary>
        /// <param name="tile"></param>
        public void AddTile(Tile tile, Cell cell)
        {
            foreach (ObjectPart part in tile.Parts)
                foreach (var manager in Extensions.SelectMany(e => e.Managers))
                    manager.ProcessPart(part, cell);
        }

        private IEnumerable<PlayerScore> GetPlayersScores(GamePlayersPool plyersPool, TileStack cardPool)
        {
            var scores = new List<PlayerScore>();
            foreach (var player in plyersPool.GamePlayers)
            {
                var overallScore = 0;
                foreach (var manager in Extensions.SelectMany(e => e.Managers))
                {
                    overallScore += manager.GetPlayerScore(player);
                }

                var score = new PlayerScore()
                {
                    PlayerName = player.Name,
                    OverallScore = overallScore,
                    ChipCount = player.СhipList.Count
                };
                scores.Add(score);
            }

            scores.Sort(delegate (PlayerScore x, PlayerScore y)
            {
                return y.OverallScore.CompareTo(x.OverallScore);
            });

            foreach (var score in scores)
            {
                score.Rank = scores.IndexOf(score);
            }

            return scores;
        }
    }
}
