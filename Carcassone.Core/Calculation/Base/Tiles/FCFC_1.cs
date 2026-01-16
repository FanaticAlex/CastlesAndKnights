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
        protected string CityPartName = "City_0";
        protected string FarmPartName = "Farm_0";
        protected string FarmPart1Name = "Farm_1";

        public FCFC_1(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var CityPart = new CityPart(CityPartName, Id);
            Parts.Add(CityPart);

            var FarmPart1 = new FarmPart(FarmPartName, Id);
            Parts.Add(FarmPart1);

            var FarmPart2 = new FarmPart(FarmPart1Name, Id);
            Parts.Add(FarmPart2);

            FarmToCityParts.Add(FarmPart1.PartId, new List<string>() { CityPart.PartId });
            FarmToCityParts.Add(FarmPart2.PartId, new List<string>() { CityPart.PartId });
        }

        public override void ConnectCell(Cell cell, Grid grid)
        {
            AddBorderToPart(cell, Side.right, GetPart(CityPartName), grid);
            AddBorderToPart(cell, Side.left, GetPart(CityPartName), grid);

            AddBorderToPart(cell, Side.top, GetPart(FarmPartName), grid);

            AddBorderToPart(cell, Side.bottom, GetPart(FarmPart1Name), grid);
        }
    }
}