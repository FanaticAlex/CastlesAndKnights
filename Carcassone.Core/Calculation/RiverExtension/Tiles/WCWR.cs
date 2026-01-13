using Carcassone.Core.Board;
using Carcassone.Core.Calculation.Base.Cities;
using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.Base.Roads;
using Carcassone.Core.Calculation.RiverExtension.Rivers;
using Carcassone.Core.Tiles;
using System.Collections.Generic;

namespace Carcassone.Core.Calculation.RiverExtension.Tiles
{
    /// <summary>
    ///       W
    ///   |   +  /|
    /// R |-----| | C
    ///   |   +  \|
    ///       W
    /// </summary>
    public class WCWR : Tile
    {
        protected string RiverPart0Name = "River_0";
        protected string CityPart0Name = "City_0";
        protected string FarmPart0Name = "Farm_0";
        protected string FarmPart1Name = "Farm_1";
        protected string FarmPart2Name = "Farm_2";
        protected string FarmPart3Name = "Farm_3";
        protected string roadPartName = "Road_0";

        public WCWR(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var RiverPart0 = new RiverPart(RiverPart0Name, Id);
            Parts.Add(RiverPart0);

            var CityPart0 = new CityPart(CityPart0Name, Id);
            Parts.Add(CityPart0);

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

            FieldToCityParts.Add(FarmPart0.PartId, new List<string>() { CityPart0.PartId });
            FieldToCityParts.Add(FarmPart1.PartId, new List<string>() { CityPart0.PartId });
        }

        public override void ConnectField(Cell cell, Grid grid)
        {
            AddBorderToPart(cell, CellSide.top, GetPart(RiverPart0Name), grid);
            AddBorderToPart(cell, CellSide.bottom, GetPart(RiverPart0Name), grid);

            AddBorderToPart(cell, CellSide.right, GetPart(CityPart0Name), grid);

            AddFarmSplittedBorder(cell, CellSide.top, FieldSide.side_0, GetPart(FarmPart0Name), grid);

            AddFarmSplittedBorder(cell, CellSide.bottom, FieldSide.side_3, GetPart(FarmPart1Name), grid);

            AddFarmSplittedBorder(cell, CellSide.bottom, FieldSide.side_4, GetPart(FarmPart2Name), grid);
            AddFarmSplittedBorder(cell, CellSide.left, FieldSide.side_5, GetPart(FarmPart2Name), grid);

            AddFarmSplittedBorder(cell, CellSide.left, FieldSide.side_6, GetPart(FarmPart3Name), grid);
            AddFarmSplittedBorder(cell, CellSide.top, FieldSide.side_7, GetPart(FarmPart3Name), grid);

            AddBorderToPart(cell, CellSide.left, GetPart(roadPartName), grid);
        }
    }
}