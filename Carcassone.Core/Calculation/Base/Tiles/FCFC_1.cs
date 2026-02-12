using Carcassone.Core.Tiles;
using System.Collections.Generic;
using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.Base.Cities;

namespace Carcassone.Core.Calculation.Base.Tiles
{

    /// <summary>
    ///       F
    ///   |\_____/|
    /// C | _____ | C
    ///   |/     \|
    ///       F
    /// </summary>
    public class FCFC_1 : Tile
    {
        protected string CityPartName = "City_0";
        protected string FarmPartName = "Farm_0";
        protected string FarmPart1Name = "Farm_1";

        public FCFC_1(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var CityPart = new CityPart(CityPartName, this);
            CityPart.Sides.Add(Side.right);
            CityPart.Sides.Add(Side.left);
            Parts.Add(CityPart);

            var FarmPart1 = new FarmPart(FarmPartName, this);
            FarmPart1.Sides.Add(Side.top);
            Parts.Add(FarmPart1);

            var FarmPart2 = new FarmPart(FarmPart1Name, this);
            FarmPart2.Sides.Add(Side.bottom);
            Parts.Add(FarmPart2);

            FarmToCityParts.Add(FarmPart1, new List<CityPart>() { CityPart });
            FarmToCityParts.Add(FarmPart2, new List<CityPart>() { CityPart });
        }
    }
}