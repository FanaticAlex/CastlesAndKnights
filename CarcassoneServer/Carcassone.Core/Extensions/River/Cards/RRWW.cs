using Carcassone.Core.Cards;
using Carcassone.Core.Fields;

namespace Carcassone.Core.Extensions.River.Cards
{
    /// <summary>
    ///       R
    ///   |    \  |
    /// W |+++  \_| R
    ///   |   +   |
    ///       W
    /// </summary>
    public class RRWW : Card
    {
        protected string cornfieldPart0Name = "Cornfield_0";
        protected string cornfieldPart1Name = "Cornfield_1";
        protected string cornfieldPart2Name = "Cornfield_2";
        protected string roadPartName = "Road_0";

        public RRWW(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var cornfieldPart0 = new CornfieldPart(cornfieldPart0Name, CardId);
            Parts.Add(cornfieldPart0);

            var cornfieldPart1 = new CornfieldPart(cornfieldPart1Name, CardId);
            Parts.Add(cornfieldPart1);

            var cornfieldPart2 = new CornfieldPart(cornfieldPart2Name, CardId);
            Parts.Add(cornfieldPart2);

            var roadPart = new RoadPart(roadPartName, CardId);
            Parts.Add(roadPart);
        }

        public override void ConnectField(Field field, FieldBoard fieldBoard)
        {
            // поле 1
            var cornfield1Side0 = FieldSide.top;
            cornfield1Side0 = RotateSide(cornfield1Side0, RotationsCount);
            var cornfield1sidePart0 = CornfieldSide.side_0;
            cornfield1sidePart0 = RotateSidePart(cornfield1sidePart0, RotationsCount);
            var cornfield1Border0 = new Border(field, fieldBoard.GetNeighbour(field, cornfield1Side0), this);
            cornfield1Border0.CornfieldSide = cornfield1sidePart0;
            GetPart(cornfieldPart0Name).Borders.Add(cornfield1Border0);

            var cornfield1Side1 = FieldSide.right;
            cornfield1Side1 = RotateSide(cornfield1Side1, RotationsCount);
            var cornfield1sidePart1 = CornfieldSide.side_1;
            cornfield1sidePart1 = RotateSidePart(cornfield1sidePart1, RotationsCount);
            var cornfield1Border1 = new Border(field, fieldBoard.GetNeighbour(field, cornfield1Side1), this);
            cornfield1Border1.CornfieldSide = cornfield1sidePart1;
            GetPart(cornfieldPart0Name).Borders.Add(cornfield1Border1);


            // поле 2
            var cornfield2Side1 = FieldSide.right;
            cornfield2Side1 = RotateSide(cornfield2Side1, RotationsCount);
            var cornfield2sidePart1 = CornfieldSide.side_2;
            cornfield2sidePart1 = RotateSidePart(cornfield2sidePart1, RotationsCount);
            var cornfield2Border1 = new Border(field, fieldBoard.GetNeighbour(field, cornfield2Side1), this);
            cornfield2Border1.CornfieldSide = cornfield2sidePart1;
            GetPart(cornfieldPart1Name).Borders.Add(cornfield2Border1);

            var cornfield2Side2 = FieldSide.bottom;
            cornfield2Side2 = RotateSide(cornfield2Side2, RotationsCount);
            var cornfield2SidePart2 = CornfieldSide.side_3;
            cornfield2SidePart2 = RotateSidePart(cornfield2SidePart2, RotationsCount);
            var cornfield2Border2 = new Border(field, fieldBoard.GetNeighbour(field, cornfield2Side2), this);
            cornfield2Border2.CornfieldSide = cornfield2SidePart2;
            GetPart(cornfieldPart1Name).Borders.Add(cornfield2Border2);

            var cornfield2Side3 = FieldSide.left;
            cornfield2Side3 = RotateSide(cornfield2Side3, RotationsCount);
            var cornfield2sidePart3 = CornfieldSide.side_6;
            cornfield2sidePart3 = RotateSidePart(cornfield2sidePart3, RotationsCount);
            var cornfield2Border3 = new Border(field, fieldBoard.GetNeighbour(field, cornfield2Side3), this);
            cornfield2Border3.CornfieldSide = cornfield2sidePart3;
            GetPart(cornfieldPart1Name).Borders.Add(cornfield2Border3);

            var cornfield2Side4 = FieldSide.top;
            cornfield2Side4 = RotateSide(cornfield2Side4, RotationsCount);
            var cornfield2SidePart4 = CornfieldSide.side_7;
            cornfield2SidePart4 = RotateSidePart(cornfield2SidePart4, RotationsCount);
            var cornfield2Border4 = new Border(field, fieldBoard.GetNeighbour(field, cornfield2Side4), this);
            cornfield2Border4.CornfieldSide = cornfield2SidePart4;
            GetPart(cornfieldPart1Name).Borders.Add(cornfield2Border4);


            // поле 3
            var cornfield3Side0 = FieldSide.bottom;
            cornfield3Side0 = RotateSide(cornfield3Side0, RotationsCount);
            var cornfield3sidePart0 = CornfieldSide.side_4;
            cornfield3sidePart0 = RotateSidePart(cornfield3sidePart0, RotationsCount);
            var cornfield3Border0 = new Border(field, fieldBoard.GetNeighbour(field, cornfield3Side0), this);
            cornfield3Border0.CornfieldSide = cornfield3sidePart0;
            GetPart(cornfieldPart2Name).Borders.Add(cornfield3Border0);

            var cornfield3Side1 = FieldSide.left;
            cornfield3Side1 = RotateSide(cornfield3Side1, RotationsCount);
            var cornfield3sidePart1 = CornfieldSide.side_5;
            cornfield3sidePart1 = RotateSidePart(cornfield3sidePart1, RotationsCount);
            var cornfield3Border1 = new Border(field, fieldBoard.GetNeighbour(field, cornfield3Side1), this);
            cornfield3Border1.CornfieldSide = cornfield3sidePart1;
            GetPart(cornfieldPart2Name).Borders.Add(cornfield3Border1);

            // дорога
            AddBorderToPart(field, FieldSide.top, GetPart(roadPartName), fieldBoard);
            AddBorderToPart(field, FieldSide.right, GetPart(roadPartName), fieldBoard);
        }
    }
}