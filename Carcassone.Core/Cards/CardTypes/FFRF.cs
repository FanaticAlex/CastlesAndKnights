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

            ((ChurchPart)GetPart(churchPartName)).ChurchFieldId = field.Id;

            AddBorderToPart(field, FieldSide.top, GetPart(cornfieldPartName), fieldBoard);
            AddBorderToPart(field, FieldSide.right, GetPart(cornfieldPartName), fieldBoard);
            AddBorderToPart(field, FieldSide.left, GetPart(cornfieldPartName), fieldBoard);
            AddCornfieldSplittedBorder(field, FieldSide.bottom, CornfieldSide.side_3, GetPart(cornfieldPartName), fieldBoard);
            AddCornfieldSplittedBorder(field, FieldSide.bottom, CornfieldSide.side_4, GetPart(cornfieldPartName), fieldBoard);
        }
    }
}