using Carcassone.Core.Board;
using System.Collections.Generic;

namespace Carcassone.Core.Tiles
{

    /// <summary>
    ///       C
    ///   |\_____/|
    /// R |_______| R
    ///   |   |   |
    ///       R
    /// </summary>
    public class CRRR : Tile
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
            var castlePart = new CityPart(castlePartName, Id);
            Parts.Add(castlePart);

            var roadPart0 = new RoadPart(roadPart0Name, Id);
            Parts.Add(roadPart0);

            var roadPart1 = new RoadPart(roadPart1Name, Id);
            Parts.Add(roadPart1);

            var roadPart2 = new RoadPart(roadPart2Name, Id);
            Parts.Add(roadPart2);

            var cornfieldPart0 = new FieldPart(cornfieldPart0Name, Id);
            Parts.Add(cornfieldPart0);

            var cornfieldPart1 = new FieldPart(cornfieldPart1Name, Id);
            Parts.Add(cornfieldPart1);

            var cornfieldPart2 = new FieldPart(cornfieldPart2Name, Id);
            Parts.Add(cornfieldPart2);


            FieldToCastleParts.Add(cornfieldPart0.PartId, new List<string>() { castlePart.PartId });
        }

        public override void ConnectField(Cell field, Grid grid)
        {
            AddBorderToPart(field, CellSide.top, GetPart(castlePartName), grid);

            AddBorderToPart(field, CellSide.right, GetPart(roadPart0Name), grid);

            AddBorderToPart(field, CellSide.bottom, GetPart(roadPart1Name), grid);

            AddBorderToPart(field, CellSide.left, GetPart(roadPart2Name), grid);

            AddCornfieldSplittedBorder(field, CellSide.right, CornfieldSide.side_1, GetPart(cornfieldPart0Name), grid);
            AddCornfieldSplittedBorder(field, CellSide.left, CornfieldSide.side_6, GetPart(cornfieldPart0Name), grid);

            AddCornfieldSplittedBorder(field, CellSide.right, CornfieldSide.side_2, GetPart(cornfieldPart1Name), grid);
            AddCornfieldSplittedBorder(field, CellSide.bottom, CornfieldSide.side_3, GetPart(cornfieldPart1Name), grid);

            AddCornfieldSplittedBorder(field, CellSide.bottom, CornfieldSide.side_4, GetPart(cornfieldPart2Name), grid);
            AddCornfieldSplittedBorder(field, CellSide.left, CornfieldSide.side_5, GetPart(cornfieldPart2Name), grid);
        }
    }
}