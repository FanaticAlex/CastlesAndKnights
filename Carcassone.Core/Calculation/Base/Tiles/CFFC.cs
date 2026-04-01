using Carcassone.Core.Tiles;
using System.Collections.Generic;
using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.Base.Cities;

namespace Carcassone.Core.Calculation.Base.Tiles
{

    /// <summary>
    ///       C
    ///   |    __/|
    /// C | __/   | F
    ///   |/      |
    ///       F
    /// </summary>
    public class CFFC : Tile
    {
        protected string CityPartName = "City_0";
        protected string FarmPartName = "Farm_0";

        public CFFC(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var CityPart = new CityPart(CityPartName, this);
            CityPart.AddSide(Side.top);
            CityPart.AddSide(Side.left);
            Parts.Add(CityPart);

            var FarmPart = new FarmPart(FarmPartName, this);
            FarmPart.AddSide(Side.right);
            FarmPart.AddSide(Side.bottom);
            Parts.Add(FarmPart);
         
            FarmPart.ConnectedCityParts.Add(CityPart);
        }
    }
}