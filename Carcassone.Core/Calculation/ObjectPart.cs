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
        public Meeple? Meeple { get; set; }
        public Flag? Flag { get; set; }

        public string PartId => TileId + PartName;
        public string TileId {  get; set; }
        public string PartName { get; set; }
        public string PartType { get; set; }
        public Point Location { get; set; }

        public List<Side> Sides { get; set; } = new List<Side>();

        public ObjectPart(string partName, Tile tile)
        {
            TileId = tile.Id;
            PartName = partName;
            PartType = string.Empty;
        }

        public List<TileBorder> GetBorders()
        {
            return Sides.Select(s => new TileBorder(Location, s)).ToList();
        }
    }
}