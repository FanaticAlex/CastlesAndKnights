using Carcassone.Core.Fields;

namespace Carcassone.Core.Cards
{

    /// <summary>
    ///       R
    ///   |   |   |
    /// F |   |   | F
    ///   |   |   |
    ///       R
    /// </summary>
    public class RFRF : Card
    {
        protected string roadPartName = "Road_0";
        protected string cornfieldPartName = "Cornfield_0";
        protected string cornfieldPart1Name = "Cornfield_1";

        public RFRF(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var roadPart1 = new RoadPart(roadPartName, CardId);
            Parts.Add(roadPart1);

            var cornfieldPart1 = new CornfieldPart(cornfieldPartName, CardId);
            Parts.Add(cornfieldPart1);

            var cornfieldPart2 = new CornfieldPart(cornfieldPart1Name, CardId);
            Parts.Add(cornfieldPart2);
        }

        public override void ConnectField(Field field, FieldBoard fieldBoard)
        {
            AddBorderToPart(field, FieldSide.top, GetPart(roadPartName), fieldBoard);
            AddBorderToPart(field, FieldSide.bottom, GetPart(roadPartName), fieldBoard);


            // поле 1
            var side12 = RotateSide(FieldSide.top, RotationsCount);
            var sidePart12 = RotateSidePart(CornfieldSide.side_0, RotationsCount);
            var cornfieldBorder12 = new Border(field, fieldBoard.GetNeighbour(field, side12), this);
            GetPart(cornfieldPartName).Borders.Add(cornfieldBorder12);
            cornfieldBorder12.CornfieldSide = sidePart12;

            var side2 = RotateSide(FieldSide.right, RotationsCount);
            var cornfieldBorder2 = new Border(field, fieldBoard.GetNeighbour(field, side2), this);
            GetPart(cornfieldPartName).Borders.Add(cornfieldBorder2);

            var side31 = RotateSide(FieldSide.bottom, RotationsCount);
            var sidePart31 = RotateSidePart(CornfieldSide.side_3, RotationsCount);
            var cornfieldBorder31 = new Border(field, fieldBoard.GetNeighbour(field, side31), this);
            GetPart(cornfieldPartName).Borders.Add(cornfieldBorder31);
            cornfieldBorder31.CornfieldSide = sidePart31;


            // поле 2
            var side32 = RotateSide(FieldSide.bottom, RotationsCount);
            var sidePart32 = RotateSidePart(CornfieldSide.side_4, RotationsCount);
            var cornfieldBorder32 = new Border(field, fieldBoard.GetNeighbour(field, side32), this);
            GetPart(cornfieldPart1Name).Borders.Add(cornfieldBorder32);
            cornfieldBorder32.CornfieldSide = sidePart32;

            var side4 = RotateSide(FieldSide.left, RotationsCount);
            var cornfieldBorder4 = new Border(field, fieldBoard.GetNeighbour(field, side4), this);
            GetPart(cornfieldPart1Name).Borders.Add(cornfieldBorder4);

            var side11 = RotateSide(FieldSide.top, RotationsCount);
            var sidePart11 = RotateSidePart(CornfieldSide.side_7, RotationsCount);
            var cornfieldBorder11 = new Border(field, fieldBoard.GetNeighbour(field, side11), this);
            GetPart(cornfieldPart1Name).Borders.Add(cornfieldBorder11);
            cornfieldBorder11.CornfieldSide = sidePart11;
        }
    }
}