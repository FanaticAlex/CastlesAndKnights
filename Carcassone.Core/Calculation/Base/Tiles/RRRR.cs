using Carcassone.Core.Board;
using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.Base.Roads;
using Carcassone.Core.Tiles;

namespace Carcassone.Core.Calculation.Base.Tiles
{

    /// <summary>
    ///       R
    ///   |   |   |
    /// R |---|---| R
    ///   |   |   |
    ///       R
    /// </summary>
    public class RRRR : Tile
    {
        protected string roadPartName = "Road_0";
        protected string roadPart1Name = "Road_1";
        protected string roadPart2Name = "Road_2";
        protected string roadPart3Name = "Road_3";
        protected string cornfieldPartName = "Cornfield_0";
        protected string cornfieldPart1Name = "Cornfield_1";
        protected string cornfieldPart2Name = "Cornfield_2";
        protected string cornfieldPart3Name = "Cornfield_3";

        public RRRR(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var roadPart1 = new RoadPart(roadPartName, Id);
            Parts.Add(roadPart1);

            var roadPart2 = new RoadPart(roadPart1Name, Id);
            Parts.Add(roadPart2);

            var roadPart3 = new RoadPart(roadPart2Name, Id);
            Parts.Add(roadPart3);

            var roadPart4 = new RoadPart(roadPart3Name, Id);
            Parts.Add(roadPart4);

            var cornfieldPart1 = new FieldPart(cornfieldPartName, Id);
            Parts.Add(cornfieldPart1);

            var cornfieldPart2 = new FieldPart(cornfieldPart1Name, Id);
            Parts.Add(cornfieldPart2);

            var cornfieldPart3 = new FieldPart(cornfieldPart2Name, Id);
            Parts.Add(cornfieldPart3);

            var cornfieldPart4 = new FieldPart(cornfieldPart3Name, Id);
            Parts.Add(cornfieldPart4);
        }

        public override void ConnectField(Cell field, Grid grid)
        {
            AddBorderToPart(field, CellSide.top, GetPart(roadPartName), grid);

            AddBorderToPart(field, CellSide.right, GetPart(roadPart1Name), grid);

            AddBorderToPart(field, CellSide.bottom, GetPart(roadPart2Name), grid);

            AddBorderToPart(field, CellSide.left, GetPart(roadPart3Name), grid);

            AddCornfieldSplittedBorder(field, CellSide.top, FieldSide.side_0, GetPart(cornfieldPartName), grid);
            AddCornfieldSplittedBorder(field, CellSide.right, FieldSide.side_1, GetPart(cornfieldPartName), grid);

            AddCornfieldSplittedBorder(field, CellSide.right, FieldSide.side_2, GetPart(cornfieldPart1Name), grid);
            AddCornfieldSplittedBorder(field, CellSide.bottom, FieldSide.side_3, GetPart(cornfieldPart1Name), grid);

            AddCornfieldSplittedBorder(field, CellSide.bottom, FieldSide.side_4, GetPart(cornfieldPart2Name), grid);
            AddCornfieldSplittedBorder(field, CellSide.left, FieldSide.side_5, GetPart(cornfieldPart2Name), grid);

            AddCornfieldSplittedBorder(field, CellSide.left, FieldSide.side_6, GetPart(cornfieldPart3Name), grid);
            AddCornfieldSplittedBorder(field, CellSide.top, FieldSide.side_7, GetPart(cornfieldPart3Name), grid);
        }
    }
}