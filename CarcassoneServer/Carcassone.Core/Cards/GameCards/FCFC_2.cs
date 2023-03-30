using System.Collections.Generic;
using Carcassone.Core.Fields;

namespace Carcassone.Core.Cards
{

    /// <summary>
    ///       F
    ///   |\_____/|
    /// C | __*__ | C
    ///   |/     \|
    ///       F
    /// </summary>
    public class FCFC_2 : Card
    {
        private readonly CastlePart _castlePart;
        private readonly CornfieldPart _cornfieldPart1;
        private readonly CornfieldPart _cornfieldPart2;

        public FCFC_2(string cardName) : base(cardName)
        {
            _castlePart = new CastlePart("Castle_0", cardName, true);
            Parts.Add(_castlePart);

            _cornfieldPart1 = new CornfieldPart("Cornfield_0", cardName);
            Parts.Add(_cornfieldPart1);

            _cornfieldPart2 = new CornfieldPart("Cornfield_1", cardName);
            Parts.Add(_cornfieldPart2);


            _fieldToCastleParts.Add(_cornfieldPart1, new List<CastlePart>() { _castlePart });
            _fieldToCastleParts.Add(_cornfieldPart2, new List<CastlePart>() { _castlePart });
        }

        public override void ConnectField(Field field)
        {
            Field = field;

            AddBorderToPart(Side.right, _castlePart);
            AddBorderToPart(Side.left, _castlePart);

            AddBorderToPart(Side.top, _cornfieldPart1);

            AddBorderToPart(Side.bottom, _cornfieldPart2);
        }
    }
}