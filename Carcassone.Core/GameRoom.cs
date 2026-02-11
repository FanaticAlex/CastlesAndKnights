using Carcassone.Core.Board;
using Carcassone.Core.Calculation;
using Carcassone.Core.Calculation.Base;
using Carcassone.Core.Calculation.Base.Cities;
using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.Base.Monasteries;
using Carcassone.Core.Calculation.Base.Roads;
using Carcassone.Core.Calculation.RiverExtension;
using Carcassone.Core.Calculation.RiverExtension.Rivers;
using Carcassone.Core.Players;
using Carcassone.Core.Tiles;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace Carcassone.Core
{
    public enum Extension
    { 
        River
    }

    /// <summary>
    /// Store all game data.
    /// </summary>
    public class GameRoom
    {
        private List<GameMove> _moves = new List<GameMove>();
        private List<IGameRules> _rules;
        private TileStack _tileStack;
        private Grid _gameGrid;
        private GamePlayersPool _playersPool;

        public string Id { get; }
        public bool IsFinished { get; set; }

        public event EventHandler Finished;

        private IEnumerable<Extension> _extentions;
        private List<PlayerInfo> _players;

        public GameRoom(IEnumerable<Extension> extentions, List<PlayerInfo> players)
        {
            _extentions = extentions;
            _players = players;

            Id = Guid.NewGuid().ToString();
            _rules = new List<IGameRules>();
            _tileStack = new TileStack();
            _gameGrid = new Grid();
            _playersPool = new GamePlayersPool(players);

            _rules.Add(new BaseRules(_gameGrid));

            if (extentions.Contains(Extension.River))
                _rules.Add(new RiverExtension());

            // compose stack from all tiles of all extensions
            foreach (var extension in _rules)
                extension.AddTiles(_tileStack);

            _tileStack.Shaffle();
        }

        public GameRoom Copy()
        {
            var copy = new GameRoom(_extentions, _players);
            foreach (var move in _moves)
                copy.MakeMove(move);

            return copy;
        }

        public IEnumerable<BaseGameObject> GetAllGameObjects()
        {
            return _rules.SelectMany(e => e.Managers.SelectMany(m => m.GetGameObjects()));
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

        public GamePlayer GetCurrentPlayer()
        {
            return _playersPool.GetCurrentPlayer();
        }

        public PlayerScore GetPlayerScore(string playerName)
        {
            var scores = GetPlayersScores();
            var score = scores.Single(s => s.PlayerName == playerName);
            return score;
        }

        public IEnumerable<PlayerScore> GetPlayersScores()
        {
            var scores = new List<PlayerScore>();
            foreach (var player in _playersPool.GamePlayers)
            {
                var playerObjects = GetAllGameObjects()
                                    .Where(o => o.IsPlayerOwner(player.Info.Name));

                var score = new PlayerScore()
                {
                    PlayerName = player.Info.Name,
                    OverallScore = playerObjects.Select(o => o.GetScore()).Sum(),
                    ChipCount = player.Info.MeeplesCount,
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

        public Tile GetTile(string cardId) => _tileStack.GetTile(cardId);

        public int GetRemainTilesCount()
        {
            return _tileStack.GetRemainTiles().Count;
        }

        public List<GameMove> GetAvailableMoves()
        {
            var gameMoves = new List<GameMove>();
            var tile = _tileStack.GetTopTile();
            var cells = GetCellsToPutTile(tile);
            foreach (var cell in cells)
            {
                for (var rotation = 0; rotation < 4; rotation++)
                {
                    tile.RotateCard(rotation);
                    if (CanPutTileInCell(cell, tile))
                    {
                        // without meeple
                        var move0 = new GameMove();
                        move0.CellId = cell.Id;
                        move0.TileId = tile.Id;
                        move0.TileRotation = rotation;
                        move0.PartName = null;
                        move0.PlayerName = _playersPool.GetCurrentPlayer().Info.Name;
                        gameMoves.Add(move0);

                        // with meeple
                        var parts = GetAvailableParts(cell, tile);
                        foreach (var part in parts)
                        {
                            var move = new GameMove();
                            move.CellId = cell.Id;
                            move.TileId = tile.Id;
                            move.TileRotation = rotation;
                            move.PartName = part.PartName;
                            move.PlayerName = _playersPool.GetCurrentPlayer().Info.Name;
                            gameMoves.Add(move);
                        }
                    }
                }

                tile.RotateCard(1); // rotate to initial position
            }

            return gameMoves;
        }

        private List<Cell> GetCellsToPutTile(Tile tile)
        {
            var list = new List<Cell>();
            if (tile == null)
                return list;

            var cells = _gameGrid.GetAvailableCells();
            foreach (var cell in cells)
            {
                if (CanPutTileInCellWithRotation(cell, tile))
                    list.Add(cell);
            }

            return list;
        }

        private List<ObjectPart> GetAvailableParts(Cell cell, Tile tile)
        {
            var copy = this.Copy();
            copy.AddTile(cell, tile);

            var partsOfOwnedObjects = copy.GetAllGameObjects()
                .Where(o => HasOwnerHelper.HasOwner(o))
                .SelectMany(o => o.Parts)
                .Select(p => p.PartId);

            var list = new List<ObjectPart>();
            foreach (var part in tile.Parts)
            {
                if (!(partsOfOwnedObjects.Contains(part.PartId)))
                    list.Add(part);
            }

            return list;
        }

        public List<Cell> RecalculateNotAvailableCells()
        {
            var emptyCells = _gameGrid.GetEmptyCells();
            foreach (var cell in emptyCells)
            {
                var canPut = false;
                foreach (var tile in _tileStack.GetRemainTiles())
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

            return _gameGrid.GetUnavailableCells();
        }

        public void MakeMove(GameMove gameMove)
        {
            if (gameMove == null) throw new ArgumentNullException("Move obj can not be null");
            
            var cell = _gameGrid.GetCell(gameMove.CellId);
            if (cell == null) throw new Exception("Cell can't be null");

            var tile = _tileStack.GetTile(gameMove.TileId);
            if (tile == null) throw new Exception("Card can't be null");


            // PutChipOnTile
            if ((gameMove.PlayerName != null) && (gameMove.PartName != null))
            {
                var player = _playersPool.GetPlayer(gameMove.PlayerName);
                if (player == null) throw new NullReferenceException("Player not found: " + gameMove.PlayerName);

                var part = tile.GetPart(gameMove.PartName);
                part.Chip = player.TakeChip();
            }


            tile.RotateCard(gameMove.TileRotation);


            if (!CanPutTileInCell(cell, tile))
                throw new Exception("Card can't be put");

            AddTile(cell, tile);

            _moves.Add(gameMove);
            _tileStack.DiscardTile(tile);

            if (gameMove.PlayerName != null)
                _playersPool.MoveToNextPlayer();
        }

        public List<Tile> GetActiveTiles()
        {
            return _gameGrid.Cells
                .Where(c => c.IsContainingTile())
                .Select(c => c.Tile)
                .ToList();
        }

        public Tile? GetCurrentTile()
        {
            do
            {
                var topTile = _tileStack.GetTopTile();
                if (topTile == null)
                {
                    IsFinished = true;
                    Finished?.Invoke(this, null);
                    return null;
                }

                if (GetAvailableMoves().Any())
                    return topTile;
                else
                    _tileStack.DiscardTile(topTile);
            }
            while (!_tileStack.IsEmpty());

            return null;
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

        private bool RotateTileTilFit(Cell cell, Tile tile)
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
            foreach(var extension in _rules)
                result &= extension.CanPutTileInCell(cell, tile, _gameGrid);

            return result;
        }

        /// <summary>
        /// Add tile and update objects
        /// </summary>
        /// <param name="tile"></param>
        public void AddTile(Cell cell, Tile tile)
        {
            if (!(_gameGrid.Cells.Contains(cell))) throw new Exception("Sell is unknown");

            _gameGrid.PutTile(cell, tile);
            foreach (ObjectPart part in tile.Parts)
                foreach (var manager in _rules.SelectMany(e => e.Managers))
                    manager.ProcessPart(part, cell);
        }

        
    }
}
