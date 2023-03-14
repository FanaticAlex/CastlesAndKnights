using System.Collections.Generic;
using Carcassone.Core.Fields;

namespace Carcassone.Core.Cards
{

    /// <summary>
    ///       F
    ///   |\_____/|
    /// C | __*__ | C
    ///   |/     \|
    ///       F
    /// </summary>
    public class FCFC_2 : Card
    {
        private readonly CastlePart _castlePart2;
        private readonly CornfieldPart _cornfieldPart1;
        private readonly CornfieldPart _cornfieldPart2;

        public FCFC_2(string cardName) : base(cardName)
        {
            _castlePart2 = new CastlePart("Castle_0", cardName, true);
            Parts.Add(_castlePart2);

            _cornfieldPart1 = new CornfieldPart("Cornfield_0", cardName);
            Parts.Add(_cornfieldPart1);

            _cornfieldPart2 = new CornfieldPart("Cornfield_1", cardName);
            Parts.Add(_cornfieldPart2);


            _fieldToCastleParts.Add(_cornfieldPart1, new List<CastlePart>() { _castlePart2 });
            _fieldToCastleParts.Add(_cornfieldPart2, new List<CastlePart>() { _castlePart2 });
        }

        public override void ConnectField(Field field)
        {
            base.ConnectField(field);

            // замок
            var sides1 = new List<Side>() { Side.right, Side.left };
            foreach (var side in sides1)
            {
                var side2 = RotateSide(side, RotationsCount);
                var castleBorder2 = new Border(this.Field, this.Field?.GetNeighbour(side2), this);
                _castlePart2.Borders.Add(castleBorder2);
            }


            // поле1
            var side1 = RotateSide(Side.top, RotationsCount);
            var cornfieldBorder1 = new Border(this.Field, this.Field?.GetNeighbour(side1), this);
            _cornfieldPart1.Borders.Add(cornfieldBorder1);

            // поле2
            var side3 = RotateSide(Side.bottom, RotationsCount);
            var castleBorder3 = new Border(this.Field, this.Field?.GetNeighbour(side3), this);
            _cornfieldPart2.Borders.Add(castleBorder3);
        }
    }
}