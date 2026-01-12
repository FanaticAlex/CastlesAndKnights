using Carcassone.Core.Board;
using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Tiles;
using Newtonsoft.Json;

namespace Carcassone.Core.Calculation
{
    public class TileBorder
    {
        public string FirstCellId { get; set; }

        public string SecondCellId { get; set; }

        public FieldSide? CornfieldSide { get; set; }

        public string TileId { get; set; }

        [JsonConstructor]
        public TileBorder() { }

        public TileBorder(Cell? first, Cell? second, Tile tile)
        {
            if (first == null || second == null)
                throw new System.Exception($"Cell can not be null. Tile: {tile.Id}, first: {first?.Id}, second: {second?.Id}");

            FirstCellId = first.Id;
            SecondCellId = second.Id;
            TileId = tile.Id;
        }

        public static bool Equial(TileBorder border1, TileBorder border2)
        {
            if (border1.FirstCellId == null || border1.SecondCellId == null)
                return false;

            if (border2.FirstCellId == null || border2.SecondCellId == null)
                return false;

            if (border1.FirstCellId == border2.FirstCellId &&
                border1.SecondCellId == border2.SecondCellId)
            {
                return true;
            }

            if (border1.FirstCellId == border2.SecondCellId &&
                border1.SecondCellId == border2.FirstCellId)
            {
                return true;
            }

            return false;
        }
    }
}