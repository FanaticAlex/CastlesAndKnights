using Carcassone.Core.Fields;
using System.Collections.Generic;

namespace Carcassone.Core.Cards
{

    /// <summary>
    ///       F
    ///   |\_____/|
    /// C | _____ | C
    ///   |/     \|
    ///       F
    /// </summary>
    public class FCFC_1 : Card
    {
        protected string castlePartName = "Castle_0";
        protected string cornfieldPartName = "Cornfield_0";
        protected string cornfieldPart1Name = "Cornfield_1";

        public FCFC_1(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var castlePart = new CastlePart(castlePartName, Id);
            Parts.Add(castlePart);

            var cornfieldPart1 = new CornfieldPart(cornfieldPartName, Id);
            Parts.Add(cornfieldPart1);

            var cornfieldPart2 = new CornfieldPart(cornfieldPart1Name, Id);
            Parts.Add(cornfieldPart2);


            FieldToCastleParts.Add(cornfieldPart1.PartId, new List<string>() { castlePart.PartId });
            FieldToCastleParts.Add(cornfieldPart2.PartId, new List<string>() { castlePart.PartId });
        }


        public override void ConnectField(Field field, FieldBoard fieldBoard)
        {
            AddBorderToPart(field, FieldSide.right, GetPart(castlePartName), fieldBoard);
            AddBorderToPart(field, FieldSide.left, GetPart(castlePartName), fieldBoard);

            AddBorderToPart(field, FieldSide.top, GetPart(cornfieldPartName), fieldBoard);

            AddBorderToPart(field, FieldSide.bottom, GetPart(cornfieldPart1Name), fieldBoard);
        }
    }
}