using Carcassone.Core.Tiles;
using Carcassone.Core.Board;
using Carcassone.Core.Calculation.Base.Farms;

namespace Carcassone.Core.Calculation.River.Tiles
{
    /// <summary>
    ///       F
    ///   |       |
    /// F |   ++++| W
    ///   |   +   |
    ///       W
    /// </summary>
    public class FWWF : Tile
    {
        protected string cornfieldPart0Name = "Cornfield_0";
        protected string cornfieldPart1Name = "Cornfield_1";

        public FWWF(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var cornfieldPart1 = new FieldPart(cornfieldPart0Name, Id);
            Parts.Add(cornfieldPart1);

            var cornfieldPart2 = new FieldPart(cornfieldPart1Name, Id);
            Parts.Add(cornfieldPart2);
        }

        public override void ConnectField(Cell field, Grid grid)
        {
            AddBorderToPart(field, CellSide.top, GetPart(cornfieldPart0Name), grid);
            AddCornfieldSplittedBorder(field, CellSide.right, FieldSide.side_1, GetPart(cornfieldPart0Name), grid);
            AddCornfieldSplittedBorder(field, CellSide.bottom, FieldSide.side_4, GetPart(cornfieldPart0Name), grid);
            AddBorderToPart(field, CellSide.left, GetPart(cornfieldPart0Name), grid);

            AddCornfieldSplittedBorder(field, CellSide.right, FieldSide.side_2, GetPart(cornfieldPart1Name), grid);
            AddCornfieldSplittedBorder(field, CellSide.bottom, FieldSide.side_3, GetPart(cornfieldPart1Name), grid);
        }
    }
}