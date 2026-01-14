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
        protected string CityPartName = "City_0";
        protected string FarmPartName = "Farm_0";

        public CFFC(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var CityPart = new CityPart(CityPartName, Id);
            Parts.Add(CityPart);

            var FarmPart = new FarmPart(FarmPartName, Id);
            Parts.Add(FarmPart);
         
            FarmToCityParts.Add(FarmPart.PartId, new List<string>() { CityPart.PartId });
        }

        public override void ConnectCell(Cell cell, Grid grid)
        {
            AddBorderToPart(cell, CellSide.top, GetPart(CityPartName), grid);
            AddBorderToPart(cell, CellSide.left, GetPart(CityPartName), grid);

            AddBorderToPart(cell, CellSide.right, GetPart(FarmPartName), grid);
            AddBorderToPart(cell, CellSide.bottom, GetPart(FarmPartName), grid);
        }
    }
}