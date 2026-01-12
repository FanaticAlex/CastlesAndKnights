using Carcassone.Core.Tiles;
using Carcassone.Core.Board;
using System.Collections.Generic;
using Carcassone.Core.Calculation.Base.Cities;
using Carcassone.Core.Calculation.Base.Farms;

namespace Carcassone.Core.Calculation.River.Tiles
{

    /// <summary>
    ///       C
    ///   |\-----/|
    /// W |+++++++| W
    ///   |/-----\|
    ///       C
    /// </summary>
    public class CWCW : Tile
    {
        protected string castlePart0Name = "Castle_0";
        protected string castlePart1Name = "Castle_1";
        protected string cornfieldPart0Name = "Cornfield_0";
        protected string cornfieldPart1Name = "Cornfield_1";

        public CWCW(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var castlePart0 = new CityPart(castlePart0Name, Id);
            Parts.Add(castlePart0);

            var castlePart1 = new CityPart(castlePart1Name, Id);
            Parts.Add(castlePart1);

            var cornfieldPart0 = new FieldPart(cornfieldPart0Name, Id);
            Parts.Add(cornfieldPart0);

            var cornfieldPart1 = new FieldPart(cornfieldPart1Name, Id);
            Parts.Add(cornfieldPart1);


            FieldToCastleParts.Add(cornfieldPart0.PartId, new List<string>() { castlePart0.PartId });
            FieldToCastleParts.Add(cornfieldPart1.PartId, new List<string>() { castlePart1.PartId });
        }

        public override void ConnectField(Cell field, Grid grid)
        {
            AddBorderToPart(field, CellSide.top, GetPart(castlePart0Name), grid);

            AddBorderToPart(field, CellSide.bottom, GetPart(castlePart1Name), grid);

            AddCornfieldSplittedBorder(field, CellSide.right, FieldSide.side_1, GetPart(cornfieldPart0Name), grid);
            AddCornfieldSplittedBorder(field, CellSide.left, FieldSide.side_6, GetPart(cornfieldPart0Name), grid);

            AddCornfieldSplittedBorder(field, CellSide.right, FieldSide.side_2, GetPart(cornfieldPart1Name), grid);
            AddCornfieldSplittedBorder(field, CellSide.left, FieldSide.side_5, GetPart(cornfieldPart1Name), grid);
        }
    }
}