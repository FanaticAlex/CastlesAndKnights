using Carcassone.Core.Tiles;
using System.Collections.Generic;
using Carcassone.Core.Calculation.Base.Cities;
using Carcassone.Core.Calculation.Base.Farms;

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
            CityPart.Sides.Add(Side.top);
            CityPart.Sides.Add(Side.left);
            Parts.Add(CityPart);

            var FarmPart = new FarmPart(FarmPartName, this);
            FarmPart.Sides.Add(Side.right);
            FarmPart.Sides.Add(Side.bottom);
            Parts.Add(FarmPart);
         
            FarmToCityParts.Add(FarmPart, new List<CityPart>() { CityPart });
        }
    }
}