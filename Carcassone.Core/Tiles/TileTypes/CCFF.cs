using Carcassone.Core.Board;
using System.Collections;
using System.Collections.Generic;

namespace Carcassone.Core.Tiles
{

    /// <summary>
    ///       C
    ///   |\_____//|
    /// F |      | | C
    ///   |       \|
    ///       F
    /// </summary>
    public class CCFF : Tile
    {
        protected string castlePartName = "Castle_0";
        protected string castlePart1Name = "Castle_1";
        protected string cornfieldPartName = "Cornfield_0";

        public CCFF(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var castlePart = new CityPart(castlePartName, Id);
            Parts.Add(castlePart);

            var castlePart1 = new CityPart(castlePart1Name, Id);
            Parts.Add(castlePart1);

            var cornfieldPart = new FieldPart(cornfieldPartName, Id);
            Parts.Add(cornfieldPart);

            FieldToCastleParts.Add(cornfieldPart.PartId, new List<string>() { castlePart.PartId, castlePart1.PartId });
        }

        public override void ConnectField(Cell field, Grid grid)
        {
            AddBorderToPart(field, CellSide.top, GetPart(castlePartName), grid);

            AddBorderToPart(field, CellSide.right, GetPart(castlePart1Name), grid);

            AddBorderToPart(field, CellSide.bottom, GetPart(cornfieldPartName), grid);
            AddBorderToPart(field, CellSide.left, GetPart(cornfieldPartName), grid);
        }
    }
}