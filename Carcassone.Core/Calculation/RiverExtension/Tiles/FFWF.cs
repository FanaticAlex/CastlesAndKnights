using Carcassone.Core.Tiles;
using Carcassone.Core.Board;
using Carcassone.Core.Calculation.Base.Farms;

namespace Carcassone.Core.Calculation.River.Tiles
{

    /// <summary>
    ///       F
    ///   |       |
    /// F |   +   | F
    ///   |   +   |
    ///       W
    /// </summary>
    public class FFWF : Tile
    {
        protected string cornfieldPartName = "Cornfield_0";

        public FFWF(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var cornfieldPart = new FieldPart(cornfieldPartName, Id);
            Parts.Add(cornfieldPart);
        }

        public override void ConnectField(Cell field, Grid grid)
        {
            AddBorderToPart(field, CellSide.left, GetPart(cornfieldPartName), grid);
            AddBorderToPart(field, CellSide.top, GetPart(cornfieldPartName), grid);
            AddBorderToPart(field, CellSide.right, GetPart(cornfieldPartName), grid);
            AddCornfieldSplittedBorder(field, CellSide.bottom, FieldSide.side_3, GetPart(cornfieldPartName), grid);
            AddCornfieldSplittedBorder(field, CellSide.bottom, FieldSide.side_4, GetPart(cornfieldPartName), grid);
        }
    }
}