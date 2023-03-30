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
        private readonly CastlePart _castlePart;
        private readonly CornfieldPart _cornfieldPart;

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
            Field = field;

            AddBorderToPart(Side.top, _castlePart);
            AddBorderToPart(Side.right, _castlePart);
            AddBorderToPart(Side.left, _castlePart);

            AddBorderToPart(Side.bottom, _cornfieldPart);
        }
    }
}