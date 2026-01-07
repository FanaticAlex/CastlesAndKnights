using Carcassone.Core.Tiles;
using Carcassone.Core.Board;
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
    public class WCWR : Tile
    {
        protected string castlePart0Name = "Castle_0";
        protected string cornfieldPart0Name = "Cornfield_0";
        protected string cornfieldPart1Name = "Cornfield_1";
        protected string cornfieldPart2Name = "Cornfield_2";
        protected string cornfieldPart3Name = "Cornfield_3";
        protected string roadPartName = "Road_0";

        public WCWR(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var castlePart0 = new CityPart(castlePart0Name, Id);
            Parts.Add(castlePart0);

            var cornfieldPart0 = new FieldPart(cornfieldPart0Name, Id);
            Parts.Add(cornfieldPart0);

            var cornfieldPart1 = new FieldPart(cornfieldPart1Name, Id);
            Parts.Add(cornfieldPart1);

            var cornfieldPart2 = new FieldPart(cornfieldPart2Name, Id);
            Parts.Add(cornfieldPart2);

            var cornfieldPart3 = new FieldPart(cornfieldPart3Name, Id);
            Parts.Add(cornfieldPart3);

            var roadPart = new RoadPart(roadPartName, Id);
            Parts.Add(roadPart);

            FieldToCastleParts.Add(cornfieldPart0.PartId, new List<string>() { castlePart0.PartId });
            FieldToCastleParts.Add(cornfieldPart1.PartId, new List<string>() { castlePart0.PartId });
        }

        public override void ConnectField(Cell field, Grid grid)
        {
            AddBorderToPart(field, CellSide.right, GetPart(castlePart0Name), grid);

            AddCornfieldSplittedBorder(field, CellSide.top, CornfieldSide.side_0, GetPart(cornfieldPart0Name), grid);

            AddCornfieldSplittedBorder(field, CellSide.bottom, CornfieldSide.side_3, GetPart(cornfieldPart1Name), grid);

            AddCornfieldSplittedBorder(field, CellSide.bottom, CornfieldSide.side_4, GetPart(cornfieldPart2Name), grid);
            AddCornfieldSplittedBorder(field, CellSide.left, CornfieldSide.side_5, GetPart(cornfieldPart2Name), grid);

            AddCornfieldSplittedBorder(field, CellSide.left, CornfieldSide.side_6, GetPart(cornfieldPart3Name), grid);
            AddCornfieldSplittedBorder(field, CellSide.top, CornfieldSide.side_7, GetPart(cornfieldPart3Name), grid);

            AddBorderToPart(field, CellSide.left, GetPart(roadPartName), grid);
        }
    }
}