using Carcassone.Core.Fields;

namespace Carcassone.Core.Cards
{
    /// <summary>
    ///       F
    ///   |       |
    /// F |   ++++| W
    ///   |   +   |
    ///       W
    /// </summary>
    public class FWWF : Card
    {
        private CornfieldPart _cornfieldPart1;
        private CornfieldPart _cornfieldPart2;

        public FWWF(string cardName) : base(cardName)
        {
            _cornfieldPart1 = new CornfieldPart("Cornfield_0", cardName);
            Parts.Add(_cornfieldPart1);

            _cornfieldPart2 = new CornfieldPart("Cornfield_1", cardName);
            Parts.Add(_cornfieldPart2);
        }

        public override void ConnectField(Field field)
        {
            base.ConnectField(field);

            // поле 1
            var cornfield1Side0 = Side.top;
            cornfield1Side0 = RotateSide(cornfield1Side0, RotationsCount);
            var cornfieldBorder0 = new Border(this.Field, this.Field.GetNeighbour(cornfield1Side0), this);
            _cornfieldPart1.Borders.Add(cornfieldBorder0);

            var cornfield1Side1 = Side.right;
            cornfield1Side1 = RotateSide(cornfield1Side1, RotationsCount);
            var cornfield1sidePart1 = CornfieldSide.side_1;
            cornfield1sidePart1 = RotateSidePart(cornfield1sidePart1, RotationsCount);
            var cornfield1Border1 = new Border(this.Field, this.Field.GetNeighbour(cornfield1Side1), this);
            cornfield1Border1.cornfieldSide = cornfield1sidePart1;
            _cornfieldPart1.Borders.Add(cornfield1Border1);

            var cornfield1Side2 = Side.left;
            cornfield1Side2 = RotateSide(cornfield1Side2, RotationsCount);
            var cornfield1Border2 = new Border(this.Field, this.Field.GetNeighbour(cornfield1Side2), this);
            _cornfieldPart1.Borders.Add(cornfield1Border2);


            // поле 2
            var cornfield2Side1 = Side.right;
            cornfield2Side1 = RotateSide(cornfield2Side1, RotationsCount);
            var cornfield2sidePart1 = CornfieldSide.side_2;
            cornfield2sidePart1 = RotateSidePart(cornfield2sidePart1, RotationsCount);
            var cornfield2Border1 = new Border(this.Field, this.Field.GetNeighbour(cornfield2Side1), this);
            cornfield2Border1.cornfieldSide = cornfield2sidePart1;
            _cornfieldPart2.Borders.Add(cornfield2Border1);

            var cornfield2Side2 = Side.bottom;
            cornfield2Side2 = RotateSide(cornfield2Side2, RotationsCount);
            var cornfield2SidePart2 = CornfieldSide.side_3;
            cornfield2SidePart2 = RotateSidePart(cornfield2SidePart2, RotationsCount);
            var cornfield2Border2 = new Border(this.Field, this.Field.GetNeighbour(cornfield2Side2), this);
            cornfield2Border2.cornfieldSide = cornfield2SidePart2;
            _cornfieldPart2.Borders.Add(cornfield2Border2);
        }
    }
}