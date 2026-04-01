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
            CityPart.AddSide(Side.top);
            CityPart.AddSide(Side.right);
            CityPart.AddSide(Side.bottom);
            CityPart.AddSide(Side.left);
            Parts.Add(CityPart);
        }
    }
}