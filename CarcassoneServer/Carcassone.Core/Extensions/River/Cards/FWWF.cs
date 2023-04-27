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
        protected string cornfieldPart1Name = "Cornfield_0";
        protected string cornfieldPart2Name = "Cornfield_1";

        public FWWF(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var cornfieldPart1 = new CornfieldPart(cornfieldPart1Name, Id);
            Parts.Add(cornfieldPart1);

            var cornfieldPart2 = new CornfieldPart(cornfieldPart2Name, Id);
            Parts.Add(cornfieldPart2);
        }

        public override void ConnectField(Field field, FieldBoard fieldBoard)
        {
            // поле 1
            var cornfield1Side0 = FieldSide.top;
            cornfield1Side0 = RotateSide(cornfield1Side0, RotationsCount);
            var cornfieldBorder0 = new Border(field, fieldBoard.GetNeighbour(field, cornfield1Side0), this);
            GetPart(cornfieldPart1Name).Borders.Add(cornfieldBorder0);

            var cornfield1Side1 = FieldSide.right;
            cornfield1Side1 = RotateSide(cornfield1Side1, RotationsCount);
            var cornfield1sidePart1 = CornfieldSide.side_1;
            cornfield1sidePart1 = RotateSidePart(cornfield1sidePart1, RotationsCount);
            var cornfield1Border1 = new Border(field, fieldBoard.GetNeighbour(field, cornfield1Side1), this);
            cornfield1Border1.CornfieldSide = cornfield1sidePart1;
            GetPart(cornfieldPart1Name).Borders.Add(cornfield1Border1);

            var cornfield1Side3 = FieldSide.bottom;
            cornfield1Side3 = RotateSide(cornfield1Side3, RotationsCount);
            var cornfield1sidePart3 = CornfieldSide.side_4;
            cornfield1sidePart3 = RotateSidePart(cornfield1sidePart3, RotationsCount);
            var cornfield1Border3 = new Border(field, fieldBoard.GetNeighbour(field, cornfield1Side3), this);
            cornfield1Border3.CornfieldSide = cornfield1sidePart3;
            GetPart(cornfieldPart1Name).Borders.Add(cornfield1Border3);

            var cornfield1Side2 = FieldSide.left;
            cornfield1Side2 = RotateSide(cornfield1Side2, RotationsCount);
            var cornfield1Border2 = new Border(field, fieldBoard.GetNeighbour(field, cornfield1Side2), this);
            GetPart(cornfieldPart1Name).Borders.Add(cornfield1Border2);


            // поле 2
            var cornfield2Side1 = FieldSide.right;
            cornfield2Side1 = RotateSide(cornfield2Side1, RotationsCount);
            var cornfield2sidePart1 = CornfieldSide.side_2;
            cornfield2sidePart1 = RotateSidePart(cornfield2sidePart1, RotationsCount);
            var cornfield2Border1 = new Border(field, fieldBoard.GetNeighbour(field, cornfield2Side1), this);
            cornfield2Border1.CornfieldSide = cornfield2sidePart1;
            GetPart(cornfieldPart2Name).Borders.Add(cornfield2Border1);

            var cornfield2Side2 = FieldSide.bottom;
            cornfield2Side2 = RotateSide(cornfield2Side2, RotationsCount);
            var cornfield2SidePart2 = CornfieldSide.side_3;
            cornfield2SidePart2 = RotateSidePart(cornfield2SidePart2, RotationsCount);
            var cornfield2Border2 = new Border(field, fieldBoard.GetNeighbour(field, cornfield2Side2), this);
            cornfield2Border2.CornfieldSide = cornfield2SidePart2;
            GetPart(cornfieldPart2Name).Borders.Add(cornfield2Border2);
        }
    }
}