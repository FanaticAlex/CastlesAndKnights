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

            var FarmPart = new FieldPart(FarmPartName, Id);
            Parts.Add(FarmPart);

            FieldToCityParts.Add(FarmPart.PartId, new List<string>() { CityPart.PartId, CityPart1.PartId });
        }

        public override void ConnectField(Cell field, Grid grid)
        {
            AddBorderToPart(field, CellSide.top, GetPart(CityPartName), grid);

            AddBorderToPart(field, CellSide.right, GetPart(CityPart1Name), grid);

            AddBorderToPart(field, CellSide.bottom, GetPart(FarmPartName), grid);
            AddBorderToPart(field, CellSide.left, GetPart(FarmPartName), grid);
        }
    }
}