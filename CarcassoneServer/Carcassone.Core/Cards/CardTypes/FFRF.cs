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
        protected string roadPartName = "Road_0";
        protected string churchPartName = "Church_0";
        protected string cornfieldPartName = "Cornfield_0";

        public FFRF(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var roadPart3 = new RoadPart(roadPartName, Id);
            Parts.Add(roadPart3);

            var churchPart = new ChurchPart(churchPartName, Id);
            Parts.Add(churchPart);

            var cornfieldPart1 = new CornfieldPart(cornfieldPartName, Id);
            Parts.Add(cornfieldPart1);
        }

        public override void ConnectField(Field field, FieldBoard fieldBoard)
        {
            AddBorderToPart(field, FieldSide.bottom, GetPart(roadPartName), fieldBoard);

            // церковь
            ((ChurchPart)GetPart(churchPartName)).ChurchFieldId = field.Id;

            AddBorderToPart(field, FieldSide.top, GetPart(cornfieldPartName), fieldBoard);
            AddBorderToPart(field, FieldSide.right, GetPart(cornfieldPartName), fieldBoard);
            AddBorderToPart(field, FieldSide.left, GetPart(cornfieldPartName), fieldBoard);

            var side31 = RotateSide(FieldSide.bottom, RotationsCount);
            var sidePart31 = RotateSidePart(CornfieldSide.side_3, RotationsCount);
            var cornfieldBorder31 = new Border(field, fieldBoard.GetNeighbour(field, side31), this);
            GetPart(cornfieldPartName).Borders.Add(cornfieldBorder31);
            cornfieldBorder31.CornfieldSide = sidePart31;

            var side32 = RotateSide(FieldSide.bottom, RotationsCount);
            var sidePart32 = RotateSidePart(CornfieldSide.side_4, RotationsCount);
            var cornfieldBorder32 = new Border(field, fieldBoard.GetNeighbour(field, side32), this);
            GetPart(cornfieldPartName).Borders.Add(cornfieldBorder32);
            cornfieldBorder32.CornfieldSide = sidePart32;
        }
    }
}