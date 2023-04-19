using Carcassone.Core.Fields;
using System.Collections.Generic;

namespace Carcassone.Core.Cards
{

    /// <summary>
    ///       C
    ///   | *  __/|
    /// C | __/  _| R
    ///   |/   /  |
    ///       R
    /// </summary>
    public class CRRC_1 : CRRC
    {
        public CRRC_1(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            ((CastlePart)GetPart(castlePartName)).IsThereShield = true;
        }
    }
}