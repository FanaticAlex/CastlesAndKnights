using Carcassone.Core.Fields;
using System.Collections.Generic;

namespace Carcassone.Core.Cards
{

    /// <summary>
    ///       F
    ///   |\     /|
    /// C | |   | | C
    ///   |/     \|
    ///       F
    /// </summary>
    public class FCFC : Card
    {
        private CastlePart _castlePart2;
        private CastlePart _castlePart4;
        private CornfieldPart _cornfieldPart1;

        public FCFC(string cardName) : base(cardName)
        {
            _castlePart2 = new CastlePart("Castle_0", cardName);
            Parts.Add(_castlePart2);

            _castlePart4 = new CastlePart("Castle_1", cardName);
            Parts.Add(_castlePart4);

            _cornfieldPart1 = new CornfieldPart("Cornfield_0", cardName);
            Parts.Add(_cornfieldPart1);


            _fieldToCastleParts.Add(_cornfieldPart1, new List<CastlePart>() { _castlePart2, _castlePart4 });
        }

        public override void ConnectField(Field field)
        {
            base.ConnectField(field);

            // замок
            var side2 = RotateSide(Side.left, RotationsCount);
            var castleBorder2 = new Border(this.Field, this.Field.GetNeighbour(side2), this);
            _castlePart2.Borders.Add(castleBorder2);


            // замок
            var side4 = RotateSide(Side.right, RotationsCount);
            var castleBorder4 = new Border(this.Field, this.Field.GetNeighbour(side4), this);
            _castlePart4.Borders.Add(castleBorder4);


            // поле
            var side1 = RotateSide(Side.top, RotationsCount);
            var cornfieldBorder1 = new Border(this.Field, this.Field.GetNeighbour(side1), this);
            _cornfieldPart1.Borders.Add(cornfieldBorder1);

            var side3 = RotateSide(Side.bottom, RotationsCount);
            var castleBorder3 = new Border(this.Field, this.Field.GetNeighbour(side3), this);
            _cornfieldPart1.Borders.Add(castleBorder3);
        }
    }
}