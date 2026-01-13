using Carcassone.Core.Board;
using Carcassone.Core.Calculation.Base.Cities;
using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.RiverExtension.Rivers;
using Carcassone.Core.Tiles;
using System.Collections.Generic;

namespace Carcassone.Core.Calculation.RiverExtension.Tiles
{
    /// <summary>
    ///       W
    ///   |   +  /|
    /// W |++ /   | C
    ///   |/      |
    ///       C
    /// </summary>
    public class WCCW : Tile
    {
        protected string RiverPart0Name = "River_0";
        protected string FarmPart0Name = "Farm_0";
        protected string FarmPart1Name = "Farm_1";
        protected string CityPart1Name = "City_0";

        public WCCW(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var RiverPart0 = new RiverPart(RiverPart0Name, Id);
            Parts.Add(RiverPart0);

            var FarmPart0 = new FieldPart(FarmPart0Name, Id);
            Parts.Add(FarmPart0);

            var FarmPart1 = new FieldPart(FarmPart1Name, Id);
            Parts.Add(FarmPart1);

            var CityPart1 = new CityPart(CityPart1Name, Id);
            Parts.Add(CityPart1);

            FieldToCityParts.Add(FarmPart0.PartId, new List<string>() { CityPart1.PartId });
        }

        public override void ConnectField(Cell cell, Grid grid)
        {
            AddBorderToPart(cell, CellSide.top, GetPart(RiverPart0Name), grid);
            AddBorderToPart(cell, CellSide.left, GetPart(RiverPart0Name), grid);

            AddBorderToPart(cell, CellSide.right, GetPart(CityPart1Name), grid);
            AddBorderToPart(cell, CellSide.bottom, GetPart(CityPart1Name), grid);

            AddFarmSplittedBorder(cell, CellSide.top, FieldSide.side_0, GetPart(FarmPart0Name), grid);
            AddFarmSplittedBorder(cell, CellSide.left, FieldSide.side_5, GetPart(FarmPart0Name), grid);

            AddFarmSplittedBorder(cell, CellSide.top, FieldSide.side_7, GetPart(FarmPart1Name), grid);
            AddFarmSplittedBorder(cell, CellSide.left, FieldSide.side_6, GetPart(FarmPart1Name), grid);
        }
    }
}