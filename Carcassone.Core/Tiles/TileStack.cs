using Carcassone.Core.Calculation;
using Carcassone.Core.Calculation.Base.Tiles;
using Newtonsoft.Json;
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
        private List<Tile> _remain = new List<Tile>();
        private readonly List<Tile> _discardedTiles = new List<Tile>();

        public void AddTiles(List<Tile> tiles)
        {
            _remain.AddRange(tiles);
        }

        public List<Tile> GetRemainTiles()
        {
            return _remain;
        }

        public List<Tile> GetAllTiles()
        {
            var all = new List<Tile>();
            all.AddRange(GetRemainTiles());
            all.AddRange(_discardedTiles);
            return all;
        }

        public Tile GetTile(string tileId)
        {
            var tile = GetAllTiles().FirstOrDefault(t => t.Id == tileId);
            if (tile == null)
                throw new Exception($"Tile {tileId} not found");

            return tile;
        }

        public bool IsEmpty()
        {
            return _remain.Count == 0;
        }

        public Tile? GetTopTile()
        {
            return _remain.FirstOrDefault();
        }

        public void DiscardTile(Tile? card)
        {
            if (card == null) return;

            _remain.Remove(card);
            _discardedTiles.Add(card);
        }

        public void Shaffle()
        {
            // перетосовка
            var rnd = new System.Random();
            int n = _remain.Count;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                var value = _remain[k];
                _remain[k] = _remain[n];
                _remain[n] = value;
            }

            _remain = _remain.OrderBy(tile => tile.StackPriority).ToList();
        }

        public void CreateTiles(Type tileType, byte count, int priority)
        {
            var tiles = new List<Tile>();
            for (int i = 0; i < count; i++)
            {
                var tileTypeStr = tileType.Name.Replace(tileType.Namespace ?? string.Empty, string.Empty);
                var tile = (Tile?)Activator.CreateInstance(tileType, tileTypeStr, i);
                if (tile == null)
                    throw new Exception($"Can't create card of type {tileTypeStr}: {i}");

                tile.StackPriority = priority;
                tiles.Add(tile);
            }

            _remain.AddRange(tiles);
        }
    }
}
