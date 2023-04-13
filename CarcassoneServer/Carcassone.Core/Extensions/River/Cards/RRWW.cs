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
        private CornfieldPart _cornfieldPart0;
        private CornfieldPart _cornfieldPart1;
        private CornfieldPart _cornfieldPart2;
        private RoadPart _roadPart;

        public RRWW(string cardName) : base(cardName)
        {
            _cornfieldPart0 = new CornfieldPart("Cornfield_0", cardName);
            Parts.Add(_cornfieldPart0);

            _cornfieldPart1 = new CornfieldPart("Cornfield_1", cardName);
            Parts.Add(_cornfieldPart1);

            _cornfieldPart2 = new CornfieldPart("Cornfield_2", cardName);
            Parts.Add(_cornfieldPart2);

            _roadPart = new RoadPart("Road_0", cardName);
            Parts.Add(_roadPart);
        }

        public override void ConnectField(Field field)
        {
            Field = field;

            // поле 1
            var cornfield1Side0 = Side.top;
            cornfield1Side0 = RotateSide(cornfield1Side0, RotationsCount);
            var cornfield1sidePart0 = CornfieldSide.side_0;
            cornfield1sidePart0 = RotateSidePart(cornfield1sidePart0, RotationsCount);
            var cornfield1Border0 = new Border(this.Field, this.Field?.GetNeighbour(cornfield1Side0), this);
            cornfield1Border0.CornfieldSide = cornfield1sidePart0;
            _cornfieldPart0.Borders.Add(cornfield1Border0);

            var cornfield1Side1 = Side.right;
            cornfield1Side1 = RotateSide(cornfield1Side1, RotationsCount);
            var cornfield1sidePart1 = CornfieldSide.side_1;
            cornfield1sidePart1 = RotateSidePart(cornfield1sidePart1, RotationsCount);
            var cornfield1Border1 = new Border(this.Field, this.Field?.GetNeighbour(cornfield1Side1), this);
            cornfield1Border1.CornfieldSide = cornfield1sidePart1;
            _cornfieldPart0.Borders.Add(cornfield1Border1);


            // поле 2
            var cornfield2Side1 = Side.right;
            cornfield2Side1 = RotateSide(cornfield2Side1, RotationsCount);
            var cornfield2sidePart1 = CornfieldSide.side_2;
            cornfield2sidePart1 = RotateSidePart(cornfield2sidePart1, RotationsCount);
            var cornfield2Border1 = new Border(this.Field, this.Field?.GetNeighbour(cornfield2Side1), this);
            cornfield2Border1.CornfieldSide = cornfield2sidePart1;
            _cornfieldPart1.Borders.Add(cornfield2Border1);

            var cornfield2Side2 = Side.bottom;
            cornfield2Side2 = RotateSide(cornfield2Side2, RotationsCount);
            var cornfield2SidePart2 = CornfieldSide.side_3;
            cornfield2SidePart2 = RotateSidePart(cornfield2SidePart2, RotationsCount);
            var cornfield2Border2 = new Border(this.Field, this.Field?.GetNeighbour(cornfield2Side2), this);
            cornfield2Border2.CornfieldSide = cornfield2SidePart2;
            _cornfieldPart1.Borders.Add(cornfield2Border2);

            var cornfield2Side3 = Side.left;
            cornfield2Side3 = RotateSide(cornfield2Side3, RotationsCount);
            var cornfield2sidePart3 = CornfieldSide.side_6;
            cornfield2sidePart3 = RotateSidePart(cornfield2sidePart3, RotationsCount);
            var cornfield2Border3 = new Border(this.Field, this.Field?.GetNeighbour(cornfield2Side3), this);
            cornfield2Border3.CornfieldSide = cornfield2sidePart3;
            _cornfieldPart1.Borders.Add(cornfield2Border3);

            var cornfield2Side4 = Side.top;
            cornfield2Side4 = RotateSide(cornfield2Side4, RotationsCount);
            var cornfield2SidePart4 = CornfieldSide.side_7;
            cornfield2SidePart4 = RotateSidePart(cornfield2SidePart4, RotationsCount);
            var cornfield2Border4 = new Border(this.Field, this.Field?.GetNeighbour(cornfield2Side4), this);
            cornfield2Border4.CornfieldSide = cornfield2SidePart4;
            _cornfieldPart1.Borders.Add(cornfield2Border4);


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

            // дорога
            AddBorderToPart(Side.top, _roadPart);
            AddBorderToPart(Side.right, _roadPart);
        }
    }
}