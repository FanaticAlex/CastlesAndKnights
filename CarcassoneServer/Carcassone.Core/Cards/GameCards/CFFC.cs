using Carcassone.Core.Fields;
using System.Collections.Generic;

namespace Carcassone.Core.Cards
{

    /// <summary>
    ///       C
    ///   |    __/|
    /// C | __/   | F
    ///   |/      |
    ///       F
    /// </summary>
    public class CFFC : Card
    {
        private CastlePart _castlePart;
        private CornfieldPart _cornfieldPart;

        public CFFC(string cardName) : base(cardName)
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

            //замок
            var sides = new List<Side>() { Side.top, Side.left };
            foreach (var side in sides)
            {
                var rotatedSide = RotateSide(side, RotationsCount);
                var castleBorder = new Border(this.Field, this.Field?.GetNeighbour(rotatedSide), this);
                _castlePart.Borders.Add(castleBorder);
            }


            // поле
            var sides1 = new List<Side>() { Side.right, Side.bottom };
            foreach (var side in sides1)
            {
                var rotatedSide = RotateSide(side, RotationsCount);
                var cornfieldBorder = new Border(this.Field, this.Field?.GetNeighbour(rotatedSide), this);
                _cornfieldPart.Borders.Add(cornfieldBorder);
            }
        }
    }
}