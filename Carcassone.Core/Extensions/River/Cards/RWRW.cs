using Carcassone.Core.Cards;
using Carcassone.Core.Fields;

namespace Carcassone.Core.Extensions.River.Cards
{
    /// <summary>
    ///       R
    ///   |   |   |
    /// W |+++++++| W
    ///   |   |   |
    ///       R
    /// </summary>
    public class RWRW : Card
    {
        protected string cornfieldPart0Name = "Cornfield_0";
        protected string cornfieldPart1Name = "Cornfield_1";
        protected string roadPartName = "Road_0";
        protected string cornfieldPart2Name = "Cornfield_2";
        protected string cornfieldPart3Name = "Cornfield_3";

        public RWRW(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var cornfieldPart0 = new CornfieldPart(cornfieldPart0Name, Id);
            Parts.Add(cornfieldPart0);

            var cornfieldPart1 = new CornfieldPart(cornfieldPart1Name, Id);
            Parts.Add(cornfieldPart1);

            var cornfieldPart2 = new CornfieldPart(cornfieldPart2Name, Id);
            Parts.Add(cornfieldPart2);

            var cornfieldPart3 = new CornfieldPart(cornfieldPart3Name, Id);
            Parts.Add(cornfieldPart3);

            var roadPart = new RoadPart(roadPartName, Id);
            Parts.Add(roadPart);
        }
        
        public override void ConnectField(Field field, FieldBoard fieldBoard)
        {
            AddCornfieldSplittedBorder(field, FieldSide.top, CornfieldSide.side_0, GetPart(cornfieldPart0Name), fieldBoard);
            AddCornfieldSplittedBorder(field, FieldSide.right, CornfieldSide.side_1, GetPart(cornfieldPart0Name), fieldBoard);

            AddCornfieldSplittedBorder(field, FieldSide.right, CornfieldSide.side_2, GetPart(cornfieldPart1Name), fieldBoard);
            AddCornfieldSplittedBorder(field, FieldSide.bottom, CornfieldSide.side_3, GetPart(cornfieldPart1Name), fieldBoard);

            AddCornfieldSplittedBorder(field, FieldSide.bottom, CornfieldSide.side_4, GetPart(cornfieldPart2Name), fieldBoard);
            AddCornfieldSplittedBorder(field, FieldSide.left, CornfieldSide.side_5, GetPart(cornfieldPart2Name), fieldBoard);

            AddCornfieldSplittedBorder(field, FieldSide.left, CornfieldSide.side_6, GetPart(cornfieldPart3Name), fieldBoard);
            AddCornfieldSplittedBorder(field, FieldSide.top, CornfieldSide.side_7, GetPart(cornfieldPart3Name), fieldBoard);

            // дорога
            AddBorderToPart(field, FieldSide.top, GetPart(roadPartName), fieldBoard);
            AddBorderToPart(field, FieldSide.bottom, GetPart(roadPartName), fieldBoard);
        }
    }
}