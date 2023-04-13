using Carcassone.Core.Cards;
using Carcassone.Core.Fields;

namespace Carcassone.Core.Extensions.River.Cards
{
    /// <summary>
    ///       W
    ///   |   +  /|
    /// W |++ /   | C
    ///   |/      |
    ///       C
    /// </summary>
    public class WCCW : Card
    {
        private CornfieldPart _cornfieldPart1;
        private CornfieldPart _cornfieldPart2;
        private CastlePart _castlePart1;

        public WCCW(string cardName) : base(cardName)
        {
            _cornfieldPart1 = new CornfieldPart("Cornfield_0", cardName);
            Parts.Add(_cornfieldPart1);

            _cornfieldPart2 = new CornfieldPart("Cornfield_1", cardName);
            Parts.Add(_cornfieldPart2);

            _castlePart1 = new CastlePart("Castle_0", cardName);
            Parts.Add(_castlePart1);
        }

        public override void ConnectField(Field field)
        {
            Field = field;

            // замок
            AddBorderToPart(Side.right, _castlePart1);
            AddBorderToPart(Side.bottom, _castlePart1);

            
            // поле 1
            var cornfield1Side0 = Side.top;
            cornfield1Side0 = RotateSide(cornfield1Side0, RotationsCount);
            var cornfield1sidePart0 = CornfieldSide.side_0;
            cornfield1sidePart0 = RotateSidePart(cornfield1sidePart0, RotationsCount);
            var cornfield1Border0 = new Border(this.Field, this.Field?.GetNeighbour(cornfield1Side0), this);
            cornfield1Border0.CornfieldSide = cornfield1sidePart0;
            _cornfieldPart1.Borders.Add(cornfield1Border0);

            var cornfield1Side1 = Side.left;
            cornfield1Side1 = RotateSide(cornfield1Side1, RotationsCount);
            var cornfield1sidePart1 = CornfieldSide.side_5;
            cornfield1sidePart1 = RotateSidePart(cornfield1sidePart1, RotationsCount);
            var cornfield1Border1 = new Border(this.Field, this.Field?.GetNeighbour(cornfield1Side1), this);
            cornfield1Border1.CornfieldSide = cornfield1sidePart1;
            _cornfieldPart1.Borders.Add(cornfield1Border1);

            // поле 1
            var cornfield2Side0 = Side.top;
            cornfield2Side0 = RotateSide(cornfield2Side0, RotationsCount);
            var cornfield2sidePart0 = CornfieldSide.side_7;
            cornfield2sidePart0 = RotateSidePart(cornfield2sidePart0, RotationsCount);
            var cornfield2Border0 = new Border(this.Field, this.Field?.GetNeighbour(cornfield2Side0), this);
            cornfield2Border0.CornfieldSide = cornfield2sidePart0;
            _cornfieldPart2.Borders.Add(cornfield2Border0);

            var cornfield2Side1 = Side.left;
            cornfield2Side1 = RotateSide(cornfield2Side1, RotationsCount);
            var cornfield2sidePart1 = CornfieldSide.side_6;
            cornfield2sidePart1 = RotateSidePart(cornfield2sidePart1, RotationsCount);
            var cornfield2Border1 = new Border(this.Field, this.Field?.GetNeighbour(cornfield2Side1), this);
            cornfield2Border1.CornfieldSide = cornfield2sidePart1;
            _cornfieldPart2.Borders.Add(cornfield2Border1);
        }
    }
}