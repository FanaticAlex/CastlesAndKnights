using Carcassone.Core.Calculation;
using Carcassone.Core.Calculation.Base;
using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.Base.Monasteries;
using Carcassone.Core.Calculation.Base.Roads;
using Carcassone.Core.Calculation.RiverExtension;
using Carcassone.Core.Players;
using Carcassone.Core.Tiles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;

namespace Carcassone.Core
{
    public enum Extension
    { 
        River
    }

    public class Locations
    {
        public List<Point> Awailable { get; set; } = new List<Point>();
        public List<Point> UnAwailable { get; set; } = new List<Point>();
    }


    public class GameState
    {
        public  GameMove TrialMove { get; set; }
        private List<MoveState> _movesStates { get; } = new List<MoveState>();

        public void Add(MoveState state)
        {
            _movesStates.Add(state);
        }

        public void Remove(MoveState state)
        {
            _movesStates.Remove(state);
        }

        public Point GetTileLocation(string tileId)
        {
            if (TrialMove?.TileId == tileId) return TrialMove.Location;
            return _movesStates.First(s => s.GameMove.TileId == tileId).GameMove.Location;
        }

        public int GetTileRotation(string tileId)
        {
            if (TrialMove?.TileId == tileId) return TrialMove.TileRotation;
            return _movesStates.First(s => s.GameMove.TileId == tileId).GameMove.TileRotation;
        }

        public MoveState GetLastState()
        {
            if (IsFirstMove()) return new MoveState(null, new List<BaseGameObject>());

            return _movesStates.Last();
        }

        public IEnumerable<GameMove> GetMoves()
        {
            return _movesStates.Select(s => s.GameMove);
        }

        public bool IsFirstMove()
        {
            return !_movesStates.Any();
        }

        public GameMove? GetNeighbour(Point location, Side side)
        {
            return side switch
            {
                Side.top => GetMove(new Point(location.X, location.Y + 1)),
                Side.bottom => GetMove(new Point(location.X, location.Y - 1)),
                Side.right => GetMove(new Point(location.X + 1, location.Y)),
                Side.left => GetMove(new Point(location.X - 1, location.Y)),
                _ => throw new KeyNotFoundException(),
            };
        }

        public IEnumerable<Point> GetEmptyCells()
        {
            var tilesLocations = GetMoves().Select(m => m.Location);

            var allPoints = new List<Point> { new Point(0, 0) }; // add initial location 
            foreach (var location in tilesLocations)
            {
                allPoints.Add(location + new Size(0, 1));
                allPoints.Add(location + new Size(0, -1));
                allPoints.Add(location + new Size(1, 0));
                allPoints.Add(location + new Size(-1, 0));
            }

            return allPoints.Except(tilesLocations).Distinct();
        }

        public GameMove? GetMove(Point location)
        {
            return _movesStates.FirstOrDefault(m => m.GameMove.Location == location)?.GameMove;
        }
    }

    public class MoveState
    {
        public MoveState(GameMove move, List<BaseGameObject> gameObjects)
        {
            GameMove = move;
            Calculated = gameObjects;
        }

        public GameMove GameMove { get; set; }

        public List<BaseGameObject> Calculated { get; set; }

        public IEnumerable<City> GetAllCities()
        {
            return Calculated.OfType<City>();
        }
        public IEnumerable<Road> GetAllRoads()
        {
            return Calculated.OfType<Road>();
        }

        public IEnumerable<Farm> GetAllFarms()
        {
            return Calculated.OfType<Farm>();
        }

        public IEnumerable<Monastery> GetAllMonastery()
        {
            return Calculated.OfType<Monastery>();
        }
    }

    /// <summary>
    /// Store all game data.
    /// </summary>
    public class GameRoom
    {
        public GameState State { get; }
        private List<IGameRules> _rules;
        private TileStack _tileStack;
        private GamePlayersPool _playersPool;

        private string _availableMovesCacheTile;
        private List<GameMove> _availableMovesCache;
        private Tile _cuerrentTileCache;

        public string Id { get; }

