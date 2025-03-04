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
        protected string castlePartName = "Castle_0";
        protected string roadPart0Name = "Road_0";
        protected string roadPart1Name = "Road_1";
        protected string roadPart2Name = "Road_2";
        protected string cornfieldPart0Name = "Cornfield_0";
        protected string cornfieldPart1Name = "Cornfield_1";
        protected string cornfieldPart2Name = "Cornfield_2";

        public CRRR(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var castlePart = new CastlePart(castlePartName, Id);
            Parts.Add(castlePart);

            var roadPart0 = new RoadPart(roadPart0Name, Id);
            Parts.Add(roadPart0);

            var roadPart1 = new RoadPart(roadPart1Name, Id);
            Parts.Add(roadPart1);

            var roadPart2 = new RoadPart(roadPart2Name, Id);
            Parts.Add(roadPart2);

            var cornfieldPart0 = new CornfieldPart(cornfieldPart0Name, Id);
            Parts.Add(cornfieldPart0);

            var cornfieldPart1 = new CornfieldPart(cornfieldPart1Name, Id);
            Parts.Add(cornfieldPart1);

            var cornfieldPart2 = new CornfieldPart(cornfieldPart2Name, Id);
            Parts.Add(cornfieldPart2);


            FieldToCastleParts.Add(cornfieldPart0.PartId, new List<string>() { castlePart.PartId });
        }

        public override void ConnectField(Field field, FieldBoard fieldBoard)
        {
            AddBorderToPart(field, FieldSide.top, GetPart(castlePartName), fieldBoard);

            AddBorderToPart(field, FieldSide.right, GetPart(roadPart0Name), fieldBoard);

            AddBorderToPart(field, FieldSide.bottom, GetPart(roadPart1Name), fieldBoard);

            AddBorderToPart(field, FieldSide.left, GetPart(roadPart2Name), fieldBoard);

            AddCornfieldSplittedBorder(field, FieldSide.right, CornfieldSide.side_1, GetPart(cornfieldPart0Name), fieldBoard);
            AddCornfieldSplittedBorder(field, FieldSide.left, CornfieldSide.side_6, GetPart(cornfieldPart0Name), fieldBoard);

            AddCornfieldSplittedBorder(field, FieldSide.right, CornfieldSide.side_2, GetPart(cornfieldPart1Name), fieldBoard);
            AddCornfieldSplittedBorder(field, FieldSide.bottom, CornfieldSide.side_3, GetPart(cornfieldPart1Name), fieldBoard);

            AddCornfieldSplittedBorder(field, FieldSide.bottom, CornfieldSide.side_4, GetPart(cornfieldPart2Name), fieldBoard);
            AddCornfieldSplittedBorder(field, FieldSide.left, CornfieldSide.side_5, GetPart(cornfieldPart2Name), fieldBoard);
        }
    }
}