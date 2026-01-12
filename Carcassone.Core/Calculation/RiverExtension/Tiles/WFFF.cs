using Carcassone.Core.Tiles;
using Carcassone.Core.Board;
using Carcassone.Core.Calculation.Base.Farms;

namespace Carcassone.Core.Calculation.River.Tiles
{
    /// <summary>
    ///       W
    ///   |   +   |
    /// F |   +   | F
    ///   |       |
    ///       F
    /// </summary>
    public class WFFF : Tile
    {
        protected string cornfieldPartName = "Cornfield_0";

        public WFFF(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var cornfieldPart1 = new FieldPart(cornfieldPartName, Id);
            Parts.Add(cornfieldPart1);
        }

        public override void ConnectField(Cell cell, Grid grid)
        {
            AddCornfieldSplittedBorder(cell, CellSide.top, FieldSide.side_0, GetPart(cornfieldPartName), grid);
            AddCornfieldSplittedBorder(cell, CellSide.top, FieldSide.side_7, GetPart(cornfieldPartName), grid);
            AddBorderToPart(cell, CellSide.right, GetPart(cornfieldPartName), grid);
            AddBorderToPart(cell, CellSide.bottom, GetPart(cornfieldPartName), grid);
            AddBorderToPart(cell, CellSide.left, GetPart(cornfieldPartName), grid);
        }
    }
}