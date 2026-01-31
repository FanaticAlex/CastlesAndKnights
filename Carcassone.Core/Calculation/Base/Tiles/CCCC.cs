using System.Collections.Generic;
using Carcassone.Core.Board;
using Carcassone.Core.Calculation.Base.Cities;
using Carcassone.Core.Tiles;

namespace Carcassone.Core.Calculation.Base.Tiles
{

    /// <summary>
    ///       C
    ///   |       |
    /// C |       | C
    ///   |       |
    ///       C
    /// </summary>
    public class CCCC : Tile
    {
        protected string cityPartName = "City_0";

        public CCCC(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var CityPart = new CityPart(cityPartName, this);
            CityPart.Sides.Add(Side.top);
            CityPart.Sides.Add(Side.right);
            CityPart.Sides.Add(Side.bottom);
            CityPart.Sides.Add(Side.left);
            Parts.Add(CityPart);
        }
    }
}