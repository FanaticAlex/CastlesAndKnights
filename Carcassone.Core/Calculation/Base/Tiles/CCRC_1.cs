using Carcassone.Core.Board;
using Carcassone.Core.Calculation.Base.Cities;
using System.Collections.Generic;

namespace Carcassone.Core.Calculation.Base.Tiles
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
            ((CityPart)GetPart(CityPartName)).IsThereShield = true;
        }
    }
}