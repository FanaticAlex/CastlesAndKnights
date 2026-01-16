using Carcassone.Core.Board;
using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.Base.Roads;
using Carcassone.Core.Tiles;

namespace Carcassone.Core.Calculation.Base.Tiles
{

    /// <summary>
    ///       F
    ///   |       |
    /// R |_      | F
    ///   |  \    |
    ///       R
    /// </summary>
    public class FFRR : Tile
    {
        protected string roadPartName = "Road_0";
        protected string FarmPartName = "Farm_0";
        protected string FarmPart1Name = "Farm_1";

        public FFRR(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var roadPart = new RoadPart(roadPartName, Id);
            Parts.Add(roadPart);

            var FarmPart1 = new FarmPart(FarmPartName, Id);
            Parts.Add(FarmPart1);

            var FarmPart2 = new FarmPart(FarmPart1Name, Id);
            Parts.Add(FarmPart2);
        }

        public override void ConnectCell(Cell cell, Grid grid)
        {
            AddBorderToPart(cell, Side.bottom, GetPart(roadPartName), grid);
            AddBorderToPart(cell, Side.left, GetPart(roadPartName), grid);

            AddBorderToPart(cell, Side.top, GetPart(FarmPartName), grid);
            AddBorderToPart(cell, Side.right, GetPart(FarmPartName), grid);
            AddFarmSplittedBorder(cell, Side.bottom, FieldSide.side_3, GetPart(FarmPartName), grid);
            AddFarmSplittedBorder(cell, Side.left, FieldSide.side_6, GetPart(FarmPartName), grid);

            AddFarmSplittedBorder(cell, Side.bottom, FieldSide.side_4, GetPart(FarmPart1Name), grid);
            AddFarmSplittedBorder(cell, Side.left, FieldSide.side_5, GetPart(FarmPart1Name), grid);
        }
    }
}