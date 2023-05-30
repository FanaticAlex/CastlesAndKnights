using Carcassone.Core.Cards;
using Carcassone.Core.Fields;
using System.Collections.Generic;

namespace Carcassone.Core.Extensions.River.Cards
{
    /// <summary>
    ///       W
    ///   |   +  /|
    /// W |++ /   | C
    ///   |/      |
    ///       C
    /// </summary>
    public class WCCW : Card
    {
        protected string cornfieldPart0Name = "Cornfield_0";
        protected string cornfieldPart1Name = "Cornfield_1";
        protected string castlePart1Name = "Castle_0";

        public WCCW(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var cornfieldPart0 = new CornfieldPart(cornfieldPart0Name, Id);
            Parts.Add(cornfieldPart0);

            var cornfieldPart1 = new CornfieldPart(cornfieldPart1Name, Id);
            Parts.Add(cornfieldPart1);

            var castlePart1 = new CastlePart(castlePart1Name, Id);
            Parts.Add(castlePart1);

            FieldToCastleParts.Add(cornfieldPart0.PartId, new List<string>() { castlePart1.PartId });
        }

        public override void ConnectField(Field field, FieldBoard fieldBoard)
        {
            AddBorderToPart(field, FieldSide.right, GetPart(castlePart1Name), fieldBoard);
            AddBorderToPart(field, FieldSide.bottom, GetPart(castlePart1Name), fieldBoard);

            AddCornfieldSplittedBorder(field, FieldSide.top, CornfieldSide.side_0, GetPart(cornfieldPart0Name), fieldBoard);
            AddCornfieldSplittedBorder(field, FieldSide.left, CornfieldSide.side_5, GetPart(cornfieldPart0Name), fieldBoard);

            AddCornfieldSplittedBorder(field, FieldSide.top, CornfieldSide.side_7, GetPart(cornfieldPart1Name), fieldBoard);
            AddCornfieldSplittedBorder(field, FieldSide.left, CornfieldSide.side_6, GetPart(cornfieldPart1Name), fieldBoard);
        }
    }
}