using Carcassone.Core.Calculation;
using Carcassone.Core.Calculation.Base.Cities;
using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.Base.Monasteries;
using Carcassone.Core.Calculation.Base.Roads;
using Carcassone.Core.Tiles;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;

namespace Carcassone.Core
{
    /// <summary>
    /// The state of a game. Tiles locations, roatations, meeples etc.
    /// </summary>
    public class MoveState
    {
        public MoveState(List<GameMove> moves, List<BaseGameObject> gameObjects)
        {
            GameMoves = moves;
            Calculated = gameObjects;
        }

        public List<GameMove> GameMoves { get; }

        public List<BaseGameObject> Calculated { get; set; }

        public bool IsFirstMove()
        {
            return GameMoves.Count == 0;
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
            var tilesLocations = GameMoves.Select(m => m.Location);

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
            return GameMoves.FirstOrDefault(m => m.Location == location);
        }

        public Point GetTileLocation(string tileId)
        {
            return GameMoves.First(s => s.TileId == tileId).Location;
        }

        public int GetTileRotation(string tileId)
        {
            return GameMoves.First(s => s.TileId == tileId).TileRotation;
        }
    }
}
