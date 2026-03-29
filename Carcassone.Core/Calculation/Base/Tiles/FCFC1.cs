using Carcassone.Core.Tiles;
using System.Collections.Generic;
using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.Base.Farms;

namespace Carcassone.Core.Calculation.Base.Tiles
{

    /// <summary>
    ///       F
    ///   |\_____/|
    /// C | _____ | C
    ///   |/     \|
    ///       F
    /// </summary>
    public class FCFC1 : Tile
    {
        protected string CityPartName = "City_0";
        protected string FarmPartName = "Farm_0";
        protected string FarmPart1Name = "Farm_1";

        public FCFC1(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var CityPart = new CityPart(CityPartName, this);
            CityPart.AddSide(Side.right);
            CityPart.AddSide(Side.left);
            Parts.Add(CityPart);

            var FarmPart1 = new FarmPart(FarmPartName, this);
            FarmPart1.AddSide(Side.top);
            Parts.Add(FarmPart1);

            var FarmPart2 = new FarmPart(FarmPart1Name, this);
            FarmPart2.AddSide(Side.bottom);
            Parts.Add(FarmPart2);

            FarmPart1.ConnectedCityParts.Add(CityPart);
            FarmPart2.ConnectedCityParts.Add(CityPart);
        }
    }
}