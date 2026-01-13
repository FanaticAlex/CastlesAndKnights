using Carcassone.Core.Board;
using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.Base.Roads;
using Carcassone.Core.Calculation.RiverExtension.Rivers;
using Carcassone.Core.Tiles;

namespace Carcassone.Core.Calculation.RiverExtension.Tiles
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
        protected string RiverPart0Name = "River_0";
        protected string FarmPart0Name = "Farm_0";
        protected string FarmPart1Name = "Farm_1";
        protected string roadPartName = "Road_0";
        protected string FarmPart2Name = "Farm_2";
        protected string FarmPart3Name = "Farm_3";

        public RWRW(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var RiverPart0 = new RiverPart(RiverPart0Name, Id);
            Parts.Add(RiverPart0);

            var FarmPart0 = new FieldPart(FarmPart0Name, Id);
            Parts.Add(FarmPart0);

            var FarmPart1 = new FieldPart(FarmPart1Name, Id);
            Parts.Add(FarmPart1);

            var FarmPart2 = new FieldPart(FarmPart2Name, Id);
            Parts.Add(FarmPart2);

            var FarmPart3 = new FieldPart(FarmPart3Name, Id);
            Parts.Add(FarmPart3);

            var roadPart = new RoadPart(roadPartName, Id);
            Parts.Add(roadPart);
        }
        
        public override void ConnectField(Cell cell, Grid grid)
        {
            AddBorderToPart(cell, CellSide.right, GetPart(RiverPart0Name), grid);
            AddBorderToPart(cell, CellSide.left, GetPart(RiverPart0Name), grid);

            AddFarmSplittedBorder(cell, CellSide.top, FieldSide.side_0, GetPart(FarmPart0Name), grid);
            AddFarmSplittedBorder(cell, CellSide.right, FieldSide.side_1, GetPart(FarmPart0Name), grid);

            AddFarmSplittedBorder(cell, CellSide.right, FieldSide.side_2, GetPart(FarmPart1Name), grid);
            AddFarmSplittedBorder(cell, CellSide.bottom, FieldSide.side_3, GetPart(FarmPart1Name), grid);

            AddFarmSplittedBorder(cell, CellSide.bottom, FieldSide.side_4, GetPart(FarmPart2Name), grid);
            AddFarmSplittedBorder(cell, CellSide.left, FieldSide.side_5, GetPart(FarmPart2Name), grid);

            AddFarmSplittedBorder(cell, CellSide.left, FieldSide.side_6, GetPart(FarmPart3Name), grid);
            AddFarmSplittedBorder(cell, CellSide.top, FieldSide.side_7, GetPart(FarmPart3Name), grid);

            // дорога
            AddBorderToPart(cell, CellSide.top, GetPart(roadPartName), grid);
            AddBorderToPart(cell, CellSide.bottom, GetPart(roadPartName), grid);
        }
    }
}