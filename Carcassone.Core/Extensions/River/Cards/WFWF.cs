using Carcassone.Core.Tiles;
using Carcassone.Core.Board;

namespace Carcassone.Core.Extensions.River.Cards
{
    /// <summary>
    ///       W
    ///   |   +   |
    /// F |   +   | F
    ///   |   +   |
    ///       W
    /// </summary>
    public class WFWF : Tile
    {
        protected string cornfieldPart0Name = "Cornfield_0";
        protected string cornfieldPart1Name = "Cornfield_1";

        public WFWF(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var cornfieldPart0 = new FieldPart(cornfieldPart0Name, Id);
            Parts.Add(cornfieldPart0);

            var cornfieldPart1 = new FieldPart(cornfieldPart1Name, Id);
            Parts.Add(cornfieldPart1);
        }

        public override void ConnectField(Cell field, Grid grid)
        {
            AddCornfieldSplittedBorder(field, CellSide.top, CornfieldSide.side_0, GetPart(cornfieldPart0Name), grid);
            AddBorderToPart(field, CellSide.right, GetPart(cornfieldPart0Name), grid);
            AddCornfieldSplittedBorder(field, CellSide.bottom, CornfieldSide.side_3, GetPart(cornfieldPart0Name), grid);

            AddCornfieldSplittedBorder(field, CellSide.bottom, CornfieldSide.side_4, GetPart(cornfieldPart1Name), grid);
            AddBorderToPart(field, CellSide.left, GetPart(cornfieldPart1Name), grid);
            AddCornfieldSplittedBorder(field, CellSide.top, CornfieldSide.side_7, GetPart(cornfieldPart1Name), grid);
        }
    }
}