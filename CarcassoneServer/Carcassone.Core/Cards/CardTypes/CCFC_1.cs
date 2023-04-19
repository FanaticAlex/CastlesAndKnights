namespace Carcassone.Core.Cards
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
            ((CastlePart)GetPart(castlePartName)).IsThereShield = true;
        }
    }
}