using Carcassone.Core.Board;

namespace Carcassone.Core.Tiles
{

    /// <summary>
    ///       F
    ///   |       |
    /// R |_      | F
    ///   |  \    |
    ///       R
    /// </summary>
    public class FFRR : Tile
    {
        protected string roadPartName = "Road_0";
        protected string cornfieldPartName = "Cornfield_0";
        protected string cornfieldPart1Name = "Cornfield_1";

        public FFRR(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var roadPart = new RoadPart(roadPartName, Id);
            Parts.Add(roadPart);

            var cornfieldPart1 = new CornfieldPart(cornfieldPartName, Id);
            Parts.Add(cornfieldPart1);

            var cornfieldPart2 = new CornfieldPart(cornfieldPart1Name, Id);
            Parts.Add(cornfieldPart2);
        }

        public override void ConnectField(Cell field, Grid grid)
        {
            AddBorderToPart(field, CellSide.bottom, GetPart(roadPartName), grid);
            AddBorderToPart(field, CellSide.left, GetPart(roadPartName), grid);

            AddBorderToPart(field, CellSide.top, GetPart(cornfieldPartName), grid);
            AddBorderToPart(field, CellSide.right, GetPart(cornfieldPartName), grid);
            AddCornfieldSplittedBorder(field, CellSide.bottom, CornfieldSide.side_3, GetPart(cornfieldPartName), grid);
            AddCornfieldSplittedBorder(field, CellSide.left, CornfieldSide.side_6, GetPart(cornfieldPartName), grid);

            AddCornfieldSplittedBorder(field, CellSide.bottom, CornfieldSide.side_4, GetPart(cornfieldPart1Name), grid);
            AddCornfieldSplittedBorder(field, CellSide.left, CornfieldSide.side_5, GetPart(cornfieldPart1Name), grid);
        }
    }
}