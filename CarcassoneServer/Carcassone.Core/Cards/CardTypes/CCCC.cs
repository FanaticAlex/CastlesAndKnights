using System.Collections.Generic;
using Carcassone.Core.Fields;

namespace Carcassone.Core.Cards
{

    /// <summary>
    ///       C
    ///   |       |
    /// C |       | C
    ///   |       |
    ///       C
    /// </summary>
    public class CCCC : Card
    {
        protected string castlePartName = "Castle_0";

        public CCCC(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var castlePart = new CastlePart(castlePartName, CardId);
            Parts.Add(castlePart);
        }

        public override void ConnectField(Field field, FieldBoard fieldBoard)
        {
            AddBorderToPart(field, Side.top, GetPart(castlePartName), fieldBoard);
            AddBorderToPart(field, Side.right, GetPart(castlePartName), fieldBoard);
            AddBorderToPart(field, Side.bottom, GetPart(castlePartName), fieldBoard);
            AddBorderToPart(field, Side.left, GetPart(castlePartName), fieldBoard);
        }
    }
}