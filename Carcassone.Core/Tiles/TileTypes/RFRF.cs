using Carcassone.Core.Board;

namespace Carcassone.Core.Tiles
{

    /// <summary>
    ///       R
    ///   |   |   |
    /// F |   |   | F
    ///   |   |   |
    ///       R
    /// </summary>
    public class RFRF : Tile
    {
        protected string roadPartName = "Road_0";
        protected string cornfieldPartName = "Cornfield_0";
        protected string cornfieldPart1Name = "Cornfield_1";

        public RFRF(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var roadPart1 = new RoadPart(roadPartName, Id);
            Parts.Add(roadPart1);

            var cornfieldPart1 = new FieldPart(cornfieldPartName, Id);
            Parts.Add(cornfieldPart1);

            var cornfieldPart2 = new FieldPart(cornfieldPart1Name, Id);
            Parts.Add(cornfieldPart2);
        }

        public override void ConnectField(Cell field, Grid grid)
        {
            AddBorderToPart(field, CellSide.top, GetPart(roadPartName), grid);
            AddBorderToPart(field, CellSide.bottom, GetPart(roadPartName), grid);

            AddCornfieldSplittedBorder(field, CellSide.top, CornfieldSide.side_0, GetPart(cornfieldPartName), grid);
            AddBorderToPart(field, CellSide.right, GetPart(cornfieldPartName), grid);
            AddCornfieldSplittedBorder(field, CellSide.bottom, CornfieldSide.side_3, GetPart(cornfieldPartName), grid);

            AddCornfieldSplittedBorder(field, CellSide.bottom, CornfieldSide.side_4, GetPart(cornfieldPart1Name), grid);
            AddBorderToPart(field, CellSide.left, GetPart(cornfieldPart1Name), grid);
            AddCornfieldSplittedBorder(field, CellSide.top, CornfieldSide.side_7, GetPart(cornfieldPart1Name), grid);
        }
    }
}