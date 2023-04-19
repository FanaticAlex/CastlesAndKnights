
using Carcassone.Core.Fields;
using System.Collections.Generic;

namespace Carcassone.Core.Cards
{

    /// <summary>
    ///       C
    ///   |\_____/|
    /// R |__     | F
    ///   |   \   |
    ///       R
    /// </summary>
    public class CFRR : Card
    {
        protected string castlePartName = "Castle_0";
        protected string roadPartName = "Road_0";
        protected string cornfieldPartName = "Cornfield_0";
        protected string cornfieldPart1Name = "Cornfield_1";

        public CFRR(string cardType, int cardNumber) : base(cardType, cardNumber)
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

            AddBorderToPart(field, Side.bottom, GetPart(roadPartName), fieldBoard);
            AddBorderToPart(field, Side.left, GetPart(roadPartName), fieldBoard);


            // поле
            var side2 = RotateSide(Side.right, RotationsCount);
            var cornfieldBorder2 = new Border(field, fieldBoard.GetNeighbour(field, side2), this);
            GetPart(cornfieldPartName).Borders.Add(cornfieldBorder2);

            var side31 = RotateSide(Side.bottom, RotationsCount);
            var sidePart31 = RotateSidePart(CornfieldSide.side_3, RotationsCount);
            var cornfieldBorder31 = new Border(field, fieldBoard.GetNeighbour(field, side31), this);
            GetPart(cornfieldPartName).Borders.Add(cornfieldBorder31);
            cornfieldBorder31.CornfieldSide = sidePart31;

            var side42 = RotateSide(Side.left, RotationsCount);
            var sidePart42 = RotateSidePart(CornfieldSide.side_6, RotationsCount);
            var cornfieldBorder42 = new Border(field, fieldBoard.GetNeighbour(field, side42), this);
            GetPart(cornfieldPartName).Borders.Add(cornfieldBorder42);
            cornfieldBorder42.CornfieldSide = sidePart42;


            // поле
            var side32 = RotateSide(Side.bottom, RotationsCount);
            var sidePart32 = RotateSidePart(CornfieldSide.side_4, RotationsCount);
            var cornfieldBorder32 = new Border(field, fieldBoard.GetNeighbour(field, side32), this);
            GetPart(cornfieldPart1Name).Borders.Add(cornfieldBorder32);
            cornfieldBorder32.CornfieldSide = sidePart32;

            var side41 = RotateSide(Side.left, RotationsCount);
            var sidePart41 = RotateSidePart(CornfieldSide.side_5, RotationsCount);
            var cornfieldBorder41 = new Border(field, fieldBoard.GetNeighbour(field, side41), this);
            GetPart(cornfieldPart1Name).Borders.Add(cornfieldBorder41);
            cornfieldBorder41.CornfieldSide = sidePart41;
        }
    }
}