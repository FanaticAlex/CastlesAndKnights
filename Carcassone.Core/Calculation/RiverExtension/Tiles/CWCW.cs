using Carcassone.Core.Tiles;
using Carcassone.Core.Board;
using System.Collections.Generic;
using Carcassone.Core.Calculation.Base.Cities;
using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.RiverExtension.Rivers;

namespace Carcassone.Core.Calculation.RiverExtension.Tiles
{

    /// <summary>
    ///       C
    ///   |\-----/|
    /// W |+++++++| W
    ///   |/-----\|
    ///       C
    /// </summary>
    public class CWCW : Tile
    {
        protected string RiverPart0Name = "River_0";
        protected string CityPart0Name = "City_0";
        protected string CityPart1Name = "City_1";
        protected string FarmPart0Name = "Farm_0";
        protected string FarmPart1Name = "Farm_1";

        public CWCW(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var RiverPart0 = new RiverPart(RiverPart0Name, Id);
            Parts.Add(RiverPart0);

            var CityPart0 = new CityPart(CityPart0Name, Id);
            Parts.Add(CityPart0);

            var CityPart1 = new CityPart(CityPart1Name, Id);
            Parts.Add(CityPart1);

            var FarmPart0 = new FieldPart(FarmPart0Name, Id);
            Parts.Add(FarmPart0);

            var FarmPart1 = new FieldPart(FarmPart1Name, Id);
            Parts.Add(FarmPart1);


            FieldToCityParts.Add(FarmPart0.PartId, new List<string>() { CityPart0.PartId });
            FieldToCityParts.Add(FarmPart1.PartId, new List<string>() { CityPart1.PartId });
        }

        public override void ConnectField(Cell cell, Grid grid)
        {
            AddBorderToPart(cell, CellSide.right, GetPart(RiverPart0Name), grid);
            AddBorderToPart(cell, CellSide.left, GetPart(RiverPart0Name), grid);

            AddBorderToPart(cell, CellSide.top, GetPart(CityPart0Name), grid);

            AddBorderToPart(cell, CellSide.bottom, GetPart(CityPart1Name), grid);

            AddFarmSplittedBorder(cell, CellSide.right, FieldSide.side_1, GetPart(FarmPart0Name), grid);
            AddFarmSplittedBorder(cell, CellSide.left, FieldSide.side_6, GetPart(FarmPart0Name), grid);

            AddFarmSplittedBorder(cell, CellSide.right, FieldSide.side_2, GetPart(FarmPart1Name), grid);
            AddFarmSplittedBorder(cell, CellSide.left, FieldSide.side_5, GetPart(FarmPart1Name), grid);
        }
    }
}