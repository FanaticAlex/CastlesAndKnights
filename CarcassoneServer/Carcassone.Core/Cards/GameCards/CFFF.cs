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
        private CastlePart _castlePart;
        private CornfieldPart _cornfieldPart;

        public CFFF(string cardName) : base(cardName)
        {
            _castlePart = new CastlePart("Castle_0", cardName);
            Parts.Add(_castlePart);

            _cornfieldPart = new CornfieldPart("Cornfield_0", cardName);
            Parts.Add(_cornfieldPart);


            _fieldToCastleParts.Add(_cornfieldPart, new List<CastlePart>() { _castlePart });
        }

        public override void ConnectField(Field field)
        {
            Field = field;

            // замок
            AddBorderToPart(Side.top, _castlePart);

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