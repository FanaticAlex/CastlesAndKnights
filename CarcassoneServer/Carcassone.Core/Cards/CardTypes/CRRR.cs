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


            // поле 1
            var side21 = RotateSide(FieldSide.right, RotationsCount);
            var sidePart21 = RotateSidePart(CornfieldSide.side_1, RotationsCount);
            var cornfieldBorder21 = new CardBorder(field, fieldBoard.GetNeighbour(field, side21), this);
            GetPart(cornfieldPart0Name).Borders.Add(cornfieldBorder21);
            cornfieldBorder21.CornfieldSide = sidePart21;

            var side42 = RotateSide(FieldSide.left, RotationsCount);
            var sidePart42 = RotateSidePart(CornfieldSide.side_6, RotationsCount);
            var cornfieldBorder42 = new CardBorder(field, fieldBoard.GetNeighbour(field, side42), this);
            GetPart(cornfieldPart0Name).Borders.Add(cornfieldBorder42);
            cornfieldBorder42.CornfieldSide = sidePart42;


            // поле 2
            var side22 = RotateSide(FieldSide.right, RotationsCount);
            var sidePart22 = RotateSidePart(CornfieldSide.side_2, RotationsCount);
            var cornfieldBorder22 = new CardBorder(field, fieldBoard.GetNeighbour(field, side22), this);
            GetPart(cornfieldPart1Name).Borders.Add(cornfieldBorder22);
            cornfieldBorder22.CornfieldSide = sidePart22;

            var side31 = RotateSide(FieldSide.bottom, RotationsCount);
            var sidePart31 = RotateSidePart(CornfieldSide.side_3, RotationsCount);
            var cornfieldBorder31 = new CardBorder(field, fieldBoard.GetNeighbour(field, side31), this);
            GetPart(cornfieldPart1Name).Borders.Add(cornfieldBorder31);
            cornfieldBorder31.CornfieldSide = sidePart31;


            // поле 3
            var side32 = RotateSide(FieldSide.bottom, RotationsCount);
            var sidePart32 = RotateSidePart(CornfieldSide.side_4, RotationsCount);
            var cornfieldBorder32 = new CardBorder(field, fieldBoard.GetNeighbour(field, side32), this);
            GetPart(cornfieldPart2Name).Borders.Add(cornfieldBorder32);
            cornfieldBorder32.CornfieldSide = sidePart32;

            var side41 = RotateSide(FieldSide.left, RotationsCount);
            var sidePart41 = RotateSidePart(CornfieldSide.side_5, RotationsCount);
            var cornfieldBorder41 = new CardBorder(field, fieldBoard.GetNeighbour(field, side41), this);
            GetPart(cornfieldPart2Name).Borders.Add(cornfieldBorder41);
            cornfieldBorder41.CornfieldSide = sidePart41;
        }
    }
}