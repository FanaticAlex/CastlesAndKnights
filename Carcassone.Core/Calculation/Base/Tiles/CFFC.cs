using Carcassone.Core.Board;
using Carcassone.Core.Tiles;
using System.Collections.Generic;
using Carcassone.Core.Calculation.Base.Cities;
using Carcassone.Core.Calculation.Base.Farms;

namespace Carcassone.Core.Calculation.Base.Tiles
{

    /// <summary>
    ///       C
    ///   |    __/|
    /// C | __/   | F
    ///   |/      |
    ///       F
    /// </summary>
    public class CFFC : Tile
    {
        protected string castlePartName = "Castle_0";
        protected string cornfieldPartName = "Cornfield_0";

        public CFFC(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var castlePart = new CityPart(castlePartName, Id);
            Parts.Add(castlePart);

            var cornfieldPart = new FieldPart(cornfieldPartName, Id);
            Parts.Add(cornfieldPart);
         
            FieldToCastleParts.Add(cornfieldPart.PartId, new List<string>() { castlePart.PartId });
        }

        public override void ConnectField(Cell field, Grid grid)
        {
            AddBorderToPart(field, CellSide.top, GetPart(castlePartName), grid);
            AddBorderToPart(field, CellSide.left, GetPart(castlePartName), grid);

            AddBorderToPart(field, CellSide.right, GetPart(cornfieldPartName), grid);
            AddBorderToPart(field, CellSide.bottom, GetPart(cornfieldPartName), grid);
        }
    }
}