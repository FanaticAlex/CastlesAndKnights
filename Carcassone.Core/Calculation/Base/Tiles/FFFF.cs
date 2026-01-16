using Carcassone.Core.Board;
using Carcassone.Core.Calculation.Base.Monasteries;
using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Tiles;

namespace Carcassone.Core.Calculation.Base.Tiles
{
    /// <summary>
    ///       F
    ///   |       |
    /// F |   O   | F
    ///   |       |
    ///       F
    /// </summary>
    public class FFFF : Tile
    {
        protected string churchPartName = "Church_0";
        protected string FarmPartName = "Farm_0";

        public FFFF(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var churchPart = new MonasteryPart(churchPartName, Id);
            Parts.Add(churchPart);

            var FarmPart1 = new FarmPart(FarmPartName, Id);
            Parts.Add(FarmPart1);
        }

        public override void ConnectCell(Cell cell, Grid grid)
        {
            ((MonasteryPart)GetPart(churchPartName)).CellId = cell.Id;

            AddBorderToPart(cell, Side.top, GetPart(FarmPartName), grid);
            AddBorderToPart(cell, Side.right, GetPart(FarmPartName), grid);
            AddBorderToPart(cell, Side.bottom, GetPart(FarmPartName), grid);
            AddBorderToPart(cell, Side.left, GetPart(FarmPartName), grid);
        }
    }
}