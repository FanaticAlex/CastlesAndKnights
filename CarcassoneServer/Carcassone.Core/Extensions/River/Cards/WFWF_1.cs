using Carcassone.Core.Cards;
using Carcassone.Core.Fields;

namespace Carcassone.Core.Extensions.River.Cards
{
    /// <summary>
    ///       W
    ///   |   +   |
    /// F |   +   | F
    ///   |   +   |
    ///       W
    /// </summary>
    public class WFWF_1 : Card
    {
        private CornfieldPart _cornfieldPart1;
        private CornfieldPart _cornfieldPart2;

        public WFWF_1(string cardName) : base(cardName)
        {
            _cornfieldPart1 = new CornfieldPart("Cornfield_0", cardName);
            Parts.Add(_cornfieldPart1);

            _cornfieldPart2 = new CornfieldPart("Cornfield_1", cardName);
            Parts.Add(_cornfieldPart2);
        }

        public override void ConnectField(Field field)
        {
            Field = field;

            // поле 1
            var cornfield1Side0 = Side.right;
            cornfield1Side0 = RotateSide(cornfield1Side0, RotationsCount);
            var cornfieldBorder0 = new Border(this.Field, this.Field?.GetNeighbour(cornfield1Side0), this);
            _cornfieldPart1.Borders.Add(cornfieldBorder0);

            var cornfield1Side1 = Side.top;
            cornfield1Side1 = RotateSide(cornfield1Side1, RotationsCount);
            var cornfield1sidePart1 = CornfieldSide.side_0;
            cornfield1sidePart1 = RotateSidePart(cornfield1sidePart1, RotationsCount);
            var cornfield1Border1 = new Border(this.Field, this.Field?.GetNeighbour(cornfield1Side1), this);
            cornfield1Border1.CornfieldSide = cornfield1sidePart1;
            _cornfieldPart1.Borders.Add(cornfield1Border1);

            var cornfield1Side2 = Side.bottom;
            cornfield1Side2 = RotateSide(cornfield1Side2, RotationsCount);
            var cornfield1sidePart2 = CornfieldSide.side_3;
            cornfield1sidePart2 = RotateSidePart(cornfield1sidePart2, RotationsCount);
            var cornfield1Border2 = new Border(this.Field, this.Field?.GetNeighbour(cornfield1Side2), this);
            cornfield1Border2.CornfieldSide = cornfield1sidePart2;
            _cornfieldPart1.Borders.Add(cornfield1Border2);


            // поле 2
            var cornfield2Side1 = Side.bottom;
            cornfield2Side1 = RotateSide(cornfield2Side1, RotationsCount);
            var cornfield2sidePart1 = CornfieldSide.side_4;
            cornfield2sidePart1 = RotateSidePart(cornfield2sidePart1, RotationsCount);
            var cornfield2Border1 = new Border(this.Field, this.Field?.GetNeighbour(cornfield2Side1), this);
            cornfield2Border1.CornfieldSide = cornfield2sidePart1;
            _cornfieldPart2.Borders.Add(cornfield2Border1);

            var cornfield2Side2 = Side.top;
            cornfield2Side2 = RotateSide(cornfield2Side2, RotationsCount);
            var cornfield2SidePart2 = CornfieldSide.side_7;
            cornfield2SidePart2 = RotateSidePart(cornfield2SidePart2, RotationsCount);
            var cornfield2Border2 = new Border(this.Field, this.Field?.GetNeighbour(cornfield2Side2), this);
            cornfield2Border2.CornfieldSide = cornfield2SidePart2;
            _cornfieldPart2.Borders.Add(cornfield2Border2);

            var cornfield2Side3 = Side.left;
            cornfield2Side3 = RotateSide(cornfield2Side3, RotationsCount);
            var cornfield2Border3 = new Border(this.Field, this.Field?.GetNeighbour(cornfield2Side3), this);
            _cornfieldPart2.Borders.Add(cornfield2Border3);
        }
    }
}