using Carcassone.Core.Board;
using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.RiverExtension.Rivers;
using Carcassone.Core.Tiles;

namespace Carcassone.Core.Calculation.RiverExtension.Tiles
{
    /// <summary>
    ///       F
    ///   |       |
    /// F |   ++++| W
    ///   |   +   |
    ///       W
    /// </summary>
    public class FWWF : Tile
    {
        protected string RiverPart0Name = "River_0";
        protected string FarmPart0Name = "Farm_0";
        protected string FarmPart1Name = "Farm_1";

        public FWWF(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var RiverPart0 = new RiverPart(RiverPart0Name, Id);
            Parts.Add(RiverPart0);

            var FarmPart1 = new FieldPart(FarmPart0Name, Id);
            Parts.Add(FarmPart1);

            var FarmPart2 = new FieldPart(FarmPart1Name, Id);
            Parts.Add(FarmPart2);
        }

        public override void ConnectField(Cell cell, Grid grid)
        {
            AddBorderToPart(cell, CellSide.right, GetPart(RiverPart0Name), grid);
            AddBorderToPart(cell, CellSide.bottom, GetPart(RiverPart0Name), grid);

            AddBorderToPart(cell, CellSide.top, GetPart(FarmPart0Name), grid);
            AddFarmSplittedBorder(cell, CellSide.right, FieldSide.side_1, GetPart(FarmPart0Name), grid);
            AddFarmSplittedBorder(cell, CellSide.bottom, FieldSide.side_4, GetPart(FarmPart0Name), grid);
            AddBorderToPart(cell, CellSide.left, GetPart(FarmPart0Name), grid);

            AddFarmSplittedBorder(cell, CellSide.right, FieldSide.side_2, GetPart(FarmPart1Name), grid);
            AddFarmSplittedBorder(cell, CellSide.bottom, FieldSide.side_3, GetPart(FarmPart1Name), grid);
        }
    }
}