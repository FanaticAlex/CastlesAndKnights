using Carcassone.Core.Board;
using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.Base.Cities;
using Carcassone.Core.Calculation.Base.Roads;
using Carcassone.Core.Tiles;
using System.Collections.Generic;

namespace Carcassone.Core.Calculation.Base.Tiles
{

    /// <summary>
    ///       C
    ///   |\_____/|
    /// R |__     | F
    ///   |   \   |
    ///       R
    /// </summary>
    public class CFRR : Tile
    {
        protected string CityPartName = "City_0";
        protected string roadPartName = "Road_0";
        protected string FarmPartName = "Farm_0";
        protected string FarmPart1Name = "Farm_1";

        public CFRR(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var CityPart = new CityPart(CityPartName, Id);
            Parts.Add(CityPart);

            var roadPart = new RoadPart(roadPartName, Id);
            Parts.Add(roadPart);

            var FarmPart1 = new FieldPart(FarmPartName, Id);
            Parts.Add(FarmPart1);

            var FarmPart2 = new FieldPart(FarmPart1Name, Id);
            Parts.Add(FarmPart2);


            FieldToCityParts.Add(FarmPart1.PartId, new List<string>() { CityPart.PartId });
        }

        public override void ConnectField(Cell field, Grid grid)
        {
            AddBorderToPart(field, CellSide.top, GetPart(CityPartName), grid);

            AddBorderToPart(field, CellSide.bottom, GetPart(roadPartName), grid);
            AddBorderToPart(field, CellSide.left, GetPart(roadPartName), grid);

            AddBorderToPart(field, CellSide.right, GetPart(FarmPartName), grid);
            AddFarmSplittedBorder(field, CellSide.bottom, FieldSide.side_3, GetPart(FarmPartName), grid);
            AddFarmSplittedBorder(field, CellSide.left, FieldSide.side_6, GetPart(FarmPartName), grid);

            AddFarmSplittedBorder(field, CellSide.bottom, FieldSide.side_4, GetPart(FarmPart1Name), grid);
            AddFarmSplittedBorder(field, CellSide.left, FieldSide.side_5, GetPart(FarmPart1Name), grid);
        }
    }
}