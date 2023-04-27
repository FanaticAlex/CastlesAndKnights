using Carcassone.Core.Cards;
using Carcassone.Core.Fields;

namespace Carcassone.Core.Extensions.River.Cards
{

    /// <summary>
    ///       F
    ///   |       |
    /// W |+++O+++| W
    ///   |   |   |
    ///       R
    /// </summary>
    public class FWRW : Card
    {
        protected string churchPartName = "Church_0";
        protected string cornfieldPart1Name = "Cornfield_0";
        protected string cornfieldPart2Name = "Cornfield_1";
        protected string cornfieldPart3Name = "Cornfield_2";
        protected string roadPartName = "Road_0";

        public FWRW(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var churchPart = new ChurchPart(churchPartName, Id);
            Parts.Add(churchPart);

            var cornfieldPart1 = new CornfieldPart(cornfieldPart1Name, Id);
            Parts.Add(cornfieldPart1);

            var cornfieldPart2 = new CornfieldPart(cornfieldPart2Name, Id);
            Parts.Add(cornfieldPart2);

            var roadPart = new RoadPart(roadPartName, Id);
            Parts.Add(roadPart);

            var cornfieldPart3 = new CornfieldPart(cornfieldPart3Name, Id);
            Parts.Add(cornfieldPart3);
        }

        public override void ConnectField(Field field, FieldBoard fieldBoard)
        {
            ((ChurchPart)GetPart(churchPartName)).ChurchFieldId = field.Id;

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

            var cornfield1Side2 = FieldSide.left;
            cornfield1Side2 = RotateSide(cornfield1Side2, RotationsCount);
            var cornfield1SidePart2 = CornfieldSide.side_6;
            cornfield1SidePart2 = RotateSidePart(cornfield1SidePart2, RotationsCount);
            var cornfield1Border2 = new Border(field, fieldBoard.GetNeighbour(field, cornfield1Side2), this);
            cornfield1Border2.CornfieldSide = cornfield1SidePart2;
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

            // дорога
            AddBorderToPart(field, FieldSide.bottom, GetPart(roadPartName), fieldBoard);

            // поле 3
            var cornfield3Side1 = FieldSide.bottom;
            cornfield3Side1 = RotateSide(cornfield3Side1, RotationsCount);
            var cornfield3sidePart1 = CornfieldSide.side_4;
            cornfield3sidePart1 = RotateSidePart(cornfield3sidePart1, RotationsCount);
            var cornfield3Border1 = new Border(field, fieldBoard.GetNeighbour(field, cornfield3Side1), this);
            cornfield3Border1.CornfieldSide = cornfield3sidePart1;
            GetPart(cornfieldPart3Name).Borders.Add(cornfield3Border1);

            var cornfield3Side2 = FieldSide.left;
            cornfield3Side2 = RotateSide(cornfield3Side2, RotationsCount);
            var cornfield3SidePart2 = CornfieldSide.side_5;
            cornfield3SidePart2 = RotateSidePart(cornfield3SidePart2, RotationsCount);
            var cornfield3Border2 = new Border(field, fieldBoard.GetNeighbour(field, cornfield3Side2), this);
            cornfield3Border2.CornfieldSide = cornfield3SidePart2;
            GetPart(cornfieldPart3Name).Borders.Add(cornfield3Border2);
        }
    }
}