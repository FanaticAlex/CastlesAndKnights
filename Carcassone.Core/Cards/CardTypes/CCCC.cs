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
            var castlePart = new CastlePart(castlePartName, Id);
            Parts.Add(castlePart);
        }

        public override void ConnectField(Field field, FieldBoard fieldBoard)
        {
            AddBorderToPart(field, FieldSide.top, GetPart(castlePartName), fieldBoard);
            AddBorderToPart(field, FieldSide.right, GetPart(castlePartName), fieldBoard);
            AddBorderToPart(field, FieldSide.bottom, GetPart(castlePartName), fieldBoard);
            AddBorderToPart(field, FieldSide.left, GetPart(castlePartName), fieldBoard);
        }
    }
}