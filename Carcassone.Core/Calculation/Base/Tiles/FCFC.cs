using Carcassone.Core.Board;
using Carcassone.Core.Tiles;
using System.Collections.Generic;
using Carcassone.Core.Calculation.Base.Cities;
using Carcassone.Core.Calculation.Base.Farms;

namespace Carcassone.Core.Calculation.Base.Tiles
{

    /// <summary>
    ///       F
    ///   |\     /|
    /// C | |   | | C
    ///   |/     \|
    ///       F
    /// </summary>
    public class FCFC : Tile
    {
        protected string CityPart0Name = "City_0";
        protected string CityPart1Name = "City_1";
        protected string FarmPartName = "Farm_0";

        public FCFC(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var CityPart0 = new CityPart(CityPart0Name, this);
            CityPart0.Sides.Add(Side.left);
            Parts.Add(CityPart0);

            var CityPart1 = new CityPart(CityPart1Name, this);
            CityPart1.Sides.Add(Side.right);
            Parts.Add(CityPart1);

            var FarmPart1 = new FarmPart(FarmPartName, this);
            FarmPart1.Sides.Add(Side.top);
            FarmPart1.Sides.Add(Side.bottom);
            Parts.Add(FarmPart1);


            FarmToCityParts.Add(FarmPart1, new List<CityPart>() { CityPart0, CityPart1 });
        }
    }
}