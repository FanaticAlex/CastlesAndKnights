using Carcassone.Core.Board;
using Carcassone.Core.Tiles;
using System.Collections.Generic;
using Carcassone.Core.Calculation.Base.Cities;
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
            CityPart.Sides.Add(Side.top);
            Parts.Add(CityPart);

            var FarmPart = new FarmPart(FarmPartName, this);
            FarmPart.Sides.Add(Side.right);
            FarmPart.Sides.Add(Side.bottom);
            FarmPart.Sides.Add(Side.left);
            Parts.Add(FarmPart);


            FarmToCityParts.Add(FarmPart, new List<CityPart>() { CityPart });
        }
    }
}