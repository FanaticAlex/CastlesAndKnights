using Carcassone.Core.Board;
using Carcassone.Core.Tiles;
using System.Collections.Generic;
using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.Base.Cities;

namespace Carcassone.Core.Calculation.Base.Tiles
{

    /// <summary>
    ///       F
    ///   |\_____/|
    /// C | _____ | C
    ///   |/     \|
    ///       F
    /// </summary>
    public class FCFC_1 : Tile
    {
        protected string castlePartName = "Castle_0";
        protected string cornfieldPartName = "Cornfield_0";
        protected string cornfieldPart1Name = "Cornfield_1";

        public FCFC_1(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var castlePart = new CityPart(castlePartName, Id);
            Parts.Add(castlePart);

            var cornfieldPart1 = new FieldPart(cornfieldPartName, Id);
            Parts.Add(cornfieldPart1);

            var cornfieldPart2 = new FieldPart(cornfieldPart1Name, Id);
            Parts.Add(cornfieldPart2);

            FieldToCastleParts.Add(cornfieldPart1.PartId, new List<string>() { castlePart.PartId });
            FieldToCastleParts.Add(cornfieldPart2.PartId, new List<string>() { castlePart.PartId });
        }

        public override void ConnectField(Cell field, Grid grid)
        {
            AddBorderToPart(field, CellSide.right, GetPart(castlePartName), grid);
            AddBorderToPart(field, CellSide.left, GetPart(castlePartName), grid);

            AddBorderToPart(field, CellSide.top, GetPart(cornfieldPartName), grid);

            AddBorderToPart(field, CellSide.bottom, GetPart(cornfieldPart1Name), grid);
        }
    }
}