        public GameRoom(IEnumerable<Extension> extentions, List<PlayerInfo> players)
        {
            Id = Guid.NewGuid().ToString();
            State = new GameState();
            _rules = new List<IGameRules>();
            _tileStack = new TileStack();
            _playersPool = new GamePlayersPool(players);

            _rules.Add(new BaseRules());

            if (extentions.Contains(Extension.River))
                _rules.Add(new RiverExtension());

            // compose stack from all tiles of all extensions
            foreach (var extension in _rules)
                extension.AddTiles(_tileStack);

            _tileStack.Shaffle();
        }

        public MoveState GetLastState()
        {
            return State.GetLastState();
        }

        public void ForTests_ResetTilesStack()
        {
            _tileStack.ClearStack();
        }

        public void ForTests_CustomTile(Type type, byte count, int priority)
        {
            _tileStack.CreateTiles(type, count, priority);
        }

        public List<GamePlayer> GetPlayers()
        {
            return _playersPool.GamePlayers;
        }

        public GamePlayer GetCurrentPlayer()
        {
            return _playersPool.GetCurrentPlayer();
        }

        public bool IsFinished()
        {
            return GetCurrentTile() == null;
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
                var playerObjects = State.GetLastState().Calculated
                                    .Where(o => o.IsPlayerOwner(player.Info.Name));

                var score = new PlayerScore()
                {
                    PlayerName = player.Info.Name,
                    OverallScore = playerObjects.Select(o => o.GetScore(State)).Sum(),
                    MeeplesCount = GetMeeplesCount(player),
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
            return _tileStack.GetRemainTiles().Count();
        }

        public List<GameMove> GetAvailableMoves(string tileId)
        {
            if (_availableMovesCacheTile == tileId) return _availableMovesCache; // optimization 

            var gameMoves = new List<GameMove>();
            var tile = _tileStack.GetTile(tileId);
            var locations = GetCellsToPutTile(tile);
            foreach (var location in locations)
            {
                for (var rotation = 0; rotation < 4; rotation++)
                {
                    if (CanPutTileInCell(location, rotation, tile))
                    {
                        // without meeple
                        var move0 = new GameMove();
                        move0.Location = location;
                        move0.TileId = tile.Id;
                        move0.TileRotation = rotation;
                        move0.PartName = null;
                        move0.PlayerName = _playersPool.GetCurrentPlayer().Info.Name;
                        gameMoves.Add(move0);

                        // with meeple
                        var parts = GetAvailableParts(move0);
                        foreach (var part in parts)
                        {
                            var move = new GameMove();
                            move.Location = location;
                            move.TileId = tile.Id;
                            move.TileRotation = rotation;
                            move.PartName = part.PartName;
                            move.PlayerName = _playersPool.GetCurrentPlayer().Info.Name;
                            gameMoves.Add(move);
                        }
                    }
                }
            }

            _availableMovesCacheTile = tileId;
            _availableMovesCache = gameMoves;
            return gameMoves;
        }

        private List<Point> GetCellsToPutTile(Tile tile)
        {
            var list = new List<Point>();
            if (tile == null)
                return list;

            var locations = GetCellsStatus().Awailable;
            foreach (var cell in locations)
            {
                if (CanPutTileInCellWithRotation(cell, tile))
                    list.Add(cell);
            }

            return list;
        }

        private List<ObjectPart> GetAvailableParts(GameMove move)
        {
            var state = CalculateNewMove(move);

            var partsOfOwnedObjects = state.Calculated
                .Where(o => HasOwnerHelper.HasOwner(o))
                .SelectMany(o => o.Parts)
                .Select(p => p.PartId);

            var list = new List<ObjectPart>();
            var tile = _tileStack.GetTile(move.TileId);
            foreach (var part in tile.Parts)
            {
                if (!(partsOfOwnedObjects.Contains(part.PartId)))
                    list.Add(part);
            }

            return list;
        }

        public Locations GetCellsStatus()
        {
            var status = new Locations();
            var emptyCells = State.GetEmptyCells();
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
                    status.UnAwailable.Add(cell);
                else
                    status.Awailable.Add(cell);
            }

            return status;
        }

