using Carcassone.Core.Board;
using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.Base.Roads;
using Carcassone.Core.Tiles;

namespace Carcassone.Core.Calculation.Base.Tiles
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
        protected string FarmPartName = "Farm_0";
        protected string FarmPart1Name = "Farm_1";

        public RFRF(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var roadPart1 = new RoadPart(roadPartName, Id);
            Parts.Add(roadPart1);

            var FarmPart1 = new FieldPart(FarmPartName, Id);
            Parts.Add(FarmPart1);

            var FarmPart2 = new FieldPart(FarmPart1Name, Id);
            Parts.Add(FarmPart2);
        }

        public override void ConnectField(Cell field, Grid grid)
        {
            AddBorderToPart(field, CellSide.top, GetPart(roadPartName), grid);
            AddBorderToPart(field, CellSide.bottom, GetPart(roadPartName), grid);

            AddFarmSplittedBorder(field, CellSide.top, FieldSide.side_0, GetPart(FarmPartName), grid);
            AddBorderToPart(field, CellSide.right, GetPart(FarmPartName), grid);
            AddFarmSplittedBorder(field, CellSide.bottom, FieldSide.side_3, GetPart(FarmPartName), grid);

            AddFarmSplittedBorder(field, CellSide.bottom, FieldSide.side_4, GetPart(FarmPart1Name), grid);
            AddBorderToPart(field, CellSide.left, GetPart(FarmPart1Name), grid);
            AddFarmSplittedBorder(field, CellSide.top, FieldSide.side_7, GetPart(FarmPart1Name), grid);
        }
    }
}