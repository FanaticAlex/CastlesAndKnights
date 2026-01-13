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
    /// R |_______| R
    ///   |   |   |
    ///       R
    /// </summary>
    public class CRRR : Tile
    {
        protected string CityPartName = "City_0";
        protected string roadPart0Name = "Road_0";
        protected string roadPart1Name = "Road_1";
        protected string roadPart2Name = "Road_2";
        protected string FarmPart0Name = "Farm_0";
        protected string FarmPart1Name = "Farm_1";
        protected string FarmPart2Name = "Farm_2";

        public CRRR(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var CityPart = new CityPart(CityPartName, Id);
            Parts.Add(CityPart);

            var roadPart0 = new RoadPart(roadPart0Name, Id);
            Parts.Add(roadPart0);

            var roadPart1 = new RoadPart(roadPart1Name, Id);
            Parts.Add(roadPart1);

            var roadPart2 = new RoadPart(roadPart2Name, Id);
            Parts.Add(roadPart2);

            var FarmPart0 = new FieldPart(FarmPart0Name, Id);
            Parts.Add(FarmPart0);

            var FarmPart1 = new FieldPart(FarmPart1Name, Id);
            Parts.Add(FarmPart1);

            var FarmPart2 = new FieldPart(FarmPart2Name, Id);
            Parts.Add(FarmPart2);


            FieldToCityParts.Add(FarmPart0.PartId, new List<string>() { CityPart.PartId });
        }

        public override void ConnectField(Cell field, Grid grid)
        {
            AddBorderToPart(field, CellSide.top, GetPart(CityPartName), grid);

            AddBorderToPart(field, CellSide.right, GetPart(roadPart0Name), grid);

            AddBorderToPart(field, CellSide.bottom, GetPart(roadPart1Name), grid);

            AddBorderToPart(field, CellSide.left, GetPart(roadPart2Name), grid);

            AddFarmSplittedBorder(field, CellSide.right, FieldSide.side_1, GetPart(FarmPart0Name), grid);
            AddFarmSplittedBorder(field, CellSide.left, FieldSide.side_6, GetPart(FarmPart0Name), grid);

            AddFarmSplittedBorder(field, CellSide.right, FieldSide.side_2, GetPart(FarmPart1Name), grid);
            AddFarmSplittedBorder(field, CellSide.bottom, FieldSide.side_3, GetPart(FarmPart1Name), grid);

            AddFarmSplittedBorder(field, CellSide.bottom, FieldSide.side_4, GetPart(FarmPart2Name), grid);
            AddFarmSplittedBorder(field, CellSide.left, FieldSide.side_5, GetPart(FarmPart2Name), grid);
        }
    }
}