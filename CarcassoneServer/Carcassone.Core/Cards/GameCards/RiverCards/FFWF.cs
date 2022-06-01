using Carcassone.Core.Fields;

namespace Carcassone.Core.Cards
{

    /// <summary>
    ///       F
    ///   |       |
    /// F |   +   | F
    ///   |   +   |
    ///       W
    /// </summary>
    public class FFWF : Card
    {
        private CornfieldPart _cornfieldPart;

        public FFWF(string cardName) : base(cardName)
        {
            _cornfieldPart = new CornfieldPart("Cornfield_0", cardName);
            Parts.Add(_cornfieldPart);
        }

        public override void ConnectField(Field field)
        {
            base.ConnectField(field);

            var side1 = Side.left;
            side1 = RotateSide(side1, RotationsCount);
            var cornfieldBorder1 = new Border(this.Field, this.Field.GetNeighbour(side1), this);
            _cornfieldPart.Borders.Add(cornfieldBorder1);

            var side2 = Side.top;
            side2 = RotateSide(side2, RotationsCount);
            var cornfieldBorder2 = new Border(this.Field, this.Field.GetNeighbour(side2), this);
            _cornfieldPart.Borders.Add(cornfieldBorder2);

            var side3 = Side.right;
            side3 = RotateSide(side3, RotationsCount);
            var cornfieldBorder3 = new Border(this.Field, this.Field.GetNeighbour(side3), this);
            _cornfieldPart.Borders.Add(cornfieldBorder3);

            var cornfield1Side0 = Side.bottom;
            cornfield1Side0 = RotateSide(cornfield1Side0, RotationsCount);
            var cornfield1sidePart0 = CornfieldSide.side_3;
            cornfield1sidePart0 = RotateSidePart(cornfield1sidePart0, RotationsCount);
            var cornfield1Border0 = new Border(this.Field, this.Field.GetNeighbour(cornfield1Side0), this);
            cornfield1Border0.cornfieldSide = cornfield1sidePart0;
            _cornfieldPart.Borders.Add(cornfield1Border0);

            var cornfield1Side1 = Side.bottom;
            cornfield1Side1 = RotateSide(cornfield1Side1, RotationsCount);
            var cornfield1sidePart1 = CornfieldSide.side_4;
            cornfield1sidePart1 = RotateSidePart(cornfield1sidePart1, RotationsCount);
            var cornfield1Border1 = new Border(this.Field, this.Field.GetNeighbour(cornfield1Side1), this);
            cornfield1Border1.cornfieldSide = cornfield1sidePart1;
            _cornfieldPart.Borders.Add(cornfield1Border1);
        }
    }
}