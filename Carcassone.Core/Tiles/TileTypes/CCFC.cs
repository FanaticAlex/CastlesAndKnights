using Carcassone.Core.Board;
using System.Collections;
using System.Collections.Generic;

namespace Carcassone.Core.Tiles
{

    /// <summary>
    ///       C
    ///   |       |
    /// C | _____ | C
    ///   |/     \|
    ///       F
    /// </summary>
    public class CCFC : Tile
    {
        protected string castlePartName = "Castle_0";
        protected string cornfieldPartName = "Cornfield_0";

        public CCFC(string cardType, int cardNumber) : base(cardType, cardNumber)
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
            AddBorderToPart(field, CellSide.right, GetPart(castlePartName), grid);
            AddBorderToPart(field, CellSide.left, GetPart(castlePartName), grid);

            AddBorderToPart(field, CellSide.bottom, GetPart(cornfieldPartName), grid);
        }
    }
}