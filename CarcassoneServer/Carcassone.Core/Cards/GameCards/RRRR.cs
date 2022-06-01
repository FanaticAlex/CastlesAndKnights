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
        private RoadPart _roadPart1;
        private RoadPart _roadPart2;
        private RoadPart _roadPart3;
        private RoadPart _roadPart4;
        private CornfieldPart _cornfieldPart1;
        private CornfieldPart _cornfieldPart2;
        private CornfieldPart _cornfieldPart3;
        private CornfieldPart _cornfieldPart4;

        public RRRR(string cardName) : base(cardName)
        {
            _roadPart1 = new RoadPart("Road_0", cardName);
            Parts.Add(_roadPart1);

            _roadPart2 = new RoadPart("Road_1", cardName);
            Parts.Add(_roadPart2);

            _roadPart3 = new RoadPart("Road_2", cardName);
            Parts.Add(_roadPart3);

            _roadPart4 = new RoadPart("Road_3", cardName);
            Parts.Add(_roadPart4);

            _cornfieldPart1 = new CornfieldPart("Cornfield_0", cardName);
            Parts.Add(_cornfieldPart1);

            _cornfieldPart2 = new CornfieldPart("Cornfield_1", cardName);
            Parts.Add(_cornfieldPart2);

            _cornfieldPart3 = new CornfieldPart("Cornfield_2", cardName);
            Parts.Add(_cornfieldPart3);

            _cornfieldPart4 = new CornfieldPart("Cornfield_3", cardName);
            Parts.Add(_cornfieldPart4);
        }

        public override void ConnectField(Field field)
        {
            base.ConnectField(field);

            // дорога
            var side1 = RotateSide(Side.top, RotationsCount);
            var roadBorder1 = new Border(this.Field, this.Field.GetNeighbour(side1), this);
            _roadPart1.Borders.Add(roadBorder1);


            // дорога
            var side2 = RotateSide(Side.right, RotationsCount);
            var roadBorder2 = new Border(this.Field, this.Field.GetNeighbour(side2), this);
            _roadPart2.Borders.Add(roadBorder2);


            // дорога
            var side3 = RotateSide(Side.bottom, RotationsCount);
            var roadBorder3 = new Border(this.Field, this.Field.GetNeighbour(side3), this);
            _roadPart3.Borders.Add(roadBorder3);


            // дорога
            var side4 = RotateSide(Side.left, RotationsCount);
            var roadBorder4 = new Border(this.Field, this.Field.GetNeighbour(side4), this);
            _roadPart4.Borders.Add(roadBorder4);


            // поле 1
            var side12 = RotateSide(Side.top, RotationsCount);
            var sidePart12 = RotateSidePart(CornfieldSide.side_0, RotationsCount);
            var cornfieldBorder12 = new Border(this.Field, this.Field.GetNeighbour(side12), this);
            _cornfieldPart1.Borders.Add(cornfieldBorder12);
            cornfieldBorder12.cornfieldSide = sidePart12;

            var side21 = RotateSide(Side.right, RotationsCount);
            var sidePart21 = RotateSidePart(CornfieldSide.side_1, RotationsCount);
            var cornfieldBorder21 = new Border(this.Field, this.Field.GetNeighbour(side21), this);
            _cornfieldPart1.Borders.Add(cornfieldBorder21);
            cornfieldBorder21.cornfieldSide = sidePart21;


            // поле 2
            var side22 = RotateSide(Side.right, RotationsCount);
            var sidePart22 = RotateSidePart(CornfieldSide.side_2, RotationsCount);
            var cornfieldBorder22 = new Border(this.Field, this.Field.GetNeighbour(side22), this);
            _cornfieldPart2.Borders.Add(cornfieldBorder22);
            cornfieldBorder22.cornfieldSide = sidePart22;

            var side31 = RotateSide(Side.bottom, RotationsCount);
            var sidePart31 = RotateSidePart(CornfieldSide.side_3, RotationsCount);
            var cornfieldBorder31 = new Border(this.Field, this.Field.GetNeighbour(side31), this);
            _cornfieldPart2.Borders.Add(cornfieldBorder31);
            cornfieldBorder31.cornfieldSide = sidePart31;


            // поле 3
            var side32 = RotateSide(Side.right, RotationsCount);
            var sidePart32 = RotateSidePart(CornfieldSide.side_4, RotationsCount);
            var cornfieldBorder32 = new Border(this.Field, this.Field.GetNeighbour(side32), this);
            _cornfieldPart3.Borders.Add(cornfieldBorder32);
            cornfieldBorder32.cornfieldSide = sidePart32;

            var side41 = RotateSide(Side.bottom, RotationsCount);
            var sidePart41 = RotateSidePart(CornfieldSide.side_5, RotationsCount);
            var cornfieldBorder41 = new Border(this.Field, this.Field.GetNeighbour(side41), this);
            _cornfieldPart3.Borders.Add(cornfieldBorder41);
            cornfieldBorder41.cornfieldSide = sidePart41;


            // поле 3
            var side42 = RotateSide(Side.left, RotationsCount);
            var sidePart42 = RotateSidePart(CornfieldSide.side_6, RotationsCount);
            var cornfieldBorder42 = new Border(this.Field, this.Field.GetNeighbour(side42), this);
            _cornfieldPart4.Borders.Add(cornfieldBorder42);
            cornfieldBorder42.cornfieldSide = sidePart42;

            var side11 = RotateSide(Side.top, RotationsCount);
            var sidePart11 = RotateSidePart(CornfieldSide.side_7, RotationsCount);
            var cornfieldBorder11 = new Border(this.Field, this.Field.GetNeighbour(side11), this);
            _cornfieldPart4.Borders.Add(cornfieldBorder11);
            cornfieldBorder11.cornfieldSide = sidePart11;
        }
    }
}