using Carcassone.Core.Cards;
using Carcassone.Core.Fields;

namespace Carcassone.Core.Extensions.River.Cards
{
    /// <summary>
    ///       F
    ///   |       |
    /// F |   ++++| W
    ///   |   +   |
    ///       W
    /// </summary>
    public class FWWF : Card
    {
        protected string cornfieldPart0Name = "Cornfield_0";
        protected string cornfieldPart1Name = "Cornfield_1";

        public FWWF(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var cornfieldPart1 = new CornfieldPart(cornfieldPart0Name, Id);
            Parts.Add(cornfieldPart1);

            var cornfieldPart2 = new CornfieldPart(cornfieldPart1Name, Id);
            Parts.Add(cornfieldPart2);
        }

        public override void ConnectField(Field field, FieldBoard fieldBoard)
        {
            AddBorderToPart(field, FieldSide.top, GetPart(cornfieldPart0Name), fieldBoard);
            AddCornfieldSplittedBorder(field, FieldSide.right, CornfieldSide.side_1, GetPart(cornfieldPart0Name), fieldBoard);
            AddCornfieldSplittedBorder(field, FieldSide.bottom, CornfieldSide.side_4, GetPart(cornfieldPart0Name), fieldBoard);
            AddBorderToPart(field, FieldSide.left, GetPart(cornfieldPart0Name), fieldBoard);

            AddCornfieldSplittedBorder(field, FieldSide.right, CornfieldSide.side_2, GetPart(cornfieldPart1Name), fieldBoard);
            AddCornfieldSplittedBorder(field, FieldSide.bottom, CornfieldSide.side_3, GetPart(cornfieldPart1Name), fieldBoard);
        }
    }
}