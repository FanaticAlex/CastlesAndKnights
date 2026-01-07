using Carcassone.Core.Board;
using System.Collections.Generic;

namespace Carcassone.Core.Tiles
{

    /// <summary>
    ///       C
    ///   |   *   |
    /// C | _____ | C
    ///   |/  |  \|
    ///       R
    /// </summary>
    public class CCRC_1 : CCRC
    {
        public CCRC_1(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            ((CityPart)GetPart(castlePartName)).IsThereShield = true;
        }
    }
}