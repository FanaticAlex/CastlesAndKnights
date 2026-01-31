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
            CityPart.Sides.Add(Side.top);
            Parts.Add(CityPart);

            var CityPart1 = new CityPart(CityPart1Name, this);
            CityPart1.Sides.Add(Side.right);
            Parts.Add(CityPart1);

            var FarmPart = new FarmPart(FarmPartName, this);
            FarmPart.Sides.Add(Side.bottom);
            FarmPart.Sides.Add(Side.left);
            Parts.Add(FarmPart);

            FarmToCityParts.Add(FarmPart, new List<CityPart>() { CityPart, CityPart1 });
        }
    }
}