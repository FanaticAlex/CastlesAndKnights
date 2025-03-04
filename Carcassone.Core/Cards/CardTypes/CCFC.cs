using Carcassone.Core.Fields;
using System.Collections;
using System.Collections.Generic;

namespace Carcassone.Core.Cards
{

    /// <summary>
    ///       C
    ///   |       |
    /// C | _____ | C
    ///   |/     \|
    ///       F
    /// </summary>
    public class CCFC : Card
    {
        protected string castlePartName = "Castle_0";
        protected string cornfieldPartName = "Cornfield_0";

        public CCFC(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var castlePart = new CastlePart(castlePartName, Id);
            Parts.Add(castlePart);

            var cornfieldPart = new CornfieldPart(cornfieldPartName, Id);
            Parts.Add(cornfieldPart);

            FieldToCastleParts.Add(cornfieldPart.PartId, new List<string>() { castlePart.PartId });
        }

        public override void ConnectField(Field field, FieldBoard fieldBoard)
        {
            AddBorderToPart(field, FieldSide.top, GetPart(castlePartName), fieldBoard);
            AddBorderToPart(field, FieldSide.right, GetPart(castlePartName), fieldBoard);
            AddBorderToPart(field, FieldSide.left, GetPart(castlePartName), fieldBoard);

            AddBorderToPart(field, FieldSide.bottom, GetPart(cornfieldPartName), fieldBoard);
        }
    }
}