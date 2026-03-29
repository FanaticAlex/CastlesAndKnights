using System;
using System.Collections.Generic;
using System.Linq;

namespace Carcassone.Core.Tiles
{
    /// <summary>
    /// Stack of tiles in a current game
    /// </summary>
    public class TileStack
    {
        private List<Tile> _tiles = new List<Tile>();
        private int _currentTileIndex;

        public IEnumerable<Tile> GetRemainTiles()
        {
            return _tiles.Skip(_currentTileIndex);
        }

        public Tile GetTile(string tileId)
        {
            var tile = _tiles.FirstOrDefault(t => t.Id == tileId);
            if (tile == null)
                throw new Exception($"Tile {tileId} not found");

            return tile;
        }

        public Tile? GetTopTile()
        {
            return GetRemainTiles().FirstOrDefault();
        }

        public void DiscardTopTile()
        {
            if (GetTopTile() == null) return;

            _currentTileIndex++;
        }

        public void ReturnToTile(string? tileId)
        {
            if (tileId == null)
            {
                _currentTileIndex = 0;
                return;
            }

            var tile = GetTile(tileId);
            _currentTileIndex = _tiles.IndexOf(tile);
        }

        public void Shaffle()
        {
            // перетосовка
            var rnd = new System.Random();
            int n = _tiles.Count;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                var value = _tiles[k];
                _tiles[k] = _tiles[n];
                _tiles[n] = value;
            }

            _tiles = _tiles.OrderBy(tile => tile.StackPriority).ToList();
        }

        public void ClearStack()
        {
            _tiles.Clear();
        }

        public void CreateTiles(Type tileType, byte count, int priority)
        {
            var tiles = new List<Tile>();
            for (int tileNumber = 0; tileNumber < count; tileNumber++)
            {
                var tileTypeStr = tileType.Name.Replace(tileType.Namespace ?? string.Empty, string.Empty);
                var tile = (Tile?)Activator.CreateInstance(tileType, tileTypeStr, tileNumber);
                if (tile == null)
                    throw new Exception($"Can't create card of type {tileTypeStr}: {tileNumber}");

                tile.StackPriority = priority;
                tiles.Add(tile);
            }

            _tiles.AddRange(tiles);
        }
    }
}
