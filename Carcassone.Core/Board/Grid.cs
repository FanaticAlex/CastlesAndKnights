using Carcassone.Core.Tiles;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Carcassone.Core.Board
{
    /// <summary>
    /// Represents the game board where players put tiles
    /// </summary>
    public class Grid
    {
        public List<Tile> _tilesInGame;

        public Grid()
        {
            _tilesInGame = new List<Tile>();
        }

        public void PutTile(Point location, Tile tile)
        {
            tile.Location = location;
            _tilesInGame.Add(tile);
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
            var allPoints = new List<Point>
            {
                new Point(0, 0) // add initial location
            };

            var tilePoints = _tilesInGame.Select(t => t.Location);
            foreach (var tile in _tilesInGame)
            {
                allPoints.Add(tile.Location + new Size(0, 1));
                allPoints.Add(tile.Location + new Size(0, -1));
                allPoints.Add(tile.Location + new Size(1, 0));
                allPoints.Add(tile.Location + new Size(-1, 0));
            }

            return allPoints.Except(tilePoints).Distinct();
        }

        public Tile GetTile(Point location)
        {
            return _tilesInGame.FirstOrDefault(t => t.Location == location);
        }
    }
}
