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
            Field = field;

            AddBorderToPart(Side.top, _castlePart);
            AddBorderToPart(Side.left, _castlePart);

            AddBorderToPart(Side.right, _cornfieldPart);
            AddBorderToPart(Side.bottom, _cornfieldPart);
        }
    }
}