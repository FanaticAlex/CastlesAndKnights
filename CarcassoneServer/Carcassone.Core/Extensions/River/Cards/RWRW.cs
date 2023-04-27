using Carcassone.Core.Cards;
using Carcassone.Core.Fields;

namespace Carcassone.Core.Extensions.River.Cards
{
    /// <summary>
    ///       R
    ///   |   |   |
    /// W |+++++++| W
    ///   |   |   |
    ///       R
    /// </summary>
    public class RWRW : Card
    {
        protected string cornfieldPartName = "Cornfield_0";
        protected string cornfieldPart1Name = "Cornfield_1";
        protected string roadPartName = "Road_0";
        protected string cornfieldPart2Name = "Cornfield_2";
        protected string cornfieldPart3Name = "Cornfield_3";

        public RWRW(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var cornfieldPart1 = new CornfieldPart(cornfieldPartName, CardId);
            Parts.Add(cornfieldPart1);

            var cornfieldPart2 = new CornfieldPart(cornfieldPart1Name, CardId);
            Parts.Add(cornfieldPart2);

            var cornfieldPart3 = new CornfieldPart(cornfieldPart2Name, CardId);
            Parts.Add(cornfieldPart3);

            var cornfieldPart4 = new CornfieldPart(cornfieldPart3Name, CardId);
            Parts.Add(cornfieldPart4);

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
            GetPart(cornfieldPartName).Borders.Add(cornfield1Border0);

            var cornfield1Side1 = FieldSide.right;
            cornfield1Side1 = RotateSide(cornfield1Side1, RotationsCount);
            var cornfield1sidePart1 = CornfieldSide.side_1;
            cornfield1sidePart1 = RotateSidePart(cornfield1sidePart1, RotationsCount);
            var cornfield1Border1 = new Border(field, fieldBoard.GetNeighbour(field, cornfield1Side1), this);
            cornfield1Border1.CornfieldSide = cornfield1sidePart1;
            GetPart(cornfieldPartName).Borders.Add(cornfield1Border1);

            // поле 2
            var cornfield2Side0 = FieldSide.right;
            cornfield2Side0 = RotateSide(cornfield2Side0, RotationsCount);
            var cornfield2sidePart0 = CornfieldSide.side_2;
            cornfield2sidePart0 = RotateSidePart(cornfield2sidePart0, RotationsCount);
            var cornfield2Border0 = new Border(field, fieldBoard.GetNeighbour(field, cornfield2Side0), this);
            cornfield2Border0.CornfieldSide = cornfield2sidePart0;
            GetPart(cornfieldPart1Name).Borders.Add(cornfield2Border0);

            var cornfield2Side1 = FieldSide.bottom;
            cornfield2Side1 = RotateSide(cornfield2Side1, RotationsCount);
            var cornfield2sidePart1 = CornfieldSide.side_3;
            cornfield2sidePart1 = RotateSidePart(cornfield2sidePart1, RotationsCount);
            var cornfield2Border1 = new Border(field, fieldBoard.GetNeighbour(field, cornfield2Side1), this);
            cornfield2Border1.CornfieldSide = cornfield2sidePart1;
            GetPart(cornfieldPart1Name).Borders.Add(cornfield2Border1);


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
            var cornfield3SidePart1 = CornfieldSide.side_5;
            cornfield3SidePart1 = RotateSidePart(cornfield3SidePart1, RotationsCount);
            var cornfield3Border1 = new Border(field, fieldBoard.GetNeighbour(field, cornfield3Side1), this);
            cornfield3Border1.CornfieldSide = cornfield3SidePart1;
            GetPart(cornfieldPart2Name).Borders.Add(cornfield3Border1);

            // поле 4
            var cornfield4Side0 = FieldSide.left;
            cornfield4Side0 = RotateSide(cornfield4Side0, RotationsCount);
            var cornfield4sidePart0 = CornfieldSide.side_6;
            cornfield4sidePart0 = RotateSidePart(cornfield4sidePart0, RotationsCount);
            var cornfield4Border0 = new Border(field, fieldBoard.GetNeighbour(field, cornfield4Side0), this);
            cornfield4Border0.CornfieldSide = cornfield4sidePart0;
            GetPart(cornfieldPart3Name).Borders.Add(cornfield4Border0);

            var cornfield4Side1 = FieldSide.top;
            cornfield4Side1 = RotateSide(cornfield4Side1, RotationsCount);
            var cornfield4SidePart1 = CornfieldSide.side_7;
            cornfield4SidePart1 = RotateSidePart(cornfield4SidePart1, RotationsCount);
            var cornfield4Border1 = new Border(field, fieldBoard.GetNeighbour(field, cornfield4Side1), this);
            cornfield4Border1.CornfieldSide = cornfield4SidePart1;
            GetPart(cornfieldPart3Name).Borders.Add(cornfield4Border1);

            // дорога
            AddBorderToPart(field, FieldSide.top, GetPart(roadPartName), fieldBoard);
            AddBorderToPart(field, FieldSide.bottom, GetPart(roadPartName), fieldBoard);
        }
    }
}