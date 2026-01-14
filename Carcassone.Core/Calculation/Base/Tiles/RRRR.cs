using Carcassone.Core.Board;
using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.Base.Roads;
using Carcassone.Core.Tiles;

namespace Carcassone.Core.Calculation.Base.Tiles
{

    /// <summary>
    ///       R
    ///   |   |   |
    /// R |---|---| R
    ///   |   |   |
    ///       R
    /// </summary>
    public class RRRR : Tile
    {
        protected string roadPartName = "Road_0";
        protected string roadPart1Name = "Road_1";
        protected string roadPart2Name = "Road_2";
        protected string roadPart3Name = "Road_3";
        protected string FarmPartName = "Farm_0";
        protected string FarmPart1Name = "Farm_1";
        protected string FarmPart2Name = "Farm_2";
        protected string FarmPart3Name = "Farm_3";

        public RRRR(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var roadPart1 = new RoadPart(roadPartName, Id);
            Parts.Add(roadPart1);

            var roadPart2 = new RoadPart(roadPart1Name, Id);
            Parts.Add(roadPart2);

            var roadPart3 = new RoadPart(roadPart2Name, Id);
            Parts.Add(roadPart3);

            var roadPart4 = new RoadPart(roadPart3Name, Id);
            Parts.Add(roadPart4);

            var FarmPart1 = new FarmPart(FarmPartName, Id);
            Parts.Add(FarmPart1);

            var FarmPart2 = new FarmPart(FarmPart1Name, Id);
            Parts.Add(FarmPart2);

            var FarmPart3 = new FarmPart(FarmPart2Name, Id);
            Parts.Add(FarmPart3);

            var FarmPart4 = new FarmPart(FarmPart3Name, Id);
            Parts.Add(FarmPart4);
        }

        public override void ConnectCell(Cell cell, Grid grid)
        {
            AddBorderToPart(cell, CellSide.top, GetPart(roadPartName), grid);

            AddBorderToPart(cell, CellSide.right, GetPart(roadPart1Name), grid);

            AddBorderToPart(cell, CellSide.bottom, GetPart(roadPart2Name), grid);

            AddBorderToPart(cell, CellSide.left, GetPart(roadPart3Name), grid);

            AddFarmSplittedBorder(cell, CellSide.top, FieldSide.side_0, GetPart(FarmPartName), grid);
            AddFarmSplittedBorder(cell, CellSide.right, FieldSide.side_1, GetPart(FarmPartName), grid);

            AddFarmSplittedBorder(cell, CellSide.right, FieldSide.side_2, GetPart(FarmPart1Name), grid);
            AddFarmSplittedBorder(cell, CellSide.bottom, FieldSide.side_3, GetPart(FarmPart1Name), grid);

            AddFarmSplittedBorder(cell, CellSide.bottom, FieldSide.side_4, GetPart(FarmPart2Name), grid);
            AddFarmSplittedBorder(cell, CellSide.left, FieldSide.side_5, GetPart(FarmPart2Name), grid);

            AddFarmSplittedBorder(cell, CellSide.left, FieldSide.side_6, GetPart(FarmPart3Name), grid);
            AddFarmSplittedBorder(cell, CellSide.top, FieldSide.side_7, GetPart(FarmPart3Name), grid);
        }
    }
}