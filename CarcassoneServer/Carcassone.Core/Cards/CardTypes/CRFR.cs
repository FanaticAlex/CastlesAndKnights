
using Carcassone.Core.Fields;
using System.Collections.Generic;

namespace Carcassone.Core.Cards
{

    /// <summary>
    ///       C
    ///   |\_____/|
    /// R |_______| R
    ///   |       |
    ///       F
    /// </summary>
    public class CRFR : Card
    {
        protected string castlePartName = "Castle_0";
        protected string roadPartName = "Road_0";
        protected string cornfieldPartName = "Cornfield_0";
        protected string cornfieldPart1Name = "Cornfield_1";

        public CRFR(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var castlePart = new CastlePart(castlePartName, CardId);
            Parts.Add(castlePart);

            var roadPart = new RoadPart(roadPartName, CardId);
            Parts.Add(roadPart);

            var cornfieldPart1 = new CornfieldPart(cornfieldPartName, CardId);
            Parts.Add(cornfieldPart1);

            var cornfieldPart2 = new CornfieldPart(cornfieldPart1Name, CardId);
            Parts.Add(cornfieldPart2);


            FieldToCastleParts.Add(cornfieldPart1.PartId, new List<string>() { castlePart.PartId });
        }

        public override void ConnectField(Field field, FieldBoard fieldBoard)
        {
            AddBorderToPart(field, Side.top, GetPart(castlePartName), fieldBoard);

            AddBorderToPart(field, Side.left, GetPart(roadPartName), fieldBoard);
            AddBorderToPart(field, Side.right, GetPart(roadPartName), fieldBoard);

            // поле 1
            var side21 = RotateSide(Side.right, RotationsCount);
            var sidePart21 = RotateSidePart(CornfieldSide.side_1, RotationsCount);
            var cornfieldBorder21 = new Border(field, fieldBoard.GetNeighbour(field, side21), this);
            GetPart(cornfieldPartName).Borders.Add(cornfieldBorder21);
            cornfieldBorder21.CornfieldSide = sidePart21;

            var side42 = RotateSide(Side.left, RotationsCount);
            var sidePart42 = RotateSidePart(CornfieldSide.side_6, RotationsCount);
            var cornfieldBorder42 = new Border(field, fieldBoard.GetNeighbour(field, side42), this);
            GetPart(cornfieldPartName).Borders.Add(cornfieldBorder42);
            cornfieldBorder42.CornfieldSide = sidePart42;


            // поле 2
            var side22 = RotateSide(Side.right, RotationsCount);
            var sidePart22 = RotateSidePart(CornfieldSide.side_2, RotationsCount);
            var cornfieldBorder22 = new Border(field, fieldBoard.GetNeighbour(field, side22), this);
            GetPart(cornfieldPart1Name).Borders.Add(cornfieldBorder22);
            cornfieldBorder22.CornfieldSide = sidePart22;

            var side3 = RotateSide(Side.bottom, RotationsCount);
            var cornfieldBorder3 = new Border(field, fieldBoard.GetNeighbour(field, side3), this);
            GetPart(cornfieldPart1Name).Borders.Add(cornfieldBorder3);

            var side41 = RotateSide(Side.left, RotationsCount);
            var sidePart41 = RotateSidePart(CornfieldSide.side_5, RotationsCount);
            var cornfieldBorder41 = new Border(field, fieldBoard.GetNeighbour(field, side41), this);
            GetPart(cornfieldPart1Name).Borders.Add(cornfieldBorder41);
            cornfieldBorder41.CornfieldSide = sidePart41;
        }
    }
}