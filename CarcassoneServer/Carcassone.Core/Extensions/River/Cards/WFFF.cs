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
            // поле
            var cornfield1Side0 = FieldSide.top;
            cornfield1Side0 = RotateSide(cornfield1Side0, RotationsCount);
            var cornfield1sidePart0 = CornfieldSide.side_0;
            cornfield1sidePart0 = RotateSidePart(cornfield1sidePart0, RotationsCount);
            var cornfield1Border0 = new Border(field, fieldBoard.GetNeighbour(field, cornfield1Side0), this);
            cornfield1Border0.CornfieldSide = cornfield1sidePart0;
            GetPart(cornfieldPartName).Borders.Add(cornfield1Border0);

            var cornfield1Side1 = FieldSide.top;
            cornfield1Side1 = RotateSide(cornfield1Side1, RotationsCount);
            var cornfield1sidePart1 = CornfieldSide.side_7;
            cornfield1sidePart1 = RotateSidePart(cornfield1sidePart1, RotationsCount);
            var cornfield1Border1 = new Border(field, fieldBoard.GetNeighbour(field, cornfield1Side1), this);
            cornfield1Border1.CornfieldSide = cornfield1sidePart1;
            GetPart(cornfieldPartName).Borders.Add(cornfield1Border1);

            AddBorderToPart(field, FieldSide.right, GetPart(cornfieldPartName), fieldBoard);
            AddBorderToPart(field, FieldSide.bottom, GetPart(cornfieldPartName), fieldBoard);
            AddBorderToPart(field, FieldSide.left, GetPart(cornfieldPartName), fieldBoard);
        }
    }
}