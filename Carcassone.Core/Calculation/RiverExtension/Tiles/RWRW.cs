using Carcassone.Core.Tiles;
using Carcassone.Core.Board;
using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.Base.Roads;

namespace Carcassone.Core.Calculation.River.Tiles
{
    /// <summary>
    ///       R
    ///   |   |   |
    /// W |+++++++| W
    ///   |   |   |
    ///       R
    /// </summary>
    public class RWRW : Tile
    {
        protected string cornfieldPart0Name = "Cornfield_0";
        protected string cornfieldPart1Name = "Cornfield_1";
        protected string roadPartName = "Road_0";
        protected string cornfieldPart2Name = "Cornfield_2";
        protected string cornfieldPart3Name = "Cornfield_3";

        public RWRW(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var cornfieldPart0 = new FieldPart(cornfieldPart0Name, Id);
            Parts.Add(cornfieldPart0);

            var cornfieldPart1 = new FieldPart(cornfieldPart1Name, Id);
            Parts.Add(cornfieldPart1);

            var cornfieldPart2 = new FieldPart(cornfieldPart2Name, Id);
            Parts.Add(cornfieldPart2);

            var cornfieldPart3 = new FieldPart(cornfieldPart3Name, Id);
            Parts.Add(cornfieldPart3);

            var roadPart = new RoadPart(roadPartName, Id);
            Parts.Add(roadPart);
        }
        
        public override void ConnectField(Cell field, Grid grid)
        {
            AddCornfieldSplittedBorder(field, CellSide.top, FieldSide.side_0, GetPart(cornfieldPart0Name), grid);
            AddCornfieldSplittedBorder(field, CellSide.right, FieldSide.side_1, GetPart(cornfieldPart0Name), grid);

            AddCornfieldSplittedBorder(field, CellSide.right, FieldSide.side_2, GetPart(cornfieldPart1Name), grid);
            AddCornfieldSplittedBorder(field, CellSide.bottom, FieldSide.side_3, GetPart(cornfieldPart1Name), grid);

            AddCornfieldSplittedBorder(field, CellSide.bottom, FieldSide.side_4, GetPart(cornfieldPart2Name), grid);
            AddCornfieldSplittedBorder(field, CellSide.left, FieldSide.side_5, GetPart(cornfieldPart2Name), grid);

            AddCornfieldSplittedBorder(field, CellSide.left, FieldSide.side_6, GetPart(cornfieldPart3Name), grid);
            AddCornfieldSplittedBorder(field, CellSide.top, FieldSide.side_7, GetPart(cornfieldPart3Name), grid);

            // дорога
            AddBorderToPart(field, CellSide.top, GetPart(roadPartName), grid);
            AddBorderToPart(field, CellSide.bottom, GetPart(roadPartName), grid);
        }
    }
}