using Carcassone.Core.Fields;
using System.Collections.Generic;

namespace Carcassone.Core.Cards
{

    /// <summary>
    ///       C
    ///   |\_____/|
    /// R |_______| R
    ///   |   |   |
    ///       R
    /// </summary>
    public class CRRR : Card
    {
        private CastlePart _castlePart;
        private RoadPart _roadPart0;
        private RoadPart _roadPart1;
        private RoadPart _roadPart2;
        private CornfieldPart _cornfieldPart0;
        private CornfieldPart _cornfieldPart1;
        private CornfieldPart _cornfieldPart2;

        public CRRR(string cardName) : base(cardName)
        {
            _castlePart = new CastlePart("Castle_0", cardName);
            Parts.Add(_castlePart);

            _roadPart0 = new RoadPart("Road_0", cardName);
            Parts.Add(_roadPart0);

            _roadPart1 = new RoadPart("Road_1", cardName);
            Parts.Add(_roadPart1);

            _roadPart2 = new RoadPart("Road_2", cardName);
            Parts.Add(_roadPart2);

            _cornfieldPart0 = new CornfieldPart("Cornfield_0", cardName);
            Parts.Add(_cornfieldPart0);

            _cornfieldPart1 = new CornfieldPart("Cornfield_1", cardName);
            Parts.Add(_cornfieldPart1);

            _cornfieldPart2 = new CornfieldPart("Cornfield_2", cardName);
            Parts.Add(_cornfieldPart2);


            _fieldToCastleParts.Add(_cornfieldPart0, new List<CastlePart>() { _castlePart });
        }

        public override void ConnectField(Field field)
        {
            Field = field;

            AddBorderToPart(Side.top, _castlePart);

            AddBorderToPart(Side.right, _roadPart0);

            AddBorderToPart(Side.bottom, _roadPart1);

            AddBorderToPart(Side.left, _roadPart2);


            // поле 1
            var side21 = RotateSide(Side.right, RotationsCount);
            var sidePart21 = RotateSidePart(CornfieldSide.side_1, RotationsCount);
            var cornfieldBorder21 = new Border(this.Field, this.Field?.GetNeighbour(side21), this);
            _cornfieldPart0.Borders.Add(cornfieldBorder21);
            cornfieldBorder21.CornfieldSide = sidePart21;

            var side42 = RotateSide(Side.left, RotationsCount);
            var sidePart42 = RotateSidePart(CornfieldSide.side_6, RotationsCount);
            var cornfieldBorder42 = new Border(this.Field, this.Field?.GetNeighbour(side42), this);
            _cornfieldPart0.Borders.Add(cornfieldBorder42);
            cornfieldBorder42.CornfieldSide = sidePart42;


            // поле 2
            var side22 = RotateSide(Side.right, RotationsCount);
            var sidePart22 = RotateSidePart(CornfieldSide.side_2, RotationsCount);
            var cornfieldBorder22 = new Border(this.Field, this.Field?.GetNeighbour(side22), this);
            _cornfieldPart1.Borders.Add(cornfieldBorder22);
            cornfieldBorder22.CornfieldSide = sidePart22;

            var side31 = RotateSide(Side.bottom, RotationsCount);
            var sidePart31 = RotateSidePart(CornfieldSide.side_3, RotationsCount);
            var cornfieldBorder31 = new Border(this.Field, this.Field?.GetNeighbour(side31), this);
            _cornfieldPart1.Borders.Add(cornfieldBorder31);
            cornfieldBorder31.CornfieldSide = sidePart31;


            // поле 3
            var side32 = RotateSide(Side.bottom, RotationsCount);
            var sidePart32 = RotateSidePart(CornfieldSide.side_4, RotationsCount);
            var cornfieldBorder32 = new Border(this.Field, this.Field?.GetNeighbour(side32), this);
            _cornfieldPart2.Borders.Add(cornfieldBorder32);
            cornfieldBorder32.CornfieldSide = sidePart32;

            var side41 = RotateSide(Side.left, RotationsCount);
            var sidePart41 = RotateSidePart(CornfieldSide.side_5, RotationsCount);
            var cornfieldBorder41 = new Border(this.Field, this.Field?.GetNeighbour(side41), this);
            _cornfieldPart2.Borders.Add(cornfieldBorder41);
            cornfieldBorder41.CornfieldSide = sidePart41;
        }
    }
}