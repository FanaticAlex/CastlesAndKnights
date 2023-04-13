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
        private ChurchPart _churchPart;
        private CornfieldPart _cornfieldPart1;
        private CornfieldPart _cornfieldPart2;
        private CornfieldPart _cornfieldPart3;
        private RoadPart _roadPart;

        public FWRW(string cardName) : base(cardName)
        {
            _churchPart = new ChurchPart("Church_0", cardName);
            Parts.Add(_churchPart);

            _cornfieldPart1 = new CornfieldPart("Cornfield_0", cardName);
            Parts.Add(_cornfieldPart1);

            _cornfieldPart2 = new CornfieldPart("Cornfield_1", cardName);
            Parts.Add(_cornfieldPart2);

            _roadPart = new RoadPart("Road_0", cardName);
            Parts.Add(_roadPart);

            _cornfieldPart3 = new CornfieldPart("Cornfield_2", cardName);
            Parts.Add(_cornfieldPart3);
        }

        public override void ConnectField(Field field)
        {
            Field = field;

            _churchPart.ChurchField = field;

            // поле 1
            var cornfield1Side0 = Side.top;
            cornfield1Side0 = RotateSide(cornfield1Side0, RotationsCount);
            var cornfieldBorder0 = new Border(this.Field, this.Field?.GetNeighbour(cornfield1Side0), this);
            _cornfieldPart1.Borders.Add(cornfieldBorder0);

            var cornfield1Side1 = Side.right;
            cornfield1Side1 = RotateSide(cornfield1Side1, RotationsCount);
            var cornfield1sidePart1 = CornfieldSide.side_1;
            cornfield1sidePart1 = RotateSidePart(cornfield1sidePart1, RotationsCount);
            var cornfield1Border1 = new Border(this.Field, this.Field?.GetNeighbour(cornfield1Side1), this);
            cornfield1Border1.CornfieldSide = cornfield1sidePart1;
            _cornfieldPart1.Borders.Add(cornfield1Border1);

            var cornfield1Side2 = Side.left;
            cornfield1Side2 = RotateSide(cornfield1Side2, RotationsCount);
            var cornfield1SidePart2 = CornfieldSide.side_6;
            cornfield1SidePart2 = RotateSidePart(cornfield1SidePart2, RotationsCount);
            var cornfield1Border2 = new Border(this.Field, this.Field?.GetNeighbour(cornfield1Side2), this);
            cornfield1Border2.CornfieldSide = cornfield1SidePart2;
            _cornfieldPart1.Borders.Add(cornfield1Border2);

            // поле 2
            var cornfield2Side1 = Side.right;
            cornfield2Side1 = RotateSide(cornfield2Side1, RotationsCount);
            var cornfield2sidePart1 = CornfieldSide.side_2;
            cornfield2sidePart1 = RotateSidePart(cornfield2sidePart1, RotationsCount);
            var cornfield2Border1 = new Border(this.Field, this.Field?.GetNeighbour(cornfield2Side1), this);
            cornfield2Border1.CornfieldSide = cornfield2sidePart1;
            _cornfieldPart2.Borders.Add(cornfield2Border1);

            var cornfield2Side2 = Side.bottom;
            cornfield2Side2 = RotateSide(cornfield2Side2, RotationsCount);
            var cornfield2SidePart2 = CornfieldSide.side_3;
            cornfield2SidePart2 = RotateSidePart(cornfield2SidePart2, RotationsCount);
            var cornfield2Border2 = new Border(this.Field, this.Field?.GetNeighbour(cornfield2Side2), this);
            cornfield2Border2.CornfieldSide = cornfield2SidePart2;
            _cornfieldPart2.Borders.Add(cornfield2Border2);

            // дорога
            AddBorderToPart(Side.bottom, _roadPart);

            // поле 3
            var cornfield3Side1 = Side.bottom;
            cornfield3Side1 = RotateSide(cornfield3Side1, RotationsCount);
            var cornfield3sidePart1 = CornfieldSide.side_4;
            cornfield3sidePart1 = RotateSidePart(cornfield3sidePart1, RotationsCount);
            var cornfield3Border1 = new Border(this.Field, this.Field?.GetNeighbour(cornfield3Side1), this);
            cornfield3Border1.CornfieldSide = cornfield3sidePart1;
            _cornfieldPart3.Borders.Add(cornfield3Border1);

            var cornfield3Side2 = Side.left;
            cornfield3Side2 = RotateSide(cornfield3Side2, RotationsCount);
            var cornfield3SidePart2 = CornfieldSide.side_5;
            cornfield3SidePart2 = RotateSidePart(cornfield3SidePart2, RotationsCount);
            var cornfield3Border2 = new Border(this.Field, this.Field?.GetNeighbour(cornfield3Side2), this);
            cornfield3Border2.CornfieldSide = cornfield3SidePart2;
            _cornfieldPart3.Borders.Add(cornfield3Border2);
        }
    }
}