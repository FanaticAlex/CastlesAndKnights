using Carcassone.Core.Board;
using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.Base.Cities;
using Carcassone.Core.Calculation.Base.Roads;
using Carcassone.Core.Tiles;
using System.Collections.Generic;

namespace Carcassone.Core.Calculation.Base.Tiles
{

    /// <summary>
    ///       C
    ///   |\_____/|
    /// R |__     | F
    ///   |   \   |
    ///       R
    /// </summary>
    public class CFRR : Tile
    {
        protected string castlePartName = "Castle_0";
        protected string roadPartName = "Road_0";
        protected string cornfieldPartName = "Cornfield_0";
        protected string cornfieldPart1Name = "Cornfield_1";

        public CFRR(string cardType, int cardNumber) : base(cardType, cardNumber)
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
        }

        public override void ConnectField(Cell field, Grid grid)
        {
            AddBorderToPart(field, CellSide.top, GetPart(castlePartName), grid);

            AddBorderToPart(field, CellSide.bottom, GetPart(roadPartName), grid);
            AddBorderToPart(field, CellSide.left, GetPart(roadPartName), grid);

            AddBorderToPart(field, CellSide.right, GetPart(cornfieldPartName), grid);
            AddCornfieldSplittedBorder(field, CellSide.bottom, FieldSide.side_3, GetPart(cornfieldPartName), grid);
            AddCornfieldSplittedBorder(field, CellSide.left, FieldSide.side_6, GetPart(cornfieldPartName), grid);

            AddCornfieldSplittedBorder(field, CellSide.bottom, FieldSide.side_4, GetPart(cornfieldPart1Name), grid);
            AddCornfieldSplittedBorder(field, CellSide.left, FieldSide.side_5, GetPart(cornfieldPart1Name), grid);
        }
    }
}