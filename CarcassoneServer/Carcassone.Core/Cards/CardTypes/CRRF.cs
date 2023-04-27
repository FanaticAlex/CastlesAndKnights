using Carcassone.Core.Fields;
using System.Collections.Generic;

namespace Carcassone.Core.Cards
{

    /// <summary>
    ///       C
    ///   |\_____/|
    /// F |      _| R
    ///   |    /  |
    ///       R
    /// </summary>
    public class CRRF : Card
    {
        protected string castlePartName = "Castle_0";
        protected string roadPartName = "Road_0";
        protected string cornfieldPartName = "Cornfield_0";
        protected string cornfieldPart1Name = "Cornfield_1";

        public CRRF(string cardType, int cardNumber) : base(cardType, cardNumber)
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
            AddBorderToPart(field, FieldSide.top, GetPart(castlePartName), fieldBoard);

            AddBorderToPart(field, FieldSide.right, GetPart(roadPartName), fieldBoard);
            AddBorderToPart(field, FieldSide.bottom, GetPart(roadPartName), fieldBoard);


            // поле 1
            var side21 = RotateSide(FieldSide.right, RotationsCount);
            var sidePart21 = RotateSidePart(CornfieldSide.side_1, RotationsCount);
            var cornfieldBorder21 = new Border(field, fieldBoard.GetNeighbour(field, side21), this);
            GetPart(cornfieldPartName).Borders.Add(cornfieldBorder21);
            cornfieldBorder21.CornfieldSide = sidePart21;

            var side32 = RotateSide(FieldSide.bottom, RotationsCount);
            var sidePart32 = RotateSidePart(CornfieldSide.side_4, RotationsCount);
            var cornfieldBorder32 = new Border(field, fieldBoard.GetNeighbour(field, side32), this);
            GetPart(cornfieldPartName).Borders.Add(cornfieldBorder32);
            cornfieldBorder32.CornfieldSide = sidePart32;

            var side4 = RotateSide(FieldSide.left, RotationsCount);
            var cornfieldBorder4 = new Border(field, fieldBoard.GetNeighbour(field, side4), this);
            GetPart(cornfieldPartName).Borders.Add(cornfieldBorder4);


            // поле 2
            var side22 = RotateSide(FieldSide.right, RotationsCount);
            var sidePart22 = RotateSidePart(CornfieldSide.side_2, RotationsCount);
            var cornfieldBorder22 = new Border(field, fieldBoard.GetNeighbour(field, side22), this);
            GetPart(cornfieldPart1Name).Borders.Add(cornfieldBorder22);
            cornfieldBorder22.CornfieldSide = sidePart22;

            var side31 = RotateSide(FieldSide.bottom, RotationsCount);
            var sidePart31 = RotateSidePart(CornfieldSide.side_3, RotationsCount);
            var cornfieldBorder31 = new Border(field, fieldBoard.GetNeighbour(field, side31), this);
            GetPart(cornfieldPart1Name).Borders.Add(cornfieldBorder31);
            cornfieldBorder31.CornfieldSide = sidePart31;
        }
    }
}