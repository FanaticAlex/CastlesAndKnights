using Carcassone.Core.Cards;
using Carcassone.Core.Fields;

namespace Carcassone.Core.Extensions.River.Cards
{

    /// <summary>
    ///       F
    ///   |       |
    /// F |   +   | F
    ///   |   +   |
    ///       W
    /// </summary>
    public class FFWF : Card
    {
        protected string cornfieldPartName = "Cornfield_0";

        public FFWF(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var cornfieldPart = new CornfieldPart(cornfieldPartName, Id);
            Parts.Add(cornfieldPart);
        }

        public override void ConnectField(Field field, FieldBoard fieldBoard)
        {
            AddBorderToPart(field, FieldSide.left, GetPart(cornfieldPartName), fieldBoard);
            AddBorderToPart(field, FieldSide.top, GetPart(cornfieldPartName), fieldBoard);
            AddBorderToPart(field, FieldSide.right, GetPart(cornfieldPartName), fieldBoard);
            AddCornfieldSplittedBorder(field, FieldSide.bottom, CornfieldSide.side_3, GetPart(cornfieldPartName), fieldBoard);
            AddCornfieldSplittedBorder(field, FieldSide.bottom, CornfieldSide.side_4, GetPart(cornfieldPartName), fieldBoard);
        }
    }
}