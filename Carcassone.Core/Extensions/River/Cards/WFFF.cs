using Carcassone.Core.Tiles;
using Carcassone.Core.Board;

namespace Carcassone.Core.Extensions.River.Cards
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

        public override void ConnectField(Cell field, Grid grid)
        {
            AddCornfieldSplittedBorder(field, CellSide.top, CornfieldSide.side_0, GetPart(cornfieldPartName), grid);
            AddCornfieldSplittedBorder(field, CellSide.top, CornfieldSide.side_7, GetPart(cornfieldPartName), grid);
            AddBorderToPart(field, CellSide.right, GetPart(cornfieldPartName), grid);
            AddBorderToPart(field, CellSide.bottom, GetPart(cornfieldPartName), grid);
            AddBorderToPart(field, CellSide.left, GetPart(cornfieldPartName), grid);
        }
    }
}