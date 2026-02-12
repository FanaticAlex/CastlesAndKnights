using Carcassone.Core.Calculation.Base.Cities;
using System.Collections.Generic;

namespace Carcassone.Core.Calculation.Base.Tiles
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
            ((CityPart)GetPart(CityPartName)).IsThereShield = true;
        }
    }
}