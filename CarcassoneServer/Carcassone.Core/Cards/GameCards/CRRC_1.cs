using Carcassone.Core.Fields;
using System.Collections.Generic;

namespace Carcassone.Core.Cards
{

    /// <summary>
    ///       C
    ///   | *  __/|
    /// C | __/  _| R
    ///   |/   /  |
    ///       R
    /// </summary>
    public class CRRC_1 : Card
    {
        private CastlePart _castlePart;
        private ObjectPart _roadPart;
        private CornfieldPart _cornfieldPart1;
        private ObjectPart _cornfieldPart2;

        public CRRC_1(string cardName) : base(cardName)
        {
            _castlePart = new CastlePart("Castle_0", cardName);
            _castlePart.IsThereShield = true;
            Parts.Add(_castlePart);

            _roadPart = new RoadPart("Road_0", cardName);
            Parts.Add(_roadPart);

            _cornfieldPart1 = new CornfieldPart("Cornfield_0", cardName);
            Parts.Add(_cornfieldPart1);

            _cornfieldPart2 = new CornfieldPart("Cornfield_1", cardName);
            Parts.Add(_cornfieldPart2);


            _fieldToCastleParts.Add(_cornfieldPart1, new List<CastlePart>() { _castlePart });
        }

        public override void ConnectField(Field field)
        {
            base.ConnectField(field);

            // замок
            var sides = new List<Side>() { Side.top, Side.left };
            foreach (var side in sides)
            {
                var rotatedSide = RotateSide(side, RotationsCount);
                var castleBorder = new Border(this.Field, this.Field.GetNeighbour(rotatedSide), this);
                _castlePart.Borders.Add(castleBorder);
            }


            // дорога
            var side2 = RotateSide(Side.right, RotationsCount);
            var roadBorder2 = new Border(this.Field, this.Field.GetNeighbour(side2), this);
            _roadPart.Borders.Add(roadBorder2);

            var side3 = RotateSide(Side.bottom, RotationsCount);
            var roadBorder4 = new Border(this.Field, this.Field.GetNeighbour(side3), this);
            _roadPart.Borders.Add(roadBorder4);


            // поле 1
            var side21 = RotateSide(Side.right, RotationsCount);
            var sidePart21 = RotateSidePart(CornfieldSide.side_1, RotationsCount);
            var cornfieldBorder21 = new Border(this.Field, this.Field.GetNeighbour(side21), this);
            _cornfieldPart1.Borders.Add(cornfieldBorder21);
            cornfieldBorder21.cornfieldSide = sidePart21;

            var side32 = RotateSide(Side.bottom, RotationsCount);
            var sidePart32 = RotateSidePart(CornfieldSide.side_4, RotationsCount);
            var cornfieldBorder32 = new Border(this.Field, this.Field.GetNeighbour(side32), this);
            _cornfieldPart1.Borders.Add(cornfieldBorder32);
            cornfieldBorder32.cornfieldSide = sidePart32;


            // поле 2
            var side22 = RotateSide(Side.right, RotationsCount);
            var sidePart22 = RotateSidePart(CornfieldSide.side_2, RotationsCount);
            var cornfieldBorder22 = new Border(this.Field, this.Field.GetNeighbour(side22), this);
            _cornfieldPart2.Borders.Add(cornfieldBorder22);
            cornfieldBorder22.cornfieldSide = sidePart22;

            var side31 = RotateSide(Side.bottom, RotationsCount);
            var sidePart31 = RotateSidePart(CornfieldSide.side_3, RotationsCount);
            var cornfieldBorder31 = new Border(this.Field, this.Field.GetNeighbour(side31), this);
            _cornfieldPart2.Borders.Add(cornfieldBorder31);
            cornfieldBorder31.cornfieldSide = sidePart31;
        }
    }
}