using Carcassone.Core.Board;
using System.Collections.Generic;

namespace Carcassone.Core.Tiles
{

    /// <summary>
    ///       C
    ///   |       |
    /// C | _____ | C
    ///   |/  |  \|
    ///       R
    /// </summary>
    public class CCRC : Tile
    {
        protected string castlePartName = "Castle_0";
        protected string roadPartName = "Road_0";
        protected string cornfieldPartName = "Cornfield_0";
        protected string cornfieldPart1Name = "Cornfield_1";

        public CCRC(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var castlePart = new CityPart(castlePartName, Id);
            Parts.Add(castlePart);

            var roadPart = new RoadPart(roadPartName, Id);
            Parts.Add(roadPart);

            var cornfieldPart1 = new FieldPart(cornfieldPartName, Id);
            Parts.Add(cornfieldPart1);

            var cornfieldPart2 = new FieldPart(cornfieldPart1Name, Id);
            Parts.Add(cornfieldPart2);


            FieldToCastleParts.Add(cornfieldPart1.PartId, new List<string>() { castlePart.PartId });
            FieldToCastleParts.Add(cornfieldPart2.PartId, new List<string>() { castlePart.PartId });
        }

        public override void ConnectField(Cell field, Grid grid)
        {
            AddBorderToPart(field, CellSide.top, GetPart(castlePartName), grid);
            AddBorderToPart(field, CellSide.right, GetPart(castlePartName), grid);
            AddBorderToPart(field, CellSide.left, GetPart(castlePartName), grid);

            AddBorderToPart(field, CellSide.bottom, GetPart(roadPartName), grid);

            AddCornfieldSplittedBorder(field, CellSide.bottom, CornfieldSide.side_3, GetPart(cornfieldPartName), grid);
            AddCornfieldSplittedBorder(field, CellSide.bottom, CornfieldSide.side_4, GetPart(cornfieldPart1Name), grid);
        }
    }
}