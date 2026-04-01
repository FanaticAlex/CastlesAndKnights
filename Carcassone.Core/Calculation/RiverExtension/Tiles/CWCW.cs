using Carcassone.Core.Tiles;
using System.Collections.Generic;
using Carcassone.Core.Calculation.Base.Cities;
using Carcassone.Core.Calculation.Base.Farms;

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
        protected string CityPart0Name = "City_0";
        protected string CityPart1Name = "City_1";
        protected string FarmPart0Name = "Farm_0";
        protected string FarmPart1Name = "Farm_1";

        public CWCW(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var CityPart0 = new CityPart(CityPart0Name, this);
            CityPart0.AddSide(Side.top);
            Parts.Add(CityPart0);

            var CityPart1 = new CityPart(CityPart1Name, this);
            CityPart1.AddSide(Side.bottom);
            Parts.Add(CityPart1);

            var FarmPart0 = new FarmPart(FarmPart0Name, this);
            FarmPart0.AddSide(Side.side_1);
            FarmPart0.AddSide(Side.side_6);
            Parts.Add(FarmPart0);

            var FarmPart1 = new FarmPart(FarmPart1Name, this);
            FarmPart1.AddSide(Side.side_2);
            FarmPart1.AddSide(Side.side_5);
            Parts.Add(FarmPart1);


            FarmPart0.ConnectedCityParts.Add(CityPart0);
            FarmPart1.ConnectedCityParts.Add(CityPart1);
        }
    }
}