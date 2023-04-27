using Carcassone.Core.Fields;
using System.Collections.Generic;
using System.Linq;

namespace Carcassone.Core.Cards
{

    /// <summary>
    ///       C
    ///   |\_____/|
    /// F |       | F
    ///   |       |
    ///       F
    /// </summary>
    public class CFFF : Card
    {
        private string castlePartName = "Castle_0";
        private string cornfieldPartName = "Cornfield_0";

        public CFFF(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var castlePart = new CastlePart(castlePartName, CardId);
            Parts.Add(castlePart);

            var cornfieldPart = new CornfieldPart(cornfieldPartName, CardId);
            Parts.Add(cornfieldPart);


            FieldToCastleParts.Add(cornfieldPart.PartId, new List<string>() { castlePart.PartId });
        }

        public override void ConnectField(Field field, FieldBoard fieldBoard)
        {
            AddBorderToPart(field, FieldSide.top, GetPart(castlePartName), fieldBoard);

            AddBorderToPart(field, FieldSide.right, GetPart(cornfieldPartName), fieldBoard);
            AddBorderToPart(field, FieldSide.bottom, GetPart(cornfieldPartName), fieldBoard);
            AddBorderToPart(field, FieldSide.left, GetPart(cornfieldPartName), fieldBoard);
        }
    }
}