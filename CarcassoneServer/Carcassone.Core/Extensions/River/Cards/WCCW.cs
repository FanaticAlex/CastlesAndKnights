using Carcassone.Core.Cards;
using Carcassone.Core.Fields;
using System.Collections.Generic;

namespace Carcassone.Core.Extensions.River.Cards
{
    /// <summary>
    ///       W
    ///   |   +  /|
    /// W |++ /   | C
    ///   |/      |
    ///       C
    /// </summary>
    public class WCCW : Card
    {
        protected string cornfieldPartName = "Cornfield_0";
        protected string cornfieldPart1Name = "Cornfield_1";
        protected string castlePart1Name = "Castle_0";

        public WCCW(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var cornfieldPart1 = new CornfieldPart(cornfieldPartName, Id);
            Parts.Add(cornfieldPart1);

            var cornfieldPart2 = new CornfieldPart(cornfieldPart1Name, Id);
            Parts.Add(cornfieldPart2);

            var castlePart1 = new CastlePart(castlePart1Name, Id);
            Parts.Add(castlePart1);

            FieldToCastleParts.Add(cornfieldPart1.PartId, new List<string>() { castlePart1.PartId });
        }

        public override void ConnectField(Field field, FieldBoard fieldBoard)
        {
            // замок
            AddBorderToPart(field, FieldSide.right, GetPart(castlePart1Name), fieldBoard);
            AddBorderToPart(field, FieldSide.bottom, GetPart(castlePart1Name), fieldBoard);

            
            // поле 1
            var cornfield1Side0 = FieldSide.top;
            cornfield1Side0 = RotateSide(cornfield1Side0, RotationsCount);
            var cornfield1sidePart0 = CornfieldSide.side_0;
            cornfield1sidePart0 = RotateSidePart(cornfield1sidePart0, RotationsCount);
            var cornfield1Border0 = new CardBorder(field, fieldBoard.GetNeighbour(field, cornfield1Side0), this);
            cornfield1Border0.CornfieldSide = cornfield1sidePart0;
            GetPart(cornfieldPartName).Borders.Add(cornfield1Border0);

            var cornfield1Side1 = FieldSide.left;
            cornfield1Side1 = RotateSide(cornfield1Side1, RotationsCount);
            var cornfield1sidePart1 = CornfieldSide.side_5;
            cornfield1sidePart1 = RotateSidePart(cornfield1sidePart1, RotationsCount);
            var cornfield1Border1 = new CardBorder(field, fieldBoard.GetNeighbour(field, cornfield1Side1), this);
            cornfield1Border1.CornfieldSide = cornfield1sidePart1;
            GetPart(cornfieldPartName).Borders.Add(cornfield1Border1);

            // поле 1
            var cornfield2Side0 = FieldSide.top;
            cornfield2Side0 = RotateSide(cornfield2Side0, RotationsCount);
            var cornfield2sidePart0 = CornfieldSide.side_7;
            cornfield2sidePart0 = RotateSidePart(cornfield2sidePart0, RotationsCount);
            var cornfield2Border0 = new CardBorder(field, fieldBoard.GetNeighbour(field, cornfield2Side0), this);
            cornfield2Border0.CornfieldSide = cornfield2sidePart0;
            GetPart(cornfieldPart1Name).Borders.Add(cornfield2Border0);

            var cornfield2Side1 = FieldSide.left;
            cornfield2Side1 = RotateSide(cornfield2Side1, RotationsCount);
            var cornfield2sidePart1 = CornfieldSide.side_6;
            cornfield2sidePart1 = RotateSidePart(cornfield2sidePart1, RotationsCount);
            var cornfield2Border1 = new CardBorder(field, fieldBoard.GetNeighbour(field, cornfield2Side1), this);
            cornfield2Border1.CornfieldSide = cornfield2sidePart1;
            GetPart(cornfieldPart1Name).Borders.Add(cornfield2Border1);
        }
    }
}