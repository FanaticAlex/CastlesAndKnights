using Carcassone.Core.Board;
using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.RiverExtension.Rivers;
using Carcassone.Core.Tiles;

namespace Carcassone.Core.Calculation.RiverExtension.Tiles
{
    /// <summary>
    ///       W
    ///   |   +   |
    /// F |   +   | F
    ///   |       |
    ///       F
    /// </summary>
    public class WFFF : Tile
    {
        protected string RiverPart0Name = "River_0";
        protected string FarmPartName = "Farm_0";

        public WFFF(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var RiverPart0 = new RiverPart(RiverPart0Name, Id);
            Parts.Add(RiverPart0);

            var FarmPart1 = new FieldPart(FarmPartName, Id);
            Parts.Add(FarmPart1);
        }

        public override void ConnectField(Cell cell, Grid grid)
        {
            AddBorderToPart(cell, CellSide.top, GetPart(RiverPart0Name), grid);

            AddFarmSplittedBorder(cell, CellSide.top, FieldSide.side_0, GetPart(FarmPartName), grid);
            AddFarmSplittedBorder(cell, CellSide.top, FieldSide.side_7, GetPart(FarmPartName), grid);
            AddBorderToPart(cell, CellSide.right, GetPart(FarmPartName), grid);
            AddBorderToPart(cell, CellSide.bottom, GetPart(FarmPartName), grid);
            AddBorderToPart(cell, CellSide.left, GetPart(FarmPartName), grid);
        }
    }
}