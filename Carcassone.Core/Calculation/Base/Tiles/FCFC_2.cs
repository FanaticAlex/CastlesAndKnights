using System.Collections.Generic;
using Carcassone.Core.Board;
using Carcassone.Core.Calculation.Base.Cities;

namespace Carcassone.Core.Calculation.Base.Tiles
{

    /// <summary>
    ///       F
    ///   |\_____/|
    /// C | __*__ | C
    ///   |/     \|
    ///       F
    /// </summary>
    public class FCFC_2 : FCFC_1
    {
        public FCFC_2(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            ((CityPart)GetPart(castlePartName)).IsThereShield = true;
        }
    }
}