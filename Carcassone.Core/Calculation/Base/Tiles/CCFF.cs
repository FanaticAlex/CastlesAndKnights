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
    ///   |\_____//|
    /// F |      | | C
    ///   |       \|
    ///       F
    /// </summary>
    public class CCFF : Tile
    {
        protected string CityPartName = "City_0";
        protected string CityPart1Name = "City_1";
        protected string FarmPartName = "Farm_0";

        public CCFF(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var CityPart = new CityPart(CityPartName, Id);
            Parts.Add(CityPart);

            var CityPart1 = new CityPart(CityPart1Name, Id);
            Parts.Add(CityPart1);

            var FarmPart = new FarmPart(FarmPartName, Id);
            Parts.Add(FarmPart);

            FarmToCityParts.Add(FarmPart.PartId, new List<string>() { CityPart.PartId, CityPart1.PartId });
        }

        public override void ConnectCell(Cell cell, Grid grid)
        {
            AddBorderToPart(cell, CellSide.top, GetPart(CityPartName), grid);

            AddBorderToPart(cell, CellSide.right, GetPart(CityPart1Name), grid);

            AddBorderToPart(cell, CellSide.bottom, GetPart(FarmPartName), grid);
            AddBorderToPart(cell, CellSide.left, GetPart(FarmPartName), grid);
        }
    }
}