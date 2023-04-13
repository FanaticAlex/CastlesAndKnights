using Carcassone.Core.Cards;
using Carcassone.Core.Fields;
using System.Collections.Generic;

namespace Carcassone.Core.Extensions.River.Cards
{

    /// <summary>
    ///       C
    ///   |\-----/|
    /// W |+++++++| W
    ///   |/-----\|
    ///       C
    /// </summary>
    public class CWCW : Card
    {
        private CastlePart _castlePart0;
        private CastlePart _castlePart1;
        private CornfieldPart _cornfieldPart0;
        private CornfieldPart _cornfieldPart1;

        public CWCW(string cardName) : base(cardName)
        {
            _castlePart0 = new CastlePart("Castle_0", cardName);
            Parts.Add(_castlePart0);

            _castlePart1 = new CastlePart("Castle_1", cardName);
            Parts.Add(_castlePart1);

            _cornfieldPart0 = new CornfieldPart("Cornfield_0", cardName);
            Parts.Add(_cornfieldPart0);

            _cornfieldPart1 = new CornfieldPart("Cornfield_1", cardName);
            Parts.Add(_cornfieldPart1);


            _fieldToCastleParts.Add(_cornfieldPart0, new List<CastlePart>() { _castlePart0 });
            _fieldToCastleParts.Add(_cornfieldPart1, new List<CastlePart>() { _castlePart1 });
        }

        public override void ConnectField(Field field)
        {
            Field = field;

            AddBorderToPart(Side.top, _castlePart0);

            AddBorderToPart(Side.bottom, _castlePart1);

            // поле 1
            var cornfield1Side1 = Side.right;
            cornfield1Side1 = RotateSide(cornfield1Side1, RotationsCount);
            var cornfield1sidePart1 = CornfieldSide.side_1;
            cornfield1sidePart1 = RotateSidePart(cornfield1sidePart1, RotationsCount);
            var cornfield1Border1 = new Border(this.Field, this.Field?.GetNeighbour(cornfield1Side1), this);
            cornfield1Border1.CornfieldSide = cornfield1sidePart1;
            _cornfieldPart0.Borders.Add(cornfield1Border1);

            var cornfield1Side2 = Side.left;
            cornfield1Side2 = RotateSide(cornfield1Side2, RotationsCount);
            var cornfield1SidePart2 = CornfieldSide.side_6;
            cornfield1SidePart2 = RotateSidePart(cornfield1SidePart2, RotationsCount);
            var cornfield1Border2 = new Border(this.Field, this.Field?.GetNeighbour(cornfield1Side2), this);
            cornfield1Border2.CornfieldSide = cornfield1SidePart2;
            _cornfieldPart0.Borders.Add(cornfield1Border2);


            // поле 2
            var cornfield2Side1 = Side.right;
            cornfield2Side1 = RotateSide(cornfield2Side1, RotationsCount);
            var cornfield2SidePart1 = CornfieldSide.side_2;
            cornfield2SidePart1 = RotateSidePart(cornfield2SidePart1, RotationsCount);
            var cornfield2Border1 = new Border(this.Field, this.Field?.GetNeighbour(cornfield2Side1), this);
            cornfield2Border1.CornfieldSide = cornfield2SidePart1;
            _cornfieldPart1.Borders.Add(cornfield2Border1);

            var cornfield2Side2 = Side.left;
            cornfield2Side2 = RotateSide(cornfield2Side2, RotationsCount);
            var cornfield2SidePart2 = CornfieldSide.side_5;
            cornfield2SidePart2 = RotateSidePart(cornfield2SidePart2, RotationsCount);
            var cornfield2Border2 = new Border(this.Field, this.Field?.GetNeighbour(cornfield2Side2), this);
            cornfield2Border2.CornfieldSide = cornfield2SidePart2;
            _cornfieldPart1.Borders.Add(cornfield2Border2);
        }
    }
}