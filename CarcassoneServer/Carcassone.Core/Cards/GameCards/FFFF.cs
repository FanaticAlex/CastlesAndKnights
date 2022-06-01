using Carcassone.Core.Fields;

namespace Carcassone.Core.Cards
{
    /// <summary>
    ///       F
    ///   |       |
    /// F |   O   | F
    ///   |       |
    ///       F
    /// </summary>
    public class FFFF : Card
    {
        private ChurchPart _churchPart;
        private CornfieldPart _cornfieldPart1;

        public FFFF(string cardName) : base(cardName)
        {
            _churchPart = new ChurchPart("Church_0", cardName);
            Parts.Add(_churchPart);

            _cornfieldPart1 = new CornfieldPart("Cornfield_0", cardName);
            Parts.Add(_cornfieldPart1);
        }

        public override void ConnectField(Field field)
        {
            base.ConnectField(field);

            _churchPart.ChurchField = field;

            // поле
            var side1 = RotateSide(Side.top, RotationsCount);
            var cornfieldBorder1 = new Border(this.Field, this.Field.GetNeighbour(side1), this);
            _cornfieldPart1.Borders.Add(cornfieldBorder1);

            var side2 = RotateSide(Side.right, RotationsCount);
            var castleBorder2 = new Border(this.Field, this.Field.GetNeighbour(side2), this);
            _cornfieldPart1.Borders.Add(castleBorder2);

            var side3 = RotateSide(Side.bottom, RotationsCount);
            var castleBorder3 = new Border(this.Field, this.Field.GetNeighbour(side3), this);
            _cornfieldPart1.Borders.Add(castleBorder3);

            var side4 = RotateSide(Side.left, RotationsCount);
            var castleBorder4 = new Border(this.Field, this.Field.GetNeighbour(side4), this);
            _cornfieldPart1.Borders.Add(castleBorder4);
        }
    }
}