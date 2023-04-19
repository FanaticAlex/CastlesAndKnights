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
        protected string cornfieldPart1Name = "Cornfield_0";
        protected string cornfieldPart2Name = "Cornfield_1";

        public WFWF(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var cornfieldPart1 = new CornfieldPart(cornfieldPart1Name, CardId);
            Parts.Add(cornfieldPart1);

            var cornfieldPart2 = new CornfieldPart(cornfieldPart2Name, CardId);
            Parts.Add(cornfieldPart2);
        }

        public override void ConnectField(Field field, FieldBoard fieldBoard)
        {
            // поле 1
            var cornfield1Side0 = Side.right;
            cornfield1Side0 = RotateSide(cornfield1Side0, RotationsCount);
            var cornfieldBorder0 = new Border(field, fieldBoard.GetNeighbour(field, cornfield1Side0), this);
            GetPart(cornfieldPart1Name).Borders.Add(cornfieldBorder0);

            var cornfield1Side1 = Side.top;
            cornfield1Side1 = RotateSide(cornfield1Side1, RotationsCount);
            var cornfield1sidePart1 = CornfieldSide.side_0;
            cornfield1sidePart1 = RotateSidePart(cornfield1sidePart1, RotationsCount);
            var cornfield1Border1 = new Border(field, fieldBoard.GetNeighbour(field, cornfield1Side1), this);
            cornfield1Border1.CornfieldSide = cornfield1sidePart1;
            GetPart(cornfieldPart1Name).Borders.Add(cornfield1Border1);

            var cornfield1Side2 = Side.bottom;
            cornfield1Side2 = RotateSide(cornfield1Side2, RotationsCount);
            var cornfield1sidePart2 = CornfieldSide.side_3;
            cornfield1sidePart2 = RotateSidePart(cornfield1sidePart2, RotationsCount);
            var cornfield1Border2 = new Border(field, fieldBoard.GetNeighbour(field, cornfield1Side2), this);
            cornfield1Border2.CornfieldSide = cornfield1sidePart2;
            GetPart(cornfieldPart1Name).Borders.Add(cornfield1Border2);


            // поле 2
            var cornfield2Side1 = Side.bottom;
            cornfield2Side1 = RotateSide(cornfield2Side1, RotationsCount);
            var cornfield2sidePart1 = CornfieldSide.side_4;
            cornfield2sidePart1 = RotateSidePart(cornfield2sidePart1, RotationsCount);
            var cornfield2Border1 = new Border(field, fieldBoard.GetNeighbour(field, cornfield2Side1), this);
            cornfield2Border1.CornfieldSide = cornfield2sidePart1;
            GetPart(cornfieldPart2Name).Borders.Add(cornfield2Border1);

            var cornfield2Side2 = Side.top;
            cornfield2Side2 = RotateSide(cornfield2Side2, RotationsCount);
            var cornfield2SidePart2 = CornfieldSide.side_7;
            cornfield2SidePart2 = RotateSidePart(cornfield2SidePart2, RotationsCount);
            var cornfield2Border2 = new Border(field, fieldBoard.GetNeighbour(field, cornfield2Side2), this);
            cornfield2Border2.CornfieldSide = cornfield2SidePart2;
            GetPart(cornfieldPart2Name).Borders.Add(cornfield2Border2);

            var cornfield2Side3 = Side.left;
            cornfield2Side3 = RotateSide(cornfield2Side3, RotationsCount);
            var cornfield2Border3 = new Border(field, fieldBoard.GetNeighbour(field, cornfield2Side3), this);
            GetPart(cornfieldPart2Name).Borders.Add(cornfield2Border3);
        }
    }
}