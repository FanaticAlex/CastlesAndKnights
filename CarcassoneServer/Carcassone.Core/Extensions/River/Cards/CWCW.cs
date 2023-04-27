using Carcassone.Core.Cards;
using Carcassone.Core.Fields;
using System.Collections.Generic;

namespace Carcassone.Core.Extensions.River.Cards
{

    /// <summary>
    ///       C
    ///   |\-----/|
    /// W |+++++++| W
    ///   |/-----\|
    ///       C
    /// </summary>
    public class CWCW : Card
    {
        protected string castlePart0Name = "Castle_0";
        protected string castlePart1Name = "Castle_1";
        protected string cornfieldPart0Name = "Cornfield_0";
        protected string cornfieldPart1Name = "Cornfield_1";

        public CWCW(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var castlePart0 = new CastlePart(castlePart0Name, Id);
            Parts.Add(castlePart0);

            var castlePart1 = new CastlePart(castlePart1Name, Id);
            Parts.Add(castlePart1);

            var cornfieldPart0 = new CornfieldPart(cornfieldPart0Name, Id);
            Parts.Add(cornfieldPart0);

            var cornfieldPart1 = new CornfieldPart(cornfieldPart1Name, Id);
            Parts.Add(cornfieldPart1);


            FieldToCastleParts.Add(cornfieldPart0.PartId, new List<string>() { castlePart0.PartId });
            FieldToCastleParts.Add(cornfieldPart1.PartId, new List<string>() { castlePart1.PartId });
        }

        public override void ConnectField(Field field, FieldBoard fieldBoard)
        {
            AddBorderToPart(field, FieldSide.top, GetPart(castlePart0Name), fieldBoard);

            AddBorderToPart(field, FieldSide.bottom, GetPart(castlePart1Name), fieldBoard);

            // поле 1
            var cornfield1Side1 = FieldSide.right;
            cornfield1Side1 = RotateSide(cornfield1Side1, RotationsCount);
            var cornfield1sidePart1 = CornfieldSide.side_1;
            cornfield1sidePart1 = RotateSidePart(cornfield1sidePart1, RotationsCount);
            var cornfield1Border1 = new Border(field, fieldBoard.GetNeighbour(field, cornfield1Side1), this);
            cornfield1Border1.CornfieldSide = cornfield1sidePart1;
            GetPart(cornfieldPart0Name).Borders.Add(cornfield1Border1);

            var cornfield1Side2 = FieldSide.left;
            cornfield1Side2 = RotateSide(cornfield1Side2, RotationsCount);
            var cornfield1SidePart2 = CornfieldSide.side_6;
            cornfield1SidePart2 = RotateSidePart(cornfield1SidePart2, RotationsCount);
            var cornfield1Border2 = new Border(field, fieldBoard.GetNeighbour(field, cornfield1Side2), this);
            cornfield1Border2.CornfieldSide = cornfield1SidePart2;
            GetPart(cornfieldPart0Name).Borders.Add(cornfield1Border2);


            // поле 2
            var cornfield2Side1 = FieldSide.right;
            cornfield2Side1 = RotateSide(cornfield2Side1, RotationsCount);
            var cornfield2SidePart1 = CornfieldSide.side_2;
            cornfield2SidePart1 = RotateSidePart(cornfield2SidePart1, RotationsCount);
            var cornfield2Border1 = new Border(field, fieldBoard.GetNeighbour(field, cornfield2Side1), this);
            cornfield2Border1.CornfieldSide = cornfield2SidePart1;
            GetPart(cornfieldPart1Name).Borders.Add(cornfield2Border1);

            var cornfield2Side2 = FieldSide.left;
            cornfield2Side2 = RotateSide(cornfield2Side2, RotationsCount);
            var cornfield2SidePart2 = CornfieldSide.side_5;
            cornfield2SidePart2 = RotateSidePart(cornfield2SidePart2, RotationsCount);
            var cornfield2Border2 = new Border(field, fieldBoard.GetNeighbour(field, cornfield2Side2), this);
            cornfield2Border2.CornfieldSide = cornfield2SidePart2;
            GetPart(cornfieldPart1Name).Borders.Add(cornfield2Border2);
        }
    }
}