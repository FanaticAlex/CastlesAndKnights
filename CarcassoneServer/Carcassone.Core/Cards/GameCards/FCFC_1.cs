using Carcassone.Core.Fields;
using System.Collections.Generic;

namespace Carcassone.Core.Cards
{

    /// <summary>
    ///       F
    ///   |\_____/|
    /// C | _____ | C
    ///   |/     \|
    ///       F
    /// </summary>
    public class FCFC_1 : Card
    {
        private CastlePart _castlePart;
        private CornfieldPart _cornfieldPart1;
        private CornfieldPart _cornfieldPart2;

        public FCFC_1(string cardName) : base(cardName)
        {
            _castlePart = new CastlePart("Castle_0", cardName);
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

            // замок
            AddBorderToPart(Side.right, _castlePart);
            AddBorderToPart(Side.left, _castlePart);


            // поле1
            var side1 = RotateSide(Side.top, RotationsCount);
            var cornfieldBorder1 = new Border(this.Field, this.Field?.GetNeighbour(side1), this);
            _cornfieldPart1.Borders.Add(cornfieldBorder1);

            // поле2
            var side3 = RotateSide(Side.bottom, RotationsCount);
            var castleBorder3 = new Border(this.Field, this.Field?.GetNeighbour(side3), this);
            _cornfieldPart2.Borders.Add(castleBorder3);



        }
    }
}