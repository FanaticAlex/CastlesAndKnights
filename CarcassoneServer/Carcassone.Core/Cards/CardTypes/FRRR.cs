using Carcassone.Core.Fields;

namespace Carcassone.Core.Cards
{

    /// <summary>
    ///       F
    ///   |       |
    /// R |_______| R
    ///   |   |   |
    ///       R
    /// </summary>
    public class FRRR : Card
    {
        protected string roadPartName = "Road_0";
        protected string roadPart1Name = "Road_1";
        protected string roadPart2Name = "Road_2";
        protected string cornfieldPartName = "Cornfield_0";
        protected string cornfieldPart1Name = "Cornfield_1";
        protected string cornfieldPart2Name = "Cornfield_2";

        public FRRR(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var roadPart1 = new RoadPart(roadPartName, CardId);
            Parts.Add(roadPart1);

            var roadPart3 = new RoadPart(roadPart1Name, CardId);
            Parts.Add(roadPart3);

            var roadPart4 = new RoadPart(roadPart2Name, CardId);
            Parts.Add(roadPart4);

            var cornfieldPart1 = new CornfieldPart(cornfieldPartName, CardId);
            Parts.Add(cornfieldPart1);

            var cornfieldPart2 = new CornfieldPart(cornfieldPart1Name, CardId);
            Parts.Add(cornfieldPart2);

            var cornfieldPart3 = new CornfieldPart(cornfieldPart2Name, CardId);
            Parts.Add(cornfieldPart3);
        }

        public override void ConnectField(Field field, FieldBoard fieldBoard)
        {
            AddBorderToPart(field, Side.right, GetPart(roadPartName), fieldBoard);

            AddBorderToPart(field, Side.bottom, GetPart(roadPart1Name), fieldBoard);

            AddBorderToPart(field, Side.left, GetPart(roadPart2Name), fieldBoard);


            // поле 1
            var side21 = RotateSide(Side.right, RotationsCount);
            var sidePart21 = RotateSidePart(CornfieldSide.side_1, RotationsCount);
            var cornfieldBorder21 = new Border(field, fieldBoard.GetNeighbour(field, side21), this);
            GetPart(cornfieldPartName).Borders.Add(cornfieldBorder21);
            cornfieldBorder21.CornfieldSide = sidePart21;

            var side1 = RotateSide(Side.top, RotationsCount);
            var cornfieldBorder1 = new Border(field, fieldBoard.GetNeighbour(field, side1), this);
            GetPart(cornfieldPartName).Borders.Add(cornfieldBorder1);

            var side42 = RotateSide(Side.left, RotationsCount);
            var sidePart42 = RotateSidePart(CornfieldSide.side_6, RotationsCount);
            var cornfieldBorder42 = new Border(field, fieldBoard.GetNeighbour(field, side42), this);
            GetPart(cornfieldPartName).Borders.Add(cornfieldBorder42);
            cornfieldBorder42.CornfieldSide = sidePart42;


            // поле 2
            var side22 = RotateSide(Side.right, RotationsCount);
            var sidePart22 = RotateSidePart(CornfieldSide.side_2, RotationsCount);
            var cornfieldBorder22 = new Border(field, fieldBoard.GetNeighbour(field, side22), this);
            GetPart(cornfieldPart1Name).Borders.Add(cornfieldBorder22);
            cornfieldBorder22.CornfieldSide = sidePart22;

            var side31 = RotateSide(Side.bottom, RotationsCount);
            var sidePart31 = RotateSidePart(CornfieldSide.side_3, RotationsCount);
            var cornfieldBorder31 = new Border(field, fieldBoard.GetNeighbour(field, side31), this);
            GetPart(cornfieldPart1Name).Borders.Add(cornfieldBorder31);
            cornfieldBorder31.CornfieldSide = sidePart31;


            // поле 3
            var side32 = RotateSide(Side.bottom, RotationsCount);
            var sidePart32 = RotateSidePart(CornfieldSide.side_4, RotationsCount);
            var cornfieldBorder32 = new Border(field, fieldBoard.GetNeighbour(field, side32), this);
            GetPart(cornfieldPart2Name).Borders.Add(cornfieldBorder32);
            cornfieldBorder32.CornfieldSide = sidePart32;

            var side41 = RotateSide(Side.right, RotationsCount);
            var sidePart41 = RotateSidePart(CornfieldSide.side_5, RotationsCount);
            var cornfieldBorder41 = new Border(field, fieldBoard.GetNeighbour(field, side41), this);
            GetPart(cornfieldPart2Name).Borders.Add(cornfieldBorder41);
            cornfieldBorder41.CornfieldSide = sidePart41;
        }
    }
}