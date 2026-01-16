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

            var FarmPart1 = new FarmPart(FarmPartName, Id);
            Parts.Add(FarmPart1);

            var FarmPart2 = new FarmPart(FarmPart1Name, Id);
            Parts.Add(FarmPart2);


            FarmToCityParts.Add(FarmPart1.PartId, new List<string>() { CityPart.PartId });
        }

        public override void ConnectCell(Cell cell, Grid grid)
        {
            AddBorderToPart(cell, Side.top, GetPart(CityPartName), grid);

            AddBorderToPart(cell, Side.bottom, GetPart(roadPartName), grid);
            AddBorderToPart(cell, Side.left, GetPart(roadPartName), grid);

            AddBorderToPart(cell, Side.right, GetPart(FarmPartName), grid);
            AddFarmSplittedBorder(cell, Side.bottom, FieldSide.side_3, GetPart(FarmPartName), grid);
            AddFarmSplittedBorder(cell, Side.left, FieldSide.side_6, GetPart(FarmPartName), grid);

            AddFarmSplittedBorder(cell, Side.bottom, FieldSide.side_4, GetPart(FarmPart1Name), grid);
            AddFarmSplittedBorder(cell, Side.left, FieldSide.side_5, GetPart(FarmPart1Name), grid);
        }
    }
}