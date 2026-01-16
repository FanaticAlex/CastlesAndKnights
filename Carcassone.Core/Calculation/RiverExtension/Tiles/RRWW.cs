using Carcassone.Core.Board;
using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.Base.Roads;
using Carcassone.Core.Calculation.RiverExtension.Rivers;
using Carcassone.Core.Tiles;

namespace Carcassone.Core.Calculation.RiverExtension.Tiles
{
    /// <summary>
    ///       R
    ///   |    \  |
    /// W |+++  \_| R
    ///   |   +   |
    ///       W
    /// </summary>
    public class RRWW : Tile
    {
        protected string RiverPart0Name = "River_0";
        protected string FarmPart0Name = "Farm_0";
        protected string FarmPart1Name = "Farm_1";
        protected string FarmPart2Name = "Farm_2";
        protected string roadPartName = "Road_0";

        public RRWW(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var RiverPart0 = new RiverPart(RiverPart0Name, Id);
            Parts.Add(RiverPart0);

            var FarmPart0 = new FarmPart(FarmPart0Name, Id);
            Parts.Add(FarmPart0);

            var FarmPart1 = new FarmPart(FarmPart1Name, Id);
            Parts.Add(FarmPart1);

            var FarmPart2 = new FarmPart(FarmPart2Name, Id);
            Parts.Add(FarmPart2);

            var roadPart = new RoadPart(roadPartName, Id);
            Parts.Add(roadPart);
        }

        public override void ConnectCell(Cell cell, Grid grid)
        {
            AddBorderToPart(cell, Side.bottom, GetPart(RiverPart0Name), grid);
            AddBorderToPart(cell, Side.left, GetPart(RiverPart0Name), grid);

            AddFarmSplittedBorder(cell, Side.top, FieldSide.side_0, GetPart(FarmPart0Name), grid);
            AddFarmSplittedBorder(cell, Side.right, FieldSide.side_1, GetPart(FarmPart0Name), grid);

            AddFarmSplittedBorder(cell, Side.right, FieldSide.side_2, GetPart(FarmPart1Name), grid);
            AddFarmSplittedBorder(cell, Side.bottom, FieldSide.side_3, GetPart(FarmPart1Name), grid);
            AddFarmSplittedBorder(cell, Side.left, FieldSide.side_6, GetPart(FarmPart1Name), grid);
            AddFarmSplittedBorder(cell, Side.top, FieldSide.side_7, GetPart(FarmPart1Name), grid);

            // поле 3
            AddFarmSplittedBorder(cell, Side.bottom, FieldSide.side_4, GetPart(FarmPart2Name), grid);
            AddFarmSplittedBorder(cell, Side.left, FieldSide.side_5, GetPart(FarmPart2Name), grid);

            // дорога
            AddBorderToPart(cell, Side.top, GetPart(roadPartName), grid);
            AddBorderToPart(cell, Side.right, GetPart(roadPartName), grid);
        }
    }
}