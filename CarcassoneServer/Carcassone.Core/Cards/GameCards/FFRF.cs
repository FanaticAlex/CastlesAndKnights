using Carcassone.Core.Fields;

namespace Carcassone.Core.Cards
{
    /// <summary>
    ///       F
    ///   |       |
    /// F |   O   | F
    ///   |   |   |
    ///       R
    /// </summary>
    public class FFRF : Card
    {
        private RoadPart _roadPart3;
        private ChurchPart _churchPart;
        private CornfieldPart _cornfieldPart1;

        public FFRF(string cardName) : base(cardName)
        {
            _roadPart3 = new RoadPart("Road_0", cardName);
            Parts.Add(_roadPart3);

            _churchPart = new ChurchPart("Church_0", cardName);
            Parts.Add(_churchPart);

            _cornfieldPart1 = new CornfieldPart("Cornfield_0", cardName);
            Parts.Add(_cornfieldPart1);
        }

        public override void ConnectField(Field field)
        {
            Field = field;

            // дорога
            var side3 = RotateSide(Side.bottom, RotationsCount);
            var roadBorder3 = new Border(this.Field, this.Field?.GetNeighbour(side3), this);
            _roadPart3.Borders.Add(roadBorder3);


            // церковь
            _churchPart.ChurchField = field;

            // поле
            var side1 = RotateSide(Side.top, RotationsCount);
            var cornfieldBorder1 = new Border(this.Field, this.Field?.GetNeighbour(side1), this);
            _cornfieldPart1.Borders.Add(cornfieldBorder1);

            var side2 = RotateSide(Side.right, RotationsCount);
            var cornfieldBorder2 = new Border(this.Field, this.Field?.GetNeighbour(side2), this);
            _cornfieldPart1.Borders.Add(cornfieldBorder2);

            var side31 = RotateSide(Side.bottom, RotationsCount);
            var sidePart31 = RotateSidePart(CornfieldSide.side_3, RotationsCount);
            var cornfieldBorder31 = new Border(this.Field, this.Field?.GetNeighbour(side31), this);
            _cornfieldPart1.Borders.Add(cornfieldBorder31);
            cornfieldBorder31.CornfieldSide = sidePart31;

            var side32 = RotateSide(Side.bottom, RotationsCount);
            var sidePart32 = RotateSidePart(CornfieldSide.side_4, RotationsCount);
            var cornfieldBorder32 = new Border(this.Field, this.Field?.GetNeighbour(side32), this);
            _cornfieldPart1.Borders.Add(cornfieldBorder32);
            cornfieldBorder32.CornfieldSide = sidePart32;

            var side4 = RotateSide(Side.left, RotationsCount);
            var cornfieldBorder4 = new Border(this.Field, this.Field?.GetNeighbour(side4), this);
            _cornfieldPart1.Borders.Add(cornfieldBorder4);
        }
    }
}