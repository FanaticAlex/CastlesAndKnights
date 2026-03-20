using Carcassone.Core.Tiles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Carcassone.Core
{
    /// <summary>
    /// Represents the game board where players put tiles
    /// </summary>
    public class Grid
    {
        public Dictionary<Point, Tile> _tilesInGame; // optimization for search

        public Grid()
        {
            _tilesInGame = new Dictionary<Point, Tile>();
        }

        public void PutTile(Point location, Tile tile)
        {
            tile.Location = location;
            _tilesInGame.Add(location, tile);
        }

        public Tile? GetNeighbour(Point location, Side side)
        {
            return side switch
            {
                Side.top => GetTile(new Point(location.X, location.Y + 1)),
                Side.bottom => GetTile(new Point(location.X, location.Y - 1)),
                Side.right => GetTile(new Point(location.X + 1, location.Y)),
                Side.left => GetTile(new Point(location.X - 1, location.Y)),
                _ => throw new KeyNotFoundException(),
            };
        }

        public IEnumerable<Point> GetEmptyCells()
        {
            var tilePoints = _tilesInGame.Keys;

            var allPoints = new List<Point> { new Point(0, 0) }; // add initial location 
            foreach (var location in tilePoints)
            {
                allPoints.Add(location + new Size(0, 1));
                allPoints.Add(location + new Size(0, -1));
                allPoints.Add(location + new Size(1, 0));
                allPoints.Add(location + new Size(-1, 0));
            }

            return allPoints.Except(tilePoints).Distinct();
        }

        public Tile? GetTile(Point location)
        {
            if (_tilesInGame.ContainsKey(location))
                return _tilesInGame[location];

            return null;
        }
    }
}