        public void MakeMove(GameMove gameMove)
        {
            if (gameMove == null) throw new ArgumentNullException("Move obj can not be null");

            var availableMoves = GetAvailableMoves(gameMove.TileId);
            if (!availableMoves.Contains(gameMove, new MoveComparer()))
                throw new Exception("Card can't be put");
            
            var state = CalculateNewMove(gameMove);
            State.Add(state);
            
            // pull new card
            _tileStack.DiscardTopTile();
            _cuerrentTileCache = null;
            _availableMovesCache = null;

            _playersPool.MoveToNextPlayer();
            _cuerrentTileCache = GetCurrentTile();
        }

        public void UndoLastMove()
        {
            var lastMove = State.GetLastState().GameMove;
            var tile = _tileStack.GetTile(lastMove.TileId);
            foreach (var part in tile.Parts)
                part.Meeple = null;

            State.Remove(State.GetLastState());

            _tileStack.ReturnToTile(lastMove.TileId);
            _cuerrentTileCache = null;
            _availableMovesCache = null;

            _playersPool.MoveToPreviousPlayer();
        }

        public Tile? GetCurrentTile()
        {
            if (_cuerrentTileCache != null) return _cuerrentTileCache;

            var stopSearching = false;
            while (!stopSearching)
            {
                var topTile = _tileStack.GetTopTile();
                if (topTile == null) stopSearching = true;

                if (GetCellsToPutTile(topTile).Any())
                    stopSearching = true;
                else
                    _tileStack.DiscardTopTile();
            }

            _cuerrentTileCache = _tileStack.GetTopTile();
            if (_cuerrentTileCache == null) return null;

            _availableMovesCache = GetAvailableMoves(_cuerrentTileCache.Id);
            return _cuerrentTileCache;
        }

        public int GetMeeplesCount(GamePlayer player)
        {
            var objects = State.GetLastState().Calculated;

            var usedMeeples = 0;
            foreach (var obj in objects)
            {
                var completableObj = obj as ICompletableGameObject;
                if (completableObj != null)
                {
                    if (completableObj.IsComplete(State))
                        continue;
                }

                foreach (var part in obj.Parts)
                {
                    if (part.Meeple?.Owner == player)
                        usedMeeples++;
                }
            }

            return GamePlayer.MeeplesMaxCount - usedMeeples; // TODO Create test to check correct add remove and limit meeples
        }

        private bool CanPutTileInCellWithRotation(Point location, Tile tile)
        {
            if (tile == null) return false;

            var isFit = false;
            for (int rotations = 0; rotations < 4; rotations++) // можно сделать до 4х поворотов 4й - исходное положение (в конце)
            {
                if (CanPutTileInCell(location, rotations, tile))
                    isFit = true;
            }

            return isFit;
        }

        private bool CanPutTileInCell(Point location, int rotation, Tile tile)
        {
            if (State.IsFirstMove()) return true;

            var result = true;
            foreach(var extension in _rules)
                result &= extension.CanPutTileInCell(location, rotation, tile, State);

            return result;
        }

        /// <summary>
        /// Add tile and update objects
        /// </summary>
        /// <param name="tile"></param>
        private MoveState CalculateNewMove(GameMove move)
        {
            var tile = _tileStack.GetTile(move.TileId);
            if (tile == null) throw new Exception("Tile can't be null");

            // PutMeepleOnTile
            var player = _playersPool.GetPlayer(move.PlayerName);
            if (player == null) throw new NullReferenceException("Player not found: " + move.PlayerName);

            if (!string.IsNullOrEmpty(move.PartName))
            {
                var part = tile.GetPart(move.PartName);
                part.Meeple = player.TakeMeeple();
            }

            var allObj = new List<BaseGameObject>();
            State.TrialMove = move; // for correct calculation of rotation and location of curent card
            
            foreach (var manager in _rules.SelectMany(e => e.Managers))
                allObj.AddRange(manager.CalculateObjects(tile, State));

            State.TrialMove = null;

            return new MoveState(move, allObj);
        }
    }
}
