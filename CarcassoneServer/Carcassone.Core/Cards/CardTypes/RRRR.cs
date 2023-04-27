using Carcassone.Core.Fields;

namespace Carcassone.Core.Cards
{

    /// <summary>
    ///       R
    ///   |   |   |
    /// R |---|---| R
    ///   |   |   |
    ///       R
    /// </summary>
    public class RRRR : Card
    {
        protected string roadPartName = "Road_0";
        protected string roadPart1Name = "Road_1";
        protected string roadPart2Name = "Road_2";
        protected string roadPart3Name = "Road_3";
        protected string cornfieldPartName = "Cornfield_0";
        protected string cornfieldPart1Name = "Cornfield_1";
        protected string cornfieldPart2Name = "Cornfield_2";
        protected string cornfieldPart3Name = "Cornfield_3";

        public RRRR(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var roadPart1 = new RoadPart(roadPartName, Id);
            Parts.Add(roadPart1);

            var roadPart2 = new RoadPart(roadPart1Name, Id);
            Parts.Add(roadPart2);

            var roadPart3 = new RoadPart(roadPart2Name, Id);
            Parts.Add(roadPart3);

            var roadPart4 = new RoadPart(roadPart3Name, Id);
            Parts.Add(roadPart4);

            var cornfieldPart1 = new CornfieldPart(cornfieldPartName, Id);
            Parts.Add(cornfieldPart1);

            var cornfieldPart2 = new CornfieldPart(cornfieldPart1Name, Id);
            Parts.Add(cornfieldPart2);

            var cornfieldPart3 = new CornfieldPart(cornfieldPart2Name, Id);
            Parts.Add(cornfieldPart3);

            var cornfieldPart4 = new CornfieldPart(cornfieldPart3Name, Id);
            Parts.Add(cornfieldPart4);
        }

        public override void ConnectField(Field field, FieldBoard fieldBoard)
        {
            AddBorderToPart(field, FieldSide.top, GetPart(roadPartName), fieldBoard);

            AddBorderToPart(field, FieldSide.right, GetPart(roadPart1Name), fieldBoard);

            AddBorderToPart(field, FieldSide.bottom, GetPart(roadPart2Name), fieldBoard);

            AddBorderToPart(field, FieldSide.left, GetPart(roadPart3Name), fieldBoard);


            // поле 1
            var side12 = RotateSide(FieldSide.top, RotationsCount);
            var sidePart12 = RotateSidePart(CornfieldSide.side_0, RotationsCount);
            var cornfieldBorder12 = new Border(field, fieldBoard.GetNeighbour(field, side12), this);
            GetPart(cornfieldPartName).Borders.Add(cornfieldBorder12);
            cornfieldBorder12.CornfieldSide = sidePart12;

            var side21 = RotateSide(FieldSide.right, RotationsCount);
            var sidePart21 = RotateSidePart(CornfieldSide.side_1, RotationsCount);
            var cornfieldBorder21 = new Border(field, fieldBoard.GetNeighbour(field, side21), this);
            GetPart(cornfieldPartName).Borders.Add(cornfieldBorder21);
            cornfieldBorder21.CornfieldSide = sidePart21;


            // поле 2
            var side22 = RotateSide(FieldSide.right, RotationsCount);
            var sidePart22 = RotateSidePart(CornfieldSide.side_2, RotationsCount);
            var cornfieldBorder22 = new Border(field, fieldBoard.GetNeighbour(field, side22), this);
            GetPart(cornfieldPart1Name).Borders.Add(cornfieldBorder22);
            cornfieldBorder22.CornfieldSide = sidePart22;

            var side31 = RotateSide(FieldSide.bottom, RotationsCount);
            var sidePart31 = RotateSidePart(CornfieldSide.side_3, RotationsCount);
            var cornfieldBorder31 = new Border(field, fieldBoard.GetNeighbour(field, side31), this);
            GetPart(cornfieldPart1Name).Borders.Add(cornfieldBorder31);
            cornfieldBorder31.CornfieldSide = sidePart31;


            // поле 3
            var side32 = RotateSide(FieldSide.right, RotationsCount);
            var sidePart32 = RotateSidePart(CornfieldSide.side_4, RotationsCount);
            var cornfieldBorder32 = new Border(field, fieldBoard.GetNeighbour(field, side32), this);
            GetPart(cornfieldPart2Name).Borders.Add(cornfieldBorder32);
            cornfieldBorder32.CornfieldSide = sidePart32;

            var side41 = RotateSide(FieldSide.bottom, RotationsCount);
            var sidePart41 = RotateSidePart(CornfieldSide.side_5, RotationsCount);
            var cornfieldBorder41 = new Border(field, fieldBoard.GetNeighbour(field, side41), this);
            GetPart(cornfieldPart2Name).Borders.Add(cornfieldBorder41);
            cornfieldBorder41.CornfieldSide = sidePart41;


            // поле 3
            var side42 = RotateSide(FieldSide.left, RotationsCount);
            var sidePart42 = RotateSidePart(CornfieldSide.side_6, RotationsCount);
            var cornfieldBorder42 = new Border(field, fieldBoard.GetNeighbour(field, side42), this);
            GetPart(cornfieldPart3Name).Borders.Add(cornfieldBorder42);
            cornfieldBorder42.CornfieldSide = sidePart42;

            var side11 = RotateSide(FieldSide.top, RotationsCount);
            var sidePart11 = RotateSidePart(CornfieldSide.side_7, RotationsCount);
            var cornfieldBorder11 = new Border(field, fieldBoard.GetNeighbour(field, side11), this);
            GetPart(cornfieldPart3Name).Borders.Add(cornfieldBorder11);
            cornfieldBorder11.CornfieldSide = sidePart11;
        }
    }
}