using Carcassone.Core.Cards;
using Carcassone.Core.Fields;

namespace Carcassone.Core.Extensions.River.Cards
{
    /// <summary>
    ///       W
    ///   |   +   |
    /// F |   +   | F
    ///   |       |
    ///       F
    /// </summary>
    public class WFFF : Card
    {
        protected string cornfieldPartName = "Cornfield_0";

        public WFFF(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var cornfieldPart1 = new CornfieldPart(cornfieldPartName, Id);
            Parts.Add(cornfieldPart1);
        }

        public override void ConnectField(Field field, FieldBoard fieldBoard)
        {
            AddCornfieldSplittedBorder(field, FieldSide.top, CornfieldSide.side_0, GetPart(cornfieldPartName), fieldBoard);
            AddCornfieldSplittedBorder(field, FieldSide.top, CornfieldSide.side_7, GetPart(cornfieldPartName), fieldBoard);
            AddBorderToPart(field, FieldSide.right, GetPart(cornfieldPartName), fieldBoard);
            AddBorderToPart(field, FieldSide.bottom, GetPart(cornfieldPartName), fieldBoard);
            AddBorderToPart(field, FieldSide.left, GetPart(cornfieldPartName), fieldBoard);
        }
    }
}