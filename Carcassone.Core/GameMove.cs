using Carcassone.Core.Calculation;
using System.Drawing;

namespace Carcassone.Core
{
    public class GameMove
    {
        public string? PlayerName { get; set; }
        public string TileId { get; set; }
        public int TileRotation { get; set; }
        public Point Location { get; set; }
        public string? PartName { get; set; }
    }
}
