using Carcassone.Core.Fields;

namespace Carcassone.Core.Cards
{
    /// <summary>
    ///       W
    ///   |   +   |
    /// F |   +   | F
    ///   |       |
    ///       F
    /// </summary>
    public class WFFF : Card
    {
        private CornfieldPart _cornfieldPart1;

        public WFFF(string cardName) : base(cardName)
        {
            _cornfieldPart1 = new CornfieldPart("Cornfield_0", cardName);
            Parts.Add(_cornfieldPart1);
        }

        public override void ConnectField(Field field)
        {
            base.ConnectField(field);

            // поле 1
            var cornfield1Side0 = Side.top;
            cornfield1Side0 = RotateSide(cornfield1Side0, RotationsCount);
            var cornfield1sidePart0 = CornfieldSide.side_0;
            cornfield1sidePart0 = RotateSidePart(cornfield1sidePart0, RotationsCount);
            var cornfield1Border0 = new Border(this.Field, this.Field.GetNeighbour(cornfield1Side0), this);
            cornfield1Border0.cornfieldSide = cornfield1sidePart0;
            _cornfieldPart1.Borders.Add(cornfield1Border0);

            var cornfield1Side1 = Side.top;
            cornfield1Side1 = RotateSide(cornfield1Side1, RotationsCount);
            var cornfield1sidePart1 = CornfieldSide.side_7;
            cornfield1sidePart1 = RotateSidePart(cornfield1sidePart1, RotationsCount);
            var cornfield1Border1 = new Border(this.Field, this.Field.GetNeighbour(cornfield1Side1), this);
            cornfield1Border1.cornfieldSide = cornfield1sidePart1;
            _cornfieldPart1.Borders.Add(cornfield1Border1);

            var Side1 = Side.right;
            Side1 = RotateSide(Side1, RotationsCount);
            var cornfieldBorder1 = new Border(this.Field, this.Field.GetNeighbour(Side1), this);
            _cornfieldPart1.Borders.Add(cornfieldBorder1);

            var Side2 = Side.bottom;
            Side2 = RotateSide(Side2, RotationsCount);
            var cornfieldBorder2 = new Border(this.Field, this.Field.GetNeighbour(Side2), this);
            _cornfieldPart1.Borders.Add(cornfieldBorder2);

            var Side3 = Side.left;
            Side3 = RotateSide(Side3, RotationsCount);
            var cornfieldBorder3 = new Border(this.Field, this.Field.GetNeighbour(Side3), this);
            _cornfieldPart1.Borders.Add(cornfieldBorder3);
        }
    }
}