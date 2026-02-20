using Carcassone.Core.Calculation.Base.Cities;

namespace Carcassone.Core.Calculation.Base.Tiles
{

    /// <summary>
    ///       C
    ///   |   *   |
    /// C | _____ | C
    ///   |/     \|
    ///       F
    /// </summary>
    public class CCFC1 : CCFC
    {
        public CCFC1(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            ((CityPart)GetPart(CityPartName)).IsThereShield = true;
        }
    }
}