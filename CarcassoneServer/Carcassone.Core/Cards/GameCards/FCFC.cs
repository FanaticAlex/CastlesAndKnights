using Carcassone.Core.Fields;
using System.Collections.Generic;

namespace Carcassone.Core.Cards
{

    /// <summary>
    ///       F
    ///   |\     /|
    /// C | |   | | C
    ///   |/     \|
    ///       F
    /// </summary>
    public class FCFC : Card
    {
        private CastlePart _castlePart0;
        private CastlePart _castlePart1;
        private CornfieldPart _cornfieldPart1;

        public FCFC(string cardName) : base(cardName)
        {
            _castlePart0 = new CastlePart("Castle_0", cardName);
            Parts.Add(_castlePart0);

            _castlePart1 = new CastlePart("Castle_1", cardName);
            Parts.Add(_castlePart1);

            _cornfieldPart1 = new CornfieldPart("Cornfield_0", cardName);
            Parts.Add(_cornfieldPart1);


            _fieldToCastleParts.Add(_cornfieldPart1, new List<CastlePart>() { _castlePart0, _castlePart1 });
        }

        public override void ConnectField(Field field)
        {
            Field = field;

            AddBorderToPart(Side.left, _castlePart0);

            AddBorderToPart(Side.right, _castlePart1);

            AddBorderToPart(Side.top, _cornfieldPart1);
            AddBorderToPart(Side.bottom, _cornfieldPart1);
        }
    }
}