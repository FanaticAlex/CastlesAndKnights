using Carcassone.Core.Fields;
using System.Collections.Generic;

namespace Carcassone.Core.Cards
{

    /// <summary>
    ///       C
    ///   |\_____/|
    /// F |       | F
    ///   |       |
    ///       F
    /// </summary>
    public class CFFF : Card
    {
        private CastlePart _castlePart1;
        private CornfieldPart _cornfieldPart;

        public CFFF(string cardName) : base(cardName)
        {
            _castlePart1 = new CastlePart("Castle_0", cardName);
            Parts.Add(_castlePart1);

            _cornfieldPart = new CornfieldPart("Cornfield_0", cardName);
            Parts.Add(_cornfieldPart);


            _fieldToCastleParts.Add(_cornfieldPart, new List<CastlePart>() { _castlePart1 });
        }

        public override void ConnectField(Field field)
        {
            base.ConnectField(field);

            // замок
            var side1 = RotateSide(Side.top, RotationsCount);
            var castleBorder = new Border(this.Field, this.Field?.GetNeighbour(side1), this);
            _castlePart1.Borders.Add(castleBorder);

            // поле
            var sides1 = new List<Side>() { Side.right, Side.bottom, Side.left };
            foreach (var side in sides1)
            {
                var rotatedSide = RotateSide(side, RotationsCount);
                var cornfieldBorder = new Border(this.Field, this.Field?.GetNeighbour(rotatedSide), this);
                _cornfieldPart.Borders.Add(cornfieldBorder);
            }
        }
    }
}