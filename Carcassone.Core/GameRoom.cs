using Carcassone.Core.Calculation;
using Carcassone.Core.Calculation.Base;
using Carcassone.Core.Calculation.Base.Cities;
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

    /// <summary>
    /// Store all game data.
    /// </summary>
    public class GameRoom
    {
        public MoveState State { get; private set; }
        public GamePlayersPool PlayersPool { get; private set; }

        private List<IGameRules> _rules;
        private TileStack _tileStack;

        private string? _availableMovesCacheTile;
        private List<GameMove>? _availableMovesCache;
        private Tile? _cuerrentTileCache;

        public string Id { get; }

        public GameRoom(IEnumerable<Extension> extentions, List<PlayerInfo> players)
        {
            Id = Guid.NewGuid().ToString();
            _rules = new List<IGameRules>();
            _tileStack = new TileStack();
            PlayersPool = new GamePlayersPool(players);
            State = new MoveState(new List<GameMove>(), new List<BaseGameObject>());

            _rules.Add(new BaseRules());

            if (extentions.Contains(Extension.River))
                _rules.Add(new RiverExtension());

            // compose stack from all tiles of all extensions
            foreach (var extension in _rules)
                extension.AddTiles(_tileStack);

            _tileStack.Shaffle();
        }

        public void ForTests_ResetTilesStack()
        {
            _tileStack.ClearStack();
        }

        public void ForTests_CustomTile(Type type, byte count, int priority)
        {
            _tileStack.CreateTiles(type, count, priority);
        }

        public bool IsFinished()
        {
            return GetCurrentTile() == null;
        }

        public Tile GetTile(string cardId) => _tileStack.GetTile(cardId);

        public int GetRemainTilesCount()
        {
            return _tileStack.GetRemainTiles().Count();
        }

        public List<GameMove> GetAvailableMoves(string tileId)
        {
            if (_availableMovesCacheTile == tileId && _availableMovesCache != null)
                return _availableMovesCache; // optimization 

            var tile = _tileStack.GetTile(tileId);
            if (tile == null)
                throw new Exception($"Tile not found: {tileId}");

            var gameMoves = new List<GameMove>();
            var locations = GetCellsToPutTile(tile);
            foreach (var location in locations)
            {
                for (var rotation = 0; rotation < 4; rotation++)
                {
                    if (CanPutTileInCell(location, rotation, tile))
                    {
                        // without meeple
                        var move0 = new GameMove
                        (
                            playerName: PlayersPool.GetCurrentPlayer().Info.Name,
                            tileId: tile.Id,
                            tileRotation: rotation,
                            tileLocation: location,
                            partName: null
                        );
                        gameMoves.Add(move0);

                        // with meeple
                        var parts = GetAvailableParts(move0);
                        foreach (var part in parts)
                        {
                            var move = new GameMove
                            (
                                playerName: PlayersPool.GetCurrentPlayer().Info.Name,
                                tileId: tile.Id,
                                tileRotation: rotation,
                                tileLocation: location,
                                partName: part.PartName
                            );
                            gameMoves.Add(move);
                        }
                    }
                }
            }

            _availableMovesCacheTile = tileId;
            _availableMovesCache = gameMoves;
            return gameMoves;
        }

        private List<Point> GetCellsToPutTile(Tile? tile)
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
            // part is avilable for placing meeple if the object it connected to is free of other meeples

            var state = CalculateNewState(State, move);
            var partsOfOwnedObjects = state.Calculated
                .Where(o => o.HasOwner(state))
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
            
            var newState = CalculateNewState(State, gameMove);
            State = newState;
            
            // pull new card
            _tileStack.DiscardTopTile();
            _cuerrentTileCache = null;
            _availableMovesCache = null;

            PlayersPool.MoveToNextPlayer();
            _cuerrentTileCache = GetCurrentTile();
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
        public MoveState CalculateNewState(MoveState oldState, GameMove move)
        {
            var tile = _tileStack.GetTile(move.TileId);
            if (tile == null) throw new Exception("Tile can't be null");

            var player = PlayersPool.GetPlayer(move.PlayerName);
            if (player == null) throw new NullReferenceException("Player not found: " + move.PlayerName);


            var newMoves = new List<GameMove>(oldState.GameMoves) { move };
            var objCopies = oldState.Calculated.Select(o => o.Copy()).ToList(); // create copy to not interfere with previous state
            var newState = new MoveState(newMoves, objCopies);

            foreach (var manager in _rules.SelectMany(e => e.Managers))
                manager.CalculateObjects(tile, newState); // calculate new objects
            
            return newState;
        }
    }
}
