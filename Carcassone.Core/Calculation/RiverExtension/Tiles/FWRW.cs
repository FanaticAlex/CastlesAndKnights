using Carcassone.Core.Tiles;
using Carcassone.Core.Board;
using Carcassone.Core.Calculation.Base.Roads;
using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.Base.Monasteries;

namespace Carcassone.Core.Calculation.River.Tiles
{

    /// <summary>
    ///       F
    ///   |       |
    /// W |+++O+++| W
    ///   |   |   |
    ///       R
    /// </summary>
    public class FWRW : Tile
    {
        protected string churchPartName = "Church_0";
        protected string cornfieldPart0Name = "Cornfield_0";
        protected string cornfieldPart1Name = "Cornfield_1";
        protected string cornfieldPart2Name = "Cornfield_2";
        protected string roadPartName = "Road_0";

        public FWRW(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var churchPart = new MonasteryPart(churchPartName, Id);
            Parts.Add(churchPart);

            var cornfieldPart1 = new FieldPart(cornfieldPart0Name, Id);
            Parts.Add(cornfieldPart1);

            var cornfieldPart2 = new FieldPart(cornfieldPart1Name, Id);
            Parts.Add(cornfieldPart2);

            var roadPart = new RoadPart(roadPartName, Id);
            Parts.Add(roadPart);

            var cornfieldPart3 = new FieldPart(cornfieldPart2Name, Id);
            Parts.Add(cornfieldPart3);
        }

        public override void ConnectField(Cell field, Grid grid)
        {
            ((MonasteryPart)GetPart(churchPartName)).CellId = field.Id;

            AddBorderToPart(field, CellSide.top, GetPart(cornfieldPart0Name), grid);
            AddCornfieldSplittedBorder(field, CellSide.right, FieldSide.side_1, GetPart(cornfieldPart0Name), grid);
            AddCornfieldSplittedBorder(field, CellSide.left, FieldSide.side_6, GetPart(cornfieldPart0Name), grid);

            AddCornfieldSplittedBorder(field, CellSide.right, FieldSide.side_2, GetPart(cornfieldPart1Name), grid);
            AddCornfieldSplittedBorder(field, CellSide.bottom, FieldSide.side_3, GetPart(cornfieldPart1Name), grid);

            AddBorderToPart(field, CellSide.bottom, GetPart(roadPartName), grid);

            AddCornfieldSplittedBorder(field, CellSide.bottom, FieldSide.side_4, GetPart(cornfieldPart2Name), grid);
            AddCornfieldSplittedBorder(field, CellSide.left, FieldSide.side_5, GetPart(cornfieldPart2Name), grid);
        }
    }
}