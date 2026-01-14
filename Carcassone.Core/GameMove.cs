using Carcassone.Core.Calculation;

namespace Carcassone.Core
{
    public class GameMove
    {
        public string? PlayerName { get; set; }
        public string TileId { get; set; }
        public int TileRotation { get; set; }
        public string CellId { get; set; }
        public string? PartName { get; set; }
    }
}
