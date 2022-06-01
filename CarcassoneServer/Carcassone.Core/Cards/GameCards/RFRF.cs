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
        private RoadPart _roadPart1;
        private CornfieldPart _cornfieldPart1;
        private CornfieldPart _cornfieldPart2;

        public RFRF(string cardName) : base(cardName)
        {
            _roadPart1 = new RoadPart("Road_0", cardName);
            Parts.Add(_roadPart1);

            _cornfieldPart1 = new CornfieldPart("Cornfield_0", cardName);
            Parts.Add(_cornfieldPart1);

            _cornfieldPart2 = new CornfieldPart("Cornfield_1", cardName);
            Parts.Add(_cornfieldPart2);
        }

        public override void ConnectField(Field field)
        {
            base.ConnectField(field);

            // дорога
            var side1 = RotateSide(Side.top, RotationsCount);
            var roadBorder1 = new Border(this.Field, this.Field.GetNeighbour(side1), this);
            _roadPart1.Borders.Add(roadBorder1);

            var side3 = RotateSide(Side.bottom, RotationsCount);
            var roadBorder3 = new Border(this.Field, this.Field.GetNeighbour(side3), this);
            _roadPart1.Borders.Add(roadBorder3);


            // поле 1
            var side12 = RotateSide(Side.top, RotationsCount);
            var sidePart12 = RotateSidePart(CornfieldSide.side_0, RotationsCount);
            var cornfieldBorder12 = new Border(this.Field, this.Field.GetNeighbour(side12), this);
            _cornfieldPart1.Borders.Add(cornfieldBorder12);
            cornfieldBorder12.cornfieldSide = sidePart12;

            var side2 = RotateSide(Side.right, RotationsCount);
            var cornfieldBorder2 = new Border(this.Field, this.Field.GetNeighbour(side2), this);
            _cornfieldPart1.Borders.Add(cornfieldBorder2);

            var side31 = RotateSide(Side.bottom, RotationsCount);
            var sidePart31 = RotateSidePart(CornfieldSide.side_3, RotationsCount);
            var cornfieldBorder31 = new Border(this.Field, this.Field.GetNeighbour(side31), this);
            _cornfieldPart1.Borders.Add(cornfieldBorder31);
            cornfieldBorder31.cornfieldSide = sidePart31;


            // поле 2
            var side32 = RotateSide(Side.bottom, RotationsCount);
            var sidePart32 = RotateSidePart(CornfieldSide.side_4, RotationsCount);
            var cornfieldBorder32 = new Border(this.Field, this.Field.GetNeighbour(side32), this);
            _cornfieldPart2.Borders.Add(cornfieldBorder32);
            cornfieldBorder32.cornfieldSide = sidePart32;

            var side4 = RotateSide(Side.left, RotationsCount);
            var cornfieldBorder4 = new Border(this.Field, this.Field.GetNeighbour(side4), this);
            _cornfieldPart2.Borders.Add(cornfieldBorder4);

            var side11 = RotateSide(Side.top, RotationsCount);
            var sidePart11 = RotateSidePart(CornfieldSide.side_7, RotationsCount);
            var cornfieldBorder11 = new Border(this.Field, this.Field.GetNeighbour(side11), this);
            _cornfieldPart2.Borders.Add(cornfieldBorder11);
            cornfieldBorder11.cornfieldSide = sidePart11;
        }
    }
}