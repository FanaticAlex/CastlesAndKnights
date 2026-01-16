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
        protected string CityPart0Name = "City_0";
        protected string CityPart1Name = "City_1";
        protected string FarmPartName = "Farm_0";

        public FCFC(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var CityPart0 = new CityPart(CityPart0Name, Id);
            Parts.Add(CityPart0);

            var CityPart1 = new CityPart(CityPart1Name, Id);
            Parts.Add(CityPart1);

            var FarmPart1 = new FarmPart(FarmPartName, Id);
            Parts.Add(FarmPart1);


            FarmToCityParts.Add(FarmPart1.PartId, new List<string>() { CityPart0.PartId, CityPart1.PartId });
        }

        public override void ConnectCell(Cell cell, Grid grid)
        {
            AddBorderToPart(cell, Side.left, GetPart(CityPart0Name), grid);

            AddBorderToPart(cell, Side.right, GetPart(CityPart1Name), grid);

            AddBorderToPart(cell, Side.top, GetPart(FarmPartName), grid);
            AddBorderToPart(cell, Side.bottom, GetPart(FarmPartName), grid);
        }
    }
}