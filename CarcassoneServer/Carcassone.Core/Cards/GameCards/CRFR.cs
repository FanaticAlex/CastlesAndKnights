
using Carcassone.Core.Fields;
using System.Collections.Generic;

namespace Carcassone.Core.Cards
{

    /// <summary>
    ///       C
    ///   |\_____/|
    /// R |_______| R
    ///   |       |
    ///       F
    /// </summary>
    public class CRFR : Card
    {
        private CastlePart _castlePart;
        private RoadPart _roadPart;
        private CornfieldPart _cornfieldPart1;
        private CornfieldPart _cornfieldPart2;

        public CRFR(string cardName) : base(cardName)
        {
            _castlePart = new CastlePart("Castle_0", cardName);
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
            var side = RotateSide(Side.top, RotationsCount);
            var castleBorder = new Border(this.Field, this.Field.GetNeighbour(side), this);
            _castlePart.Borders.Add(castleBorder);

            // дорога
            var side2 = RotateSide(Side.left, RotationsCount);
            var roadBorder2 = new Border(this.Field, this.Field.GetNeighbour(side2), this);
            _roadPart.Borders.Add(roadBorder2);

            var side4 = RotateSide(Side.right, RotationsCount);
            var roadBorder4 = new Border(this.Field, this.Field.GetNeighbour(side4), this);
            _roadPart.Borders.Add(roadBorder4);


            // поле 1
            var side21 = RotateSide(Side.right, RotationsCount);
            var sidePart21 = RotateSidePart(CornfieldSide.side_1, RotationsCount);
            var cornfieldBorder21 = new Border(this.Field, this.Field.GetNeighbour(side21), this);
            _cornfieldPart1.Borders.Add(cornfieldBorder21);
            cornfieldBorder21.cornfieldSide = sidePart21;

            var side42 = RotateSide(Side.left, RotationsCount);
            var sidePart42 = RotateSidePart(CornfieldSide.side_6, RotationsCount);
            var cornfieldBorder42 = new Border(this.Field, this.Field.GetNeighbour(side42), this);
            _cornfieldPart1.Borders.Add(cornfieldBorder42);
            cornfieldBorder42.cornfieldSide = sidePart42;


            // поле 2
            var side22 = RotateSide(Side.right, RotationsCount);
            var sidePart22 = RotateSidePart(CornfieldSide.side_2, RotationsCount);
            var cornfieldBorder22 = new Border(this.Field, this.Field.GetNeighbour(side22), this);
            _cornfieldPart2.Borders.Add(cornfieldBorder22);
            cornfieldBorder22.cornfieldSide = sidePart22;

            var side3 = RotateSide(Side.bottom, RotationsCount);
            var cornfieldBorder3 = new Border(this.Field, this.Field.GetNeighbour(side3), this);
            _cornfieldPart2.Borders.Add(cornfieldBorder3);

            var side41 = RotateSide(Side.left, RotationsCount);
            var sidePart41 = RotateSidePart(CornfieldSide.side_5, RotationsCount);
            var cornfieldBorder41 = new Border(this.Field, this.Field.GetNeighbour(side41), this);
            _cornfieldPart2.Borders.Add(cornfieldBorder41);
            cornfieldBorder41.cornfieldSide = sidePart41;
        }
    }
}