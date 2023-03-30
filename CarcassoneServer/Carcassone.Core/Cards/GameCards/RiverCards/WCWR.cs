using Carcassone.Core.Fields;

namespace Carcassone.Core.Cards
{
    /// <summary>
    ///       W
    ///   |   +  /|
    /// R |-----| | C
    ///   |   +  \|
    ///       W
    /// </summary>
    public class WCWR : Card
    {
        private CastlePart _castlePart0;
        private CornfieldPart _cornfieldPart0;
        private CornfieldPart _cornfieldPart1;
        private CornfieldPart _cornfieldPart2;
        private CornfieldPart _cornfieldPart3;
        private RoadPart _roadPart;

        public WCWR(string cardName) : base(cardName)
        {
            _castlePart0 = new CastlePart("Castle_0", cardName);
            Parts.Add(_castlePart0);

            _cornfieldPart0 = new CornfieldPart("Cornfield_0", cardName);
            Parts.Add(_cornfieldPart0);

            _cornfieldPart1 = new CornfieldPart("Cornfield_1", cardName);
            Parts.Add(_cornfieldPart1);

            _cornfieldPart2 = new CornfieldPart("Cornfield_2", cardName);
            Parts.Add(_cornfieldPart2);

            _cornfieldPart3 = new CornfieldPart("Cornfield_3", cardName);
            Parts.Add(_cornfieldPart3);

            _roadPart = new RoadPart("Road_0", cardName);
            Parts.Add(_roadPart);
        }

        public override void ConnectField(Field field)
        {
            Field = field;

            // замок
            AddBorderToPart(Side.right, _castlePart0);

            // поле 1
            var cornfield1Side0 = Side.top;
            cornfield1Side0 = RotateSide(cornfield1Side0, RotationsCount);
            var cornfield1sidePart0 = CornfieldSide.side_0;
            cornfield1sidePart0 = RotateSidePart(cornfield1sidePart0, RotationsCount);
            var cornfield1Border0 = new Border(this.Field, this.Field?.GetNeighbour(cornfield1Side0), this);
            cornfield1Border0.CornfieldSide = cornfield1sidePart0;
            _cornfieldPart0.Borders.Add(cornfield1Border0);

            // поле 2
            var cornfield2Side0 = Side.bottom;
            cornfield2Side0 = RotateSide(cornfield2Side0, RotationsCount);
            var cornfield2sidePart0 = CornfieldSide.side_3;
            cornfield2sidePart0 = RotateSidePart(cornfield2sidePart0, RotationsCount);
            var cornfield2Border0 = new Border(this.Field, this.Field?.GetNeighbour(cornfield2Side0), this);
            cornfield2Border0.CornfieldSide = cornfield2sidePart0;
            _cornfieldPart1.Borders.Add(cornfield2Border0);

            // поле 3
            var cornfield3Side0 = Side.bottom;
            cornfield3Side0 = RotateSide(cornfield3Side0, RotationsCount);
            var cornfield3sidePart0 = CornfieldSide.side_4;
            cornfield3sidePart0 = RotateSidePart(cornfield3sidePart0, RotationsCount);
            var cornfield3Border0 = new Border(this.Field, this.Field?.GetNeighbour(cornfield3Side0), this);
            cornfield3Border0.CornfieldSide = cornfield3sidePart0;
            _cornfieldPart2.Borders.Add(cornfield3Border0);

            var cornfield3Side1 = Side.left;
            cornfield3Side1 = RotateSide(cornfield3Side1, RotationsCount);
            var cornfield3sidePart1 = CornfieldSide.side_5;
            cornfield3sidePart1 = RotateSidePart(cornfield3sidePart1, RotationsCount);
            var cornfield3Border1 = new Border(this.Field, this.Field?.GetNeighbour(cornfield3Side1), this);
            cornfield3Border1.CornfieldSide = cornfield3sidePart1;
            _cornfieldPart2.Borders.Add(cornfield3Border1);

            // поле 4
            var cornfield4Side0 = Side.left;
            cornfield4Side0 = RotateSide(cornfield4Side0, RotationsCount);
            var cornfield4sidePart0 = CornfieldSide.side_6;
            cornfield4sidePart0 = RotateSidePart(cornfield4sidePart0, RotationsCount);
            var cornfield4Border0 = new Border(this.Field, this.Field?.GetNeighbour(cornfield4Side0), this);
            cornfield4Border0.CornfieldSide = cornfield4sidePart0;
            _cornfieldPart3.Borders.Add(cornfield4Border0);

            var cornfield4Side1 = Side.top;
            cornfield4Side1 = RotateSide(cornfield4Side1, RotationsCount);
            var cornfield4sidePart1 = CornfieldSide.side_7;
            cornfield4sidePart1 = RotateSidePart(cornfield4sidePart1, RotationsCount);
            var cornfield4Border1 = new Border(this.Field, this.Field?.GetNeighbour(cornfield4Side1), this);
            cornfield4Border1.CornfieldSide = cornfield4sidePart1;
            _cornfieldPart3.Borders.Add(cornfield4Border1);


            // дорога
            var side3 = RotateSide(Side.left, RotationsCount);
            var roadBorder = new Border(this.Field, this.Field?.GetNeighbour(side3), this);
            _roadPart.Borders.Add(roadBorder);
        }
    }
}