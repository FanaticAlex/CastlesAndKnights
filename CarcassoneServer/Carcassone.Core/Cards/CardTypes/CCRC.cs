using Carcassone.Core.Fields;
using System.Collections.Generic;

namespace Carcassone.Core.Cards
{

    /// <summary>
    ///       C
    ///   |       |
    /// C | _____ | C
    ///   |/  |  \|
    ///       R
    /// </summary>
    public class CCRC : Card
    {
        protected string castlePartName = "Castle_0";
        protected string roadPartName = "Road_0";
        protected string cornfieldPartName = "Cornfield_0";
        protected string cornfieldPart1Name = "Cornfield_1";

        public CCRC(string cardType, int cardNumber) : base(cardType, cardNumber)
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
            FieldToCastleParts.Add(cornfieldPart2.PartId, new List<string>() { castlePart.PartId });
        }

        public override void ConnectField(Field field, FieldBoard fieldBoard)
        {
            //замок
            AddBorderToPart(field, FieldSide.top, GetPart(castlePartName), fieldBoard);
            AddBorderToPart(field, FieldSide.right, GetPart(castlePartName), fieldBoard);
            AddBorderToPart(field, FieldSide.left, GetPart(castlePartName), fieldBoard);

            // дорога
            AddBorderToPart(field, FieldSide.bottom, GetPart(roadPartName), fieldBoard);

            // поле 1
            var side31 = FieldSide.bottom;
            side31 = RotateSide(side31, RotationsCount);
            var sidePart31 = CornfieldSide.side_3;
            sidePart31 = RotateSidePart(sidePart31, RotationsCount);
            var cornfieldBorder31 = new Border(field, fieldBoard.GetNeighbour(field, side31), this);
            cornfieldBorder31.CornfieldSide = sidePart31;
            GetPart(cornfieldPartName).Borders.Add(cornfieldBorder31);


            // поле 2
            var side32 = FieldSide.bottom;
            side32 = RotateSide(side32, RotationsCount);
            var sidePart32 = CornfieldSide.side_4;
            sidePart32 = RotateSidePart(sidePart32, RotationsCount);
            var cornfieldBorder32 = new Border(field, fieldBoard.GetNeighbour(field, side32), this);
            cornfieldBorder32.CornfieldSide = sidePart32;
            GetPart(cornfieldPart1Name).Borders.Add(cornfieldBorder32);
        }
    }
}