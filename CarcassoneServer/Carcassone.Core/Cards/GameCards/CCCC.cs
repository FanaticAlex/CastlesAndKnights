using System.Collections.Generic;
using Carcassone.Core.Fields;

namespace Carcassone.Core.Cards
{

    /// <summary>
    ///       C
    ///   |       |
    /// C |       | C
    ///   |       |
    ///       C
    /// </summary>
    public class CCCC : Card
    {
        private CastlePart _castlePart;

        public CCCC(string cardName) : base(cardName)
        {
            _castlePart = new CastlePart("Castle_0", cardName);
            Parts.Add(_castlePart);
        }

        public override void ConnectField(Field field)
        {
            base.ConnectField(field);

            // для каждой стороны замка добавляем границу
            var sides = new List<Side>() { Side.top, Side.right, Side.bottom, Side.left };
            foreach (var side in sides)
            {
                // учет поворота карты
                var rotatedSide = RotateSide(side, RotationsCount);
                var castleBorder = new Border(Field, Field.GetNeighbour(rotatedSide), this);
                _castlePart.Borders.Add(castleBorder);
            }
        }
    }
}