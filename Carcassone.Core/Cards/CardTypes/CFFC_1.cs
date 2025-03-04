using Carcassone.Core.Fields;
using System.Collections.Generic;

namespace Carcassone.Core.Cards
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
            ((CastlePart)GetPart(castlePartName)).IsThereShield = true;
        }
    }
}