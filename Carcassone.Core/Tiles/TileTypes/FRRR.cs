using Carcassone.Core.Board;

namespace Carcassone.Core.Tiles
{

    /// <summary>
    ///       F
    ///   |       |
    /// R |_______| R
    ///   |   |   |
    ///       R
    /// </summary>
    public class FRRR : Tile
    {
        protected string roadPartName = "Road_0";
        protected string roadPart1Name = "Road_1";
        protected string roadPart2Name = "Road_2";
        protected string cornfieldPartName = "Cornfield_0";
        protected string cornfieldPart1Name = "Cornfield_1";
        protected string cornfieldPart2Name = "Cornfield_2";

        public FRRR(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var roadPart1 = new RoadPart(roadPartName, Id);
            Parts.Add(roadPart1);

            var roadPart3 = new RoadPart(roadPart1Name, Id);
            Parts.Add(roadPart3);

            var roadPart4 = new RoadPart(roadPart2Name, Id);
            Parts.Add(roadPart4);

            var cornfieldPart1 = new FieldPart(cornfieldPartName, Id);
            Parts.Add(cornfieldPart1);

            var cornfieldPart2 = new FieldPart(cornfieldPart1Name, Id);
            Parts.Add(cornfieldPart2);

            var cornfieldPart3 = new FieldPart(cornfieldPart2Name, Id);
            Parts.Add(cornfieldPart3);
        }

        public override void ConnectField(Cell field, Grid grid)
        {
            AddBorderToPart(field, CellSide.right, GetPart(roadPartName), grid);

            AddBorderToPart(field, CellSide.bottom, GetPart(roadPart1Name), grid);

            AddBorderToPart(field, CellSide.left, GetPart(roadPart2Name), grid);

            AddCornfieldSplittedBorder(field, CellSide.right, CornfieldSide.side_1, GetPart(cornfieldPartName), grid);
            AddBorderToPart(field, CellSide.top, GetPart(cornfieldPartName), grid);
            AddCornfieldSplittedBorder(field, CellSide.left, CornfieldSide.side_6, GetPart(cornfieldPartName), grid);

            AddCornfieldSplittedBorder(field, CellSide.right, CornfieldSide.side_2, GetPart(cornfieldPart1Name), grid);
            AddCornfieldSplittedBorder(field, CellSide.bottom, CornfieldSide.side_3, GetPart(cornfieldPart1Name), grid);

            AddCornfieldSplittedBorder(field, CellSide.bottom, CornfieldSide.side_4, GetPart(cornfieldPart2Name), grid);
            AddCornfieldSplittedBorder(field, CellSide.left, CornfieldSide.side_5, GetPart(cornfieldPart2Name), grid);
        }
    }
}