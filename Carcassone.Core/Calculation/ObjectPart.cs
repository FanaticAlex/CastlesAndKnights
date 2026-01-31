using Carcassone.Core.Board;
using Carcassone.Core.Players;
using Carcassone.Core.Tiles;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Carcassone.Core.Calculation
{
    /// <summary>
    /// Часть обьекта дорога, замок, поле или церковь.
    /// </summary>
    public abstract class ObjectPart
    {
        public Chip? Chip { get; set; }
        public Flag? Flag { get; set; }

        public string PartId { get; set; }
        public string PartName { get; set; }
        public Tile Tile { get; set; }
        public string PartType { get; set; }

        public List<Side> Sides { get; set; } = new List<Side>();

        public ObjectPart(string partName, Tile tile)
        {
            PartId = tile.Id + partName;
            Tile = tile;
            PartName = partName;
            PartType = string.Empty;
        }

        public List<TileBorder> GetBorders()
        {
            var cell = Tile.Cell;
            return Sides.Select(s => new TileBorder(cell, s)).ToList();
        }
    }
}