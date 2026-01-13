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

            var FarmPart1 = new FieldPart(FarmPartName, Id);
            Parts.Add(FarmPart1);

            var FarmPart2 = new FieldPart(FarmPart1Name, Id);
            Parts.Add(FarmPart2);

            FieldToCityParts.Add(FarmPart1.PartId, new List<string>() { CityPart.PartId });
            FieldToCityParts.Add(FarmPart2.PartId, new List<string>() { CityPart.PartId });
        }

        public override void ConnectField(Cell field, Grid grid)
        {
            AddBorderToPart(field, CellSide.right, GetPart(CityPartName), grid);
            AddBorderToPart(field, CellSide.left, GetPart(CityPartName), grid);

            AddBorderToPart(field, CellSide.top, GetPart(FarmPartName), grid);

            AddBorderToPart(field, CellSide.bottom, GetPart(FarmPart1Name), grid);
        }
    }
}