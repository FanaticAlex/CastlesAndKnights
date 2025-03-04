using Carcassone.Core.Cards;
using Carcassone.Core.Fields;
using System.Collections.Generic;

namespace Carcassone.Core.Extensions.River.Cards
{

    /// <summary>
    ///       C
    ///   |\-----/|
    /// W |+++++++| W
    ///   |/-----\|
    ///       C
    /// </summary>
    public class CWCW : Card
    {
        protected string castlePart0Name = "Castle_0";
        protected string castlePart1Name = "Castle_1";
        protected string cornfieldPart0Name = "Cornfield_0";
        protected string cornfieldPart1Name = "Cornfield_1";

        public CWCW(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var castlePart0 = new CastlePart(castlePart0Name, Id);
            Parts.Add(castlePart0);

            var castlePart1 = new CastlePart(castlePart1Name, Id);
            Parts.Add(castlePart1);

            var cornfieldPart0 = new CornfieldPart(cornfieldPart0Name, Id);
            Parts.Add(cornfieldPart0);

            var cornfieldPart1 = new CornfieldPart(cornfieldPart1Name, Id);
            Parts.Add(cornfieldPart1);


            FieldToCastleParts.Add(cornfieldPart0.PartId, new List<string>() { castlePart0.PartId });
            FieldToCastleParts.Add(cornfieldPart1.PartId, new List<string>() { castlePart1.PartId });
        }

        public override void ConnectField(Field field, FieldBoard fieldBoard)
        {
            AddBorderToPart(field, FieldSide.top, GetPart(castlePart0Name), fieldBoard);

            AddBorderToPart(field, FieldSide.bottom, GetPart(castlePart1Name), fieldBoard);

            AddCornfieldSplittedBorder(field, FieldSide.right, CornfieldSide.side_1, GetPart(cornfieldPart0Name), fieldBoard);
            AddCornfieldSplittedBorder(field, FieldSide.left, CornfieldSide.side_6, GetPart(cornfieldPart0Name), fieldBoard);

            AddCornfieldSplittedBorder(field, FieldSide.right, CornfieldSide.side_2, GetPart(cornfieldPart1Name), fieldBoard);
            AddCornfieldSplittedBorder(field, FieldSide.left, CornfieldSide.side_5, GetPart(cornfieldPart1Name), fieldBoard);
        }
    }
}