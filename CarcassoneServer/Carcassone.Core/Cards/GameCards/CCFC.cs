using Carcassone.Core.Fields;
using System.Collections;
using System.Collections.Generic;

namespace Carcassone.Core.Cards
{

    /// <summary>
    ///       C
    ///   |       |
    /// C | _____ | C
    ///   |/     \|
    ///       F
    /// </summary>
    public class CCFC : Card
    {
        private CastlePart _castlePart;
        private CornfieldPart _cornfieldPart;

        public CCFC(string cardName) : base(cardName)
        {
            _castlePart = new CastlePart("Castle_0", cardName);
            Parts.Add(_castlePart);

            _cornfieldPart = new CornfieldPart("Cornfield_0", cardName);
            Parts.Add(_cornfieldPart);

            _fieldToCastleParts.Add(_cornfieldPart, new List<CastlePart>() { _castlePart });
        }

        public override void ConnectField(Field field)
        {
            base.ConnectField(field);

            var sides = new List<Side>() { Side.top, Side.right, Side.left };
            foreach (var side in sides)
            {
                var rotatedSide = RotateSide(side, RotationsCount);
                var castleBorder = new Border(this.Field, this.Field.GetNeighbour(rotatedSide), this);
                _castlePart.Borders.Add(castleBorder);
            }


            var side3 = Side.bottom;
            side3 = RotateSide(side3, RotationsCount);
            var cornfieldBorder3 = new Border(this.Field, this.Field.GetNeighbour(side3), this);
            _cornfieldPart.Borders.Add(cornfieldBorder3);
        }
    }
}