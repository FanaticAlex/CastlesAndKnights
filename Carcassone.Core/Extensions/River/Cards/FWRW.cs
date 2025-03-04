using Carcassone.Core.Cards;
using Carcassone.Core.Fields;

namespace Carcassone.Core.Extensions.River.Cards
{

    /// <summary>
    ///       F
    ///   |       |
    /// W |+++O+++| W
    ///   |   |   |
    ///       R
    /// </summary>
    public class FWRW : Card
    {
        protected string churchPartName = "Church_0";
        protected string cornfieldPart0Name = "Cornfield_0";
        protected string cornfieldPart1Name = "Cornfield_1";
        protected string cornfieldPart2Name = "Cornfield_2";
        protected string roadPartName = "Road_0";

        public FWRW(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var churchPart = new ChurchPart(churchPartName, Id);
            Parts.Add(churchPart);

            var cornfieldPart1 = new CornfieldPart(cornfieldPart0Name, Id);
            Parts.Add(cornfieldPart1);

            var cornfieldPart2 = new CornfieldPart(cornfieldPart1Name, Id);
            Parts.Add(cornfieldPart2);

            var roadPart = new RoadPart(roadPartName, Id);
            Parts.Add(roadPart);

            var cornfieldPart3 = new CornfieldPart(cornfieldPart2Name, Id);
            Parts.Add(cornfieldPart3);
        }

        public override void ConnectField(Field field, FieldBoard fieldBoard)
        {
            ((ChurchPart)GetPart(churchPartName)).ChurchFieldId = field.Id;

            AddBorderToPart(field, FieldSide.top, GetPart(cornfieldPart0Name), fieldBoard);
            AddCornfieldSplittedBorder(field, FieldSide.right, CornfieldSide.side_1, GetPart(cornfieldPart0Name), fieldBoard);
            AddCornfieldSplittedBorder(field, FieldSide.left, CornfieldSide.side_6, GetPart(cornfieldPart0Name), fieldBoard);

            AddCornfieldSplittedBorder(field, FieldSide.right, CornfieldSide.side_2, GetPart(cornfieldPart1Name), fieldBoard);
            AddCornfieldSplittedBorder(field, FieldSide.bottom, CornfieldSide.side_3, GetPart(cornfieldPart1Name), fieldBoard);

            AddBorderToPart(field, FieldSide.bottom, GetPart(roadPartName), fieldBoard);

            AddCornfieldSplittedBorder(field, FieldSide.bottom, CornfieldSide.side_4, GetPart(cornfieldPart2Name), fieldBoard);
            AddCornfieldSplittedBorder(field, FieldSide.left, CornfieldSide.side_5, GetPart(cornfieldPart2Name), fieldBoard);
        }
    }
}