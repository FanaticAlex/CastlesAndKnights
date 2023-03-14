using Carcassone.Core.Fields;
using System.Collections;
using System.Collections.Generic;

namespace Carcassone.Core.Cards
{

    /// <summary>
    ///       C
    ///   |\_____//|
    /// F |      | | C
    ///   |       \|
    ///       F
    /// </summary>
    public class CCFF : Card
    {
        private CastlePart _castlePart;
        private CastlePart _castlePart1;
        private CornfieldPart _cornfieldPart;

        public CCFF(string cardName) : base(cardName)
        {
            _castlePart = new CastlePart("Castle_0", cardName);
            Parts.Add(_castlePart);

            _castlePart1 = new CastlePart("Castle_1", cardName);
            Parts.Add(_castlePart1);

            _cornfieldPart = new CornfieldPart("Cornfield_0", cardName);
            Parts.Add(_cornfieldPart);

            _fieldToCastleParts.Add(_cornfieldPart, new List<CastlePart>() { _castlePart, _castlePart1 });
        }

        /// <param name="field"></param>
        public override void ConnectField(Field field)
        {
            base.ConnectField(field);

            //замок 1
            var sides = new List<Side>() { Side.top };
            foreach (var side in sides)
            {
                var rotatedSide = RotateSide(side, RotationsCount);
                var castleBorder = new Border(this.Field, this.Field?.GetNeighbour(rotatedSide), this);
                _castlePart.Borders.Add(castleBorder);
            }

            //замок 2
            var sides1 = new List<Side>() { Side.right };
            foreach (var side in sides1)
            {
                var rotatedSide = RotateSide(side, RotationsCount);
                var castleBorder = new Border(this.Field, this.Field?.GetNeighbour(rotatedSide), this);
                _castlePart1.Borders.Add(castleBorder);
            }

            // поле
            var sides2 = new List<Side>() { Side.bottom, Side.left };
            foreach (var side in sides2)
            {
                var cornfieldSide = RotateSide(side, RotationsCount);
                var cornfieldBorder3 = new Border(this.Field, this.Field?.GetNeighbour(cornfieldSide), this);
                _cornfieldPart.Borders.Add(cornfieldBorder3);
            }
        }
    }
}