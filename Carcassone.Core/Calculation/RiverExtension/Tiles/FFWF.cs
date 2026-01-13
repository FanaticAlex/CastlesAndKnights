using Carcassone.Core.Board;
using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.RiverExtension.Rivers;
using Carcassone.Core.Tiles;

namespace Carcassone.Core.Calculation.RiverExtension.Tiles
{

    /// <summary>
    ///       F
    ///   |       |
    /// F |   +   | F
    ///   |   +   |
    ///       W
    /// </summary>
    public class FFWF : Tile
    {
        protected string RiverPart0Name = "River_0";
        protected string FarmPartName = "Farm_0";

        public FFWF(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var RiverPart0 = new RiverPart(RiverPart0Name, Id);
            Parts.Add(RiverPart0);

            var FarmPart = new FieldPart(FarmPartName, Id);
            Parts.Add(FarmPart);
        }

        public override void ConnectField(Cell cell, Grid grid)
        {
            AddBorderToPart(cell, CellSide.bottom, GetPart(RiverPart0Name), grid);

            AddBorderToPart(cell, CellSide.left, GetPart(FarmPartName), grid);
            AddBorderToPart(cell, CellSide.top, GetPart(FarmPartName), grid);
            AddBorderToPart(cell, CellSide.right, GetPart(FarmPartName), grid);
            AddFarmSplittedBorder(cell, CellSide.bottom, FieldSide.side_3, GetPart(FarmPartName), grid);
            AddFarmSplittedBorder(cell, CellSide.bottom, FieldSide.side_4, GetPart(FarmPartName), grid);
        }
    }
}