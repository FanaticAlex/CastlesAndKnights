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
            var cornfieldPart = new CornfieldPart(cornfieldPartName, CardId);
            Parts.Add(cornfieldPart);
        }

        public override void ConnectField(Field field, FieldBoard fieldBoard)
        {
            AddBorderToPart(field, Side.left, GetPart(cornfieldPartName), fieldBoard);
            AddBorderToPart(field, Side.top, GetPart(cornfieldPartName), fieldBoard);
            AddBorderToPart(field, Side.right, GetPart(cornfieldPartName), fieldBoard);


            var cornfield1Side0 = Side.bottom;
            cornfield1Side0 = RotateSide(cornfield1Side0, RotationsCount);
            var cornfield1sidePart0 = CornfieldSide.side_3;
            cornfield1sidePart0 = RotateSidePart(cornfield1sidePart0, RotationsCount);
            var cornfield1Border0 = new Border(field, fieldBoard.GetNeighbour(field, cornfield1Side0), this);
            cornfield1Border0.CornfieldSide = cornfield1sidePart0;
            GetPart(cornfieldPartName).Borders.Add(cornfield1Border0);

            var cornfield1Side1 = Side.bottom;
            cornfield1Side1 = RotateSide(cornfield1Side1, RotationsCount);
            var cornfield1sidePart1 = CornfieldSide.side_4;
            cornfield1sidePart1 = RotateSidePart(cornfield1sidePart1, RotationsCount);
            var cornfield1Border1 = new Border(field, fieldBoard.GetNeighbour(field, cornfield1Side1), this);
            cornfield1Border1.CornfieldSide = cornfield1sidePart1;
            GetPart(cornfieldPartName).Borders.Add(cornfield1Border1);
        }
    }
}