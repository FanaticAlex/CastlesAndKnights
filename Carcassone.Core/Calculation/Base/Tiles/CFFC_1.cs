using Carcassone.Core.Board;
using Carcassone.Core.Calculation.Base.Cities;
using System.Collections.Generic;

namespace Carcassone.Core.Calculation.Base.Tiles
{

    /// <summary>
    ///       C
    ///   | *  __/|
    /// C | __/   | F
    ///   |/      |
    ///       F
    /// </summary>
    public class CFFC_1 : CFFC
    {
        public CFFC_1(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            ((CityPart)GetPart(castlePartName)).IsThereShield = true;
        }
    }
}