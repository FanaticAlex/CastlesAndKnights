using Carcassone.Core.Board;
using Carcassone.Core.Tiles;
using System.Collections.Generic;
using Carcassone.Core.Calculation.Base.Cities;
using Carcassone.Core.Calculation.Base.Farms;

namespace Carcassone.Core.Calculation.Base.Tiles
{

    /// <summary>
    ///       F
    ///   |\     /|
    /// C | |   | | C
    ///   |/     \|
    ///       F
    /// </summary>
    public class FCFC : Tile
    {
        protected string castlePart0Name = "Castle_0";
        protected string castlePart1Name = "Castle_1";
        protected string cornfieldPartName = "Cornfield_0";

        public FCFC(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var castlePart0 = new CityPart(castlePart0Name, Id);
            Parts.Add(castlePart0);

            var castlePart1 = new CityPart(castlePart1Name, Id);
            Parts.Add(castlePart1);

            var cornfieldPart1 = new FieldPart(cornfieldPartName, Id);
            Parts.Add(cornfieldPart1);


            FieldToCastleParts.Add(cornfieldPart1.PartId, new List<string>() { castlePart0.PartId, castlePart1.PartId });
        }

        public override void ConnectField(Cell field, Grid grid)
        {
            AddBorderToPart(field, CellSide.left, GetPart(castlePart0Name), grid);

            AddBorderToPart(field, CellSide.right, GetPart(castlePart1Name), grid);

            AddBorderToPart(field, CellSide.top, GetPart(cornfieldPartName), grid);
            AddBorderToPart(field, CellSide.bottom, GetPart(cornfieldPartName), grid);
        }
    }
}