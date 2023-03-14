using Carcassone.Core.Fields;
using System.Collections.Generic;

namespace Carcassone.Core.Cards
{

    /// <summary>
    ///       C
    ///   |   *   |
    /// C | _____ | C
    ///   |/  |  \|
    ///       R
    /// </summary>
    public class CCRC_1 : Card
    {
        private readonly CastlePart _castlePart;
        private readonly RoadPart _roadPart;
        private readonly CornfieldPart _cornfieldPart1;
        private readonly CornfieldPart _cornfieldPart2;

        public CCRC_1(string cardName) : base(cardName)
        {
            _castlePart = new CastlePart("Castle_0", cardName, true);
            Parts.Add(_castlePart);

            _roadPart = new RoadPart("Road_0", cardName);
            Parts.Add(_roadPart);

            _cornfieldPart1 = new CornfieldPart("Cornfield_0", cardName);
            Parts.Add(_cornfieldPart1);

            _cornfieldPart2 = new CornfieldPart("Cornfield_1", cardName);
            Parts.Add(_cornfieldPart2);


            _fieldToCastleParts.Add(_cornfieldPart1, new List<CastlePart>() { _castlePart });
            _fieldToCastleParts.Add(_cornfieldPart2, new List<CastlePart>() { _castlePart });
        }


        public override void ConnectField(Field field)
        {
            base.ConnectField(field);

            //замок
            var sides = new List<Side>() { Side.top, Side.right, Side.left };
            foreach (var side in sides)
            {
                var rotatedSide = RotateSide(side, RotationsCount);
                var castleBorder = new Border(this.Field, this.Field?.GetNeighbour(rotatedSide), this);
                _castlePart.Borders.Add(castleBorder);
            }

            // дорога
            var side3 = Side.bottom;
            side3 = RotateSide(side3, RotationsCount);
            var roadBorder3 = new Border(this.Field, this.Field?.GetNeighbour(side3), this);
            _roadPart.Borders.Add(roadBorder3);

            // поле
            var side31 = Side.bottom;
            side31 = RotateSide(side31, RotationsCount);
            var sidePart31 = CornfieldSide.side_3;
            sidePart31 = RotateSidePart(sidePart31, RotationsCount);
            var cornfieldBorder31 = new Border(this.Field, this.Field?.GetNeighbour(side31), this);
            cornfieldBorder31.CornfieldSide = sidePart31;
            _cornfieldPart1.Borders.Add(cornfieldBorder31);

            // поле
            var side32 = Side.bottom;
            side32 = RotateSide(side32, RotationsCount);
            var sidePart32 = CornfieldSide.side_4;
            sidePart32 = RotateSidePart(sidePart32, RotationsCount);
            var cornfieldBorder32 = new Border(this.Field, this.Field?.GetNeighbour(side32), this);
            cornfieldBorder32.CornfieldSide = sidePart32;
            _cornfieldPart2.Borders.Add(cornfieldBorder32);
        }
    }
}