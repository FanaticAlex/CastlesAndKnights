using Carcassone.Core.Tiles;
using System.Collections.Generic;
using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.Base.Farms;
using System.Linq;

namespace Carcassone.Core.Calculation.Base.Tiles
{

    /// <summary>
    ///       C
    ///   |\_____/|
    /// F |       | F
    ///   |       |
    ///       F
    /// </summary>
    public class CFFF : Tile
    {
        private string CityPartName = "City_0";
        private string FarmPartName = "Farm_0";

        public CFFF(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var CityPart = new CityPart(CityPartName, this);
            CityPart.AddSide(Side.top);
            Parts.Add(CityPart);

            var FarmPart = new FarmPart(FarmPartName, this);
            FarmPart.AddSide(Side.right);
            FarmPart.AddSide(Side.bottom);
            FarmPart.AddSide(Side.left);
            Parts.Add(FarmPart);


            FarmPart.ConnectedCityParts.Add(CityPart);
        }
    }
}