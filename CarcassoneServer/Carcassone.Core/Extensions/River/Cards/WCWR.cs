using Carcassone.Core.Cards;
using Carcassone.Core.Fields;
using System.Collections.Generic;

namespace Carcassone.Core.Extensions.River.Cards
{
    /// <summary>
    ///       W
    ///   |   +  /|
    /// R |-----| | C
    ///   |   +  \|
    ///       W
    /// </summary>
    public class WCWR : Card
    {
        protected string castlePart0Name = "Castle_0";
        protected string cornfieldPart0Name = "Cornfield_0";
        protected string cornfieldPart1Name = "Cornfield_1";
        protected string cornfieldPart2Name = "Cornfield_2";
        protected string cornfieldPart3Name = "Cornfield_3";
        protected string roadPartName = "Road_0";

        public WCWR(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var castlePart0 = new CastlePart(castlePart0Name, Id);
            Parts.Add(castlePart0);

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

            FieldToCastleParts.Add(cornfieldPart0.PartId, new List<string>() { castlePart0.PartId });
            FieldToCastleParts.Add(cornfieldPart1.PartId, new List<string>() { castlePart0.PartId });
        }

        public override void ConnectField(Field field, FieldBoard fieldBoard)
        {
            // замок
            AddBorderToPart(field, FieldSide.right, GetPart(castlePart0Name), fieldBoard);

            // поле
            var cornfield1Side0 = FieldSide.top;
            cornfield1Side0 = RotateSide(cornfield1Side0, RotationsCount);
            var cornfield1sidePart0 = CornfieldSide.side_0;
            cornfield1sidePart0 = RotateSidePart(cornfield1sidePart0, RotationsCount);
            var cornfield1Border0 = new Border(field, fieldBoard.GetNeighbour(field, cornfield1Side0), this);
            cornfield1Border0.CornfieldSide = cornfield1sidePart0;
            GetPart(cornfieldPart0Name).Borders.Add(cornfield1Border0);

            // поле
            var cornfield2Side0 = FieldSide.bottom;
            cornfield2Side0 = RotateSide(cornfield2Side0, RotationsCount);
            var cornfield2sidePart0 = CornfieldSide.side_3;
            cornfield2sidePart0 = RotateSidePart(cornfield2sidePart0, RotationsCount);
            var cornfield2Border0 = new Border(field, fieldBoard.GetNeighbour(field, cornfield2Side0), this);
            cornfield2Border0.CornfieldSide = cornfield2sidePart0;
            GetPart(cornfieldPart1Name).Borders.Add(cornfield2Border0);

            // поле
            var cornfield3Side0 = FieldSide.bottom;
            cornfield3Side0 = RotateSide(cornfield3Side0, RotationsCount);
            var cornfield3sidePart0 = CornfieldSide.side_4;
            cornfield3sidePart0 = RotateSidePart(cornfield3sidePart0, RotationsCount);
            var cornfield3Border0 = new Border(field, fieldBoard.GetNeighbour(field, cornfield3Side0), this);
            cornfield3Border0.CornfieldSide = cornfield3sidePart0;
            GetPart(cornfieldPart2Name).Borders.Add(cornfield3Border0);

            var cornfield3Side1 = FieldSide.left;
            cornfield3Side1 = RotateSide(cornfield3Side1, RotationsCount);
            var cornfield3sidePart1 = CornfieldSide.side_5;
            cornfield3sidePart1 = RotateSidePart(cornfield3sidePart1, RotationsCount);
            var cornfield3Border1 = new Border(field, fieldBoard.GetNeighbour(field, cornfield3Side1), this);
            cornfield3Border1.CornfieldSide = cornfield3sidePart1;
            GetPart(cornfieldPart2Name).Borders.Add(cornfield3Border1);

            // поле
            var cornfield4Side0 = FieldSide.left;
            cornfield4Side0 = RotateSide(cornfield4Side0, RotationsCount);
            var cornfield4sidePart0 = CornfieldSide.side_6;
            cornfield4sidePart0 = RotateSidePart(cornfield4sidePart0, RotationsCount);
            var cornfield4Border0 = new Border(field, fieldBoard.GetNeighbour(field, cornfield4Side0), this);
            cornfield4Border0.CornfieldSide = cornfield4sidePart0;
            GetPart(cornfieldPart3Name).Borders.Add(cornfield4Border0);

            var cornfield4Side1 = FieldSide.top;
            cornfield4Side1 = RotateSide(cornfield4Side1, RotationsCount);
            var cornfield4sidePart1 = CornfieldSide.side_7;
            cornfield4sidePart1 = RotateSidePart(cornfield4sidePart1, RotationsCount);
            var cornfield4Border1 = new Border(field, fieldBoard.GetNeighbour(field, cornfield4Side1), this);
            cornfield4Border1.CornfieldSide = cornfield4sidePart1;
            GetPart(cornfieldPart3Name).Borders.Add(cornfield4Border1);


            // дорога
            AddBorderToPart(field, FieldSide.left, GetPart(roadPartName), fieldBoard);
        }
    }
}