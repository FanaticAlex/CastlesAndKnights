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
        private readonly CastlePart _castlePart;

        public CCCC(string cardName) : base(cardName)
        {
            _castlePart = new CastlePart("Castle_0", cardName);
            Parts.Add(_castlePart);
        }

        public override void ConnectField(Field field)
        {
            Field = field;

            AddBorderToPart(Side.top, _castlePart);
            AddBorderToPart(Side.right, _castlePart);
            AddBorderToPart(Side.bottom, _castlePart);
            AddBorderToPart(Side.left, _castlePart);
        }
    }
}