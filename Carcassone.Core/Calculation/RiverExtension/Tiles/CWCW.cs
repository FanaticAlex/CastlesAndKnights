using Carcassone.Core.Tiles;
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
            var RiverPart0 = new RiverPart(RiverPart0Name, this);
            RiverPart0.Sides.Add(Side.right);
            RiverPart0.Sides.Add(Side.left);
            Parts.Add(RiverPart0);

            var CityPart0 = new CityPart(CityPart0Name, this);
            CityPart0.Sides.Add(Side.top);
            Parts.Add(CityPart0);

            var CityPart1 = new CityPart(CityPart1Name, this);
            CityPart1.Sides.Add(Side.bottom);
            Parts.Add(CityPart1);

            var FarmPart0 = new FarmPart(FarmPart0Name, this);
            FarmPart0.Sides.Add(Side.side_1);
            FarmPart0.Sides.Add(Side.side_6);
            Parts.Add(FarmPart0);

            var FarmPart1 = new FarmPart(FarmPart1Name, this);
            FarmPart1.Sides.Add(Side.side_2);
            FarmPart1.Sides.Add(Side.side_5);
            Parts.Add(FarmPart1);


            FarmToCityParts.Add(FarmPart0, new List<CityPart>() { CityPart0 });
            FarmToCityParts.Add(FarmPart1, new List<CityPart>() { CityPart1 });
        }
    }
}