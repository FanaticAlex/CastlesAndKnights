using System.Collections.Generic;
using Carcassone.Core.Board;

namespace Carcassone.Core.Tiles
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
            ((CastlePart)GetPart(castlePartName)).IsThereShield = true;
        }
    }
}