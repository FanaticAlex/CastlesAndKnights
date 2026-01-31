using Carcassone.Core.Board;
using Carcassone.Core.Tiles;
using System.Collections;
using System.Collections.Generic;
using Carcassone.Core.Calculation.Base.Cities;
using Carcassone.Core.Calculation.Base.Farms;

namespace Carcassone.Core.Calculation.Base.Tiles
{

    /// <summary>
    ///       C
    ///   |       |
    /// C | _____ | C
    ///   |/     \|
    ///       F
    /// </summary>
    public class CCFC : Tile
    {
        protected string CityPartName = "City_0";
        protected string FarmPartName = "Farm_0";

        public CCFC(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var CityPart = new CityPart(CityPartName, this);
            CityPart.Sides.Add(Side.top);
            CityPart.Sides.Add(Side.right);
            CityPart.Sides.Add(Side.left);
            Parts.Add(CityPart);

            var FarmPart = new FarmPart(FarmPartName, this);
            FarmPart.Sides.Add(Side.bottom);
            Parts.Add(FarmPart);

            FarmToCityParts.Add(FarmPart, new List<CityPart>() { CityPart });
        }
    }
}