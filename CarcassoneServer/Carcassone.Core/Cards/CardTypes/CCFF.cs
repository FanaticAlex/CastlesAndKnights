using Carcassone.Core.Fields;
using System.Collections;
using System.Collections.Generic;

namespace Carcassone.Core.Cards
{

    /// <summary>
    ///       C
    ///   |\_____//|
    /// F |      | | C
    ///   |       \|
    ///       F
    /// </summary>
    public class CCFF : Card
    {
        protected string castlePartName = "Castle_0";
        protected string castlePart1Name = "Castle_1";
        protected string cornfieldPartName = "Cornfield_0";

        public CCFF(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var castlePart = new CastlePart(castlePartName, CardId);
            Parts.Add(castlePart);

            var castlePart1 = new CastlePart(castlePart1Name, CardId);
            Parts.Add(castlePart1);

            var cornfieldPart = new CornfieldPart(cornfieldPartName, CardId);
            Parts.Add(cornfieldPart);

            FieldToCastleParts.Add(cornfieldPart.PartId, new List<string>() { castlePart.PartId, castlePart1.PartId });
        }

        public override void ConnectField(Field field, FieldBoard fieldBoard)
        {
            AddBorderToPart(field, Side.top, GetPart(castlePartName), fieldBoard);

            AddBorderToPart(field, Side.right, GetPart(castlePart1Name), fieldBoard);

            AddBorderToPart(field, Side.bottom, GetPart(cornfieldPartName), fieldBoard);
            AddBorderToPart(field, Side.left, GetPart(cornfieldPartName), fieldBoard);
        }
    }
}