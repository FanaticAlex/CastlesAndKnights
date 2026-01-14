using Carcassone.Core.Board;
using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.Base.Monasteries;
using Carcassone.Core.Calculation.Base.Roads;
using Carcassone.Core.Tiles;

namespace Carcassone.Core.Calculation.Base.Tiles
{
    /// <summary>
    ///       F
    ///   |       |
    /// F |   O   | F
    ///   |   |   |
    ///       R
    /// </summary>
    public class FFRF : Tile
    {
        protected string roadPartName = "Road_0";
        protected string churchPartName = "Church_0";
        protected string FarmPartName = "Farm_0";

        public FFRF(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var roadPart3 = new RoadPart(roadPartName, Id);
            Parts.Add(roadPart3);

            var churchPart = new MonasteryPart(churchPartName, Id);
            Parts.Add(churchPart);

            var FarmPart1 = new FarmPart(FarmPartName, Id);
            Parts.Add(FarmPart1);
        }

        public override void ConnectCell(Cell cell, Grid grid)
        {
            AddBorderToPart(cell, CellSide.bottom, GetPart(roadPartName), grid);

            ((MonasteryPart)GetPart(churchPartName)).CellId = cell.Id;

            AddBorderToPart(cell, CellSide.top, GetPart(FarmPartName), grid);
            AddBorderToPart(cell, CellSide.right, GetPart(FarmPartName), grid);
            AddBorderToPart(cell, CellSide.left, GetPart(FarmPartName), grid);
            AddFarmSplittedBorder(cell, CellSide.bottom, FieldSide.side_3, GetPart(FarmPartName), grid);
            AddFarmSplittedBorder(cell, CellSide.bottom, FieldSide.side_4, GetPart(FarmPartName), grid);
        }
    }
}