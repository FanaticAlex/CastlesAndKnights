using Carcassone.Core.Fields;

namespace Carcassone.Core.Cards
{

    /// <summary>
    ///       F
    ///   |       |
    /// R |_______| R
    ///   |   |   |
    ///       R
    /// </summary>
    public class FRRR : Card
    {
        protected string roadPartName = "Road_0";
        protected string roadPart1Name = "Road_1";
        protected string roadPart2Name = "Road_2";
        protected string cornfieldPartName = "Cornfield_0";
        protected string cornfieldPart1Name = "Cornfield_1";
        protected string cornfieldPart2Name = "Cornfield_2";

        public FRRR(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var roadPart1 = new RoadPart(roadPartName, Id);
            Parts.Add(roadPart1);

            var roadPart3 = new RoadPart(roadPart1Name, Id);
            Parts.Add(roadPart3);

            var roadPart4 = new RoadPart(roadPart2Name, Id);
            Parts.Add(roadPart4);

            var cornfieldPart1 = new CornfieldPart(cornfieldPartName, Id);
            Parts.Add(cornfieldPart1);

            var cornfieldPart2 = new CornfieldPart(cornfieldPart1Name, Id);
            Parts.Add(cornfieldPart2);

            var cornfieldPart3 = new CornfieldPart(cornfieldPart2Name, Id);
            Parts.Add(cornfieldPart3);
        }

        public override void ConnectField(Field field, FieldBoard fieldBoard)
        {
            AddBorderToPart(field, FieldSide.right, GetPart(roadPartName), fieldBoard);

            AddBorderToPart(field, FieldSide.bottom, GetPart(roadPart1Name), fieldBoard);

            AddBorderToPart(field, FieldSide.left, GetPart(roadPart2Name), fieldBoard);

            AddCornfieldSplittedBorder(field, FieldSide.right, CornfieldSide.side_1, GetPart(cornfieldPartName), fieldBoard);
            AddBorderToPart(field, FieldSide.top, GetPart(cornfieldPartName), fieldBoard);
            AddCornfieldSplittedBorder(field, FieldSide.left, CornfieldSide.side_6, GetPart(cornfieldPartName), fieldBoard);

            AddCornfieldSplittedBorder(field, FieldSide.right, CornfieldSide.side_2, GetPart(cornfieldPart1Name), fieldBoard);
            AddCornfieldSplittedBorder(field, FieldSide.bottom, CornfieldSide.side_3, GetPart(cornfieldPart1Name), fieldBoard);

            AddCornfieldSplittedBorder(field, FieldSide.bottom, CornfieldSide.side_4, GetPart(cornfieldPart2Name), fieldBoard);
            AddCornfieldSplittedBorder(field, FieldSide.left, CornfieldSide.side_5, GetPart(cornfieldPart2Name), fieldBoard);
        }
    }
}