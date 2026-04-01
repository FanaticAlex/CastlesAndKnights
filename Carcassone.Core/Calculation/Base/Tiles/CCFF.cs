using Carcassone.Core.Tiles;
using System.Collections;
using System.Collections.Generic;
using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.Base.Cities;

namespace Carcassone.Core.Calculation.Base.Tiles
{

    /// <summary>
    ///       C
    ///   |\_____//|
    /// F |      | | C
    ///   |       \|
    ///       F
    /// </summary>
    public class CCFF : Tile
    {
        protected string CityPartName = "City_0";
        protected string CityPart1Name = "City_1";
        protected string FarmPartName = "Farm_0";

        public CCFF(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var CityPart = new CityPart(CityPartName, this);
            CityPart.AddSide(Side.top);
            Parts.Add(CityPart);

            var CityPart1 = new CityPart(CityPart1Name, this);
            CityPart1.AddSide(Side.right);
            Parts.Add(CityPart1);

            var FarmPart = new FarmPart(FarmPartName, this);
            FarmPart.AddSide(Side.bottom);
            FarmPart.AddSide(Side.left);
            Parts.Add(FarmPart);

            FarmPart.ConnectedCityParts.Add(CityPart);
            FarmPart.ConnectedCityParts.Add(CityPart1);
        }
    }
}