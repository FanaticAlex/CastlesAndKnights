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

            // замок 1
            AddBorderToPart(Side.left, _castlePart0);

            // замок 2
            AddBorderToPart(Side.right, _castlePart1);


            // поле
            var side1 = RotateSide(Side.top, RotationsCount);
            var cornfieldBorder1 = new Border(this.Field, this.Field?.GetNeighbour(side1), this);
            _cornfieldPart1.Borders.Add(cornfieldBorder1);

            var side3 = RotateSide(Side.bottom, RotationsCount);
            var castleBorder3 = new Border(this.Field, this.Field?.GetNeighbour(side3), this);
            _cornfieldPart1.Borders.Add(castleBorder3);
        }
    }
}