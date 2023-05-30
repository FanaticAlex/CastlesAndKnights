using Carcassone.Core.Cards;
using Carcassone.Core.Fields;

namespace Carcassone.Core.Extensions.River.Cards
{
    /// <summary>
    ///       W
    ///   |   +   |
    /// F |   +   | F
    ///   |   +   |
    ///       W
    /// </summary>
    public class WFWF : Card
    {
        protected string cornfieldPart0Name = "Cornfield_0";
        protected string cornfieldPart1Name = "Cornfield_1";

        public WFWF(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var cornfieldPart0 = new CornfieldPart(cornfieldPart0Name, Id);
            Parts.Add(cornfieldPart0);

            var cornfieldPart1 = new CornfieldPart(cornfieldPart1Name, Id);
            Parts.Add(cornfieldPart1);
        }

        public override void ConnectField(Field field, FieldBoard fieldBoard)
        {
            AddCornfieldSplittedBorder(field, FieldSide.top, CornfieldSide.side_0, GetPart(cornfieldPart0Name), fieldBoard);
            AddBorderToPart(field, FieldSide.right, GetPart(cornfieldPart0Name), fieldBoard);
            AddCornfieldSplittedBorder(field, FieldSide.bottom, CornfieldSide.side_3, GetPart(cornfieldPart0Name), fieldBoard);

            AddCornfieldSplittedBorder(field, FieldSide.bottom, CornfieldSide.side_4, GetPart(cornfieldPart1Name), fieldBoard);
            AddBorderToPart(field, FieldSide.left, GetPart(cornfieldPart1Name), fieldBoard);
            AddCornfieldSplittedBorder(field, FieldSide.top, CornfieldSide.side_7, GetPart(cornfieldPart1Name), fieldBoard);
        }
    }
}