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

            var FarmPart0 = new FarmPart(FarmPart0Name, Id);
            Parts.Add(FarmPart0);

            var FarmPart1 = new FarmPart(FarmPart1Name, Id);
            Parts.Add(FarmPart1);

            var CityPart1 = new CityPart(CityPart1Name, Id);
            Parts.Add(CityPart1);

            FarmToCityParts.Add(FarmPart0.PartId, new List<string>() { CityPart1.PartId });
        }

        public override void ConnectCell(Cell cell, Grid grid)
        {
            AddBorderToPart(cell, Side.top, GetPart(RiverPart0Name), grid);
            AddBorderToPart(cell, Side.left, GetPart(RiverPart0Name), grid);

            AddBorderToPart(cell, Side.right, GetPart(CityPart1Name), grid);
            AddBorderToPart(cell, Side.bottom, GetPart(CityPart1Name), grid);

            AddFarmSplittedBorder(cell, Side.top, FieldSide.side_0, GetPart(FarmPart0Name), grid);
            AddFarmSplittedBorder(cell, Side.left, FieldSide.side_5, GetPart(FarmPart0Name), grid);

            AddFarmSplittedBorder(cell, Side.top, FieldSide.side_7, GetPart(FarmPart1Name), grid);
            AddFarmSplittedBorder(cell, Side.left, FieldSide.side_6, GetPart(FarmPart1Name), grid);
        }
    }
}