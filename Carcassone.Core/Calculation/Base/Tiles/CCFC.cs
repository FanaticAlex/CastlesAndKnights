using Carcassone.Core.Board;
using Carcassone.Core.Tiles;
using System.Collections;
using System.Collections.Generic;
using Carcassone.Core.Calculation.Base.Cities;
using Carcassone.Core.Calculation.Base.Farms;

namespace Carcassone.Core.Calculation.Base.Tiles
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
        protected string CityPartName = "City_0";
        protected string FarmPartName = "Farm_0";

        public CCFC(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var CityPart = new CityPart(CityPartName, Id);
            Parts.Add(CityPart);

            var FarmPart = new FieldPart(FarmPartName, Id);
            Parts.Add(FarmPart);

            FieldToCityParts.Add(FarmPart.PartId, new List<string>() { CityPart.PartId });
        }

        public override void ConnectField(Cell field, Grid grid)
        {
            AddBorderToPart(field, CellSide.top, GetPart(CityPartName), grid);
            AddBorderToPart(field, CellSide.right, GetPart(CityPartName), grid);
            AddBorderToPart(field, CellSide.left, GetPart(CityPartName), grid);

            AddBorderToPart(field, CellSide.bottom, GetPart(FarmPartName), grid);
        }
    }
}