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

            var FarmPart1 = new FieldPart(FarmPartName, Id);
            Parts.Add(FarmPart1);


            FieldToCityParts.Add(FarmPart1.PartId, new List<string>() { CityPart0.PartId, CityPart1.PartId });
        }

        public override void ConnectField(Cell field, Grid grid)
        {
            AddBorderToPart(field, CellSide.left, GetPart(CityPart0Name), grid);

            AddBorderToPart(field, CellSide.right, GetPart(CityPart1Name), grid);

            AddBorderToPart(field, CellSide.top, GetPart(FarmPartName), grid);
            AddBorderToPart(field, CellSide.bottom, GetPart(FarmPartName), grid);
        }
    }
}