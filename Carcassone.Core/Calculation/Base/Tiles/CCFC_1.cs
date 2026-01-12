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
    public class CCFC_1 : CCFC
    {
        public CCFC_1(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            ((CityPart)GetPart(castlePartName)).IsThereShield = true;
        }
    }
}