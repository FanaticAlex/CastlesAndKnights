using Carcassone.Core.Fields;
using System.Collections.Generic;

namespace Carcassone.Core.Cards
{

    /// <summary>
    ///       F
    ///   |\     /|
    /// C | |   | | C
    ///   |/     \|
    ///       F
    /// </summary>
    public class FCFC : Card
    {
        protected string castlePart0Name = "Castle_0";
        protected string castlePart1Name = "Castle_1";
        protected string cornfieldPartName = "Cornfield_0";

        public FCFC(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var castlePart0 = new CastlePart(castlePart0Name, Id);
            Parts.Add(castlePart0);

            var castlePart1 = new CastlePart(castlePart1Name, Id);
            Parts.Add(castlePart1);

            var cornfieldPart1 = new CornfieldPart(cornfieldPartName, Id);
            Parts.Add(cornfieldPart1);


            FieldToCastleParts.Add(cornfieldPart1.PartId, new List<string>() { castlePart0.PartId, castlePart1.PartId });
        }

        public override void ConnectField(Field field, FieldBoard fieldBoard)
        {
            AddBorderToPart(field, FieldSide.left, GetPart(castlePart0Name), fieldBoard);

            AddBorderToPart(field, FieldSide.right, GetPart(castlePart1Name), fieldBoard);

            AddBorderToPart(field, FieldSide.top, GetPart(cornfieldPartName), fieldBoard);
            AddBorderToPart(field, FieldSide.bottom, GetPart(cornfieldPartName), fieldBoard);
        }
    }
}