using Carcassone.Core.Fields;

namespace Carcassone.Core.Cards
{

    /// <summary>
    ///       F
    ///   |       |
    /// R |_      | F
    ///   |  \    |
    ///       R
    /// </summary>
    public class FFRR : Card
    {
        private RoadPart _roadPart;
        private CornfieldPart _cornfieldPart1;
        private CornfieldPart _cornfieldPart2;

        public FFRR(string cardName) : base(cardName)
        {
            _roadPart = new RoadPart("Road_0", cardName);
            Parts.Add(_roadPart);

            _cornfieldPart1 = new CornfieldPart("Cornfield_0", cardName);
            Parts.Add(_cornfieldPart1);

            _cornfieldPart2 = new CornfieldPart("Cornfield_1", cardName);
            Parts.Add(_cornfieldPart2);
        }

        /// <param name="field"></param>
        public override void ConnectField(Field field)
        {
            base.ConnectField(field);

            // дорога
            var side3 = RotateSide(Side.bottom, RotationsCount);
            var roadBorder3 = new Border(this.Field, this.Field.GetNeighbour(side3), this);
            _roadPart.Borders.Add(roadBorder3);

            var side4 = RotateSide(Side.left, RotationsCount);
            var roadBorder4 = new Border(this.Field, this.Field.GetNeighbour(side4), this);
            _roadPart.Borders.Add(roadBorder4);


            // поле
            var side1 = RotateSide(Side.top, RotationsCount);
            var cornfieldBorder1 = new Border(this.Field, this.Field.GetNeighbour(side1), this);
            _cornfieldPart1.Borders.Add(cornfieldBorder1);

            var side2 = RotateSide(Side.right, RotationsCount);
            var castleBorder2 = new Border(this.Field, this.Field.GetNeighbour(side2), this);
            _cornfieldPart1.Borders.Add(castleBorder2);

            var side31 = RotateSide(Side.bottom, RotationsCount);
            var sidePart31 = RotateSidePart(CornfieldSide.side_3, RotationsCount);
            var cornfieldBorder31 = new Border(this.Field, this.Field.GetNeighbour(side31), this);
            _cornfieldPart1.Borders.Add(cornfieldBorder31);
            cornfieldBorder31.cornfieldSide = sidePart31;

            var side42 = RotateSide(Side.left, RotationsCount);
            var sidePart42 = RotateSidePart(CornfieldSide.side_6, RotationsCount);
            var cornfieldBorder42 = new Border(this.Field, this.Field.GetNeighbour(side42), this);
            _cornfieldPart1.Borders.Add(cornfieldBorder42);
            cornfieldBorder42.cornfieldSide = sidePart42;


            // поле
            var side32 = RotateSide(Side.bottom, RotationsCount);
            var sidePart32 = RotateSidePart(CornfieldSide.side_4, RotationsCount);
            var cornfieldBorder32 = new Border(this.Field, this.Field.GetNeighbour(side32), this);
            _cornfieldPart2.Borders.Add(cornfieldBorder32);
            cornfieldBorder32.cornfieldSide = sidePart32;

            var side41 = RotateSide(Side.left, RotationsCount);
            var sidePart41 = RotateSidePart(CornfieldSide.side_5, RotationsCount);
            var cornfieldBorder41 = new Border(this.Field, this.Field.GetNeighbour(side41), this);
            _cornfieldPart2.Borders.Add(cornfieldBorder41);
            cornfieldBorder41.cornfieldSide = sidePart41;
        }
    }
}