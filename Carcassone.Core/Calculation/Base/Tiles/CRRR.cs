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
            var CityPart = new CityPart(CityPartName, this);
            CityPart.Sides.Add(Side.top);
            Parts.Add(CityPart);

            var roadPart0 = new RoadPart(roadPart0Name, this);
            roadPart0.Sides.Add(Side.right);
            Parts.Add(roadPart0);

            var roadPart1 = new RoadPart(roadPart1Name, this);
            roadPart1.Sides.Add(Side.bottom);
            Parts.Add(roadPart1);

            var roadPart2 = new RoadPart(roadPart2Name, this);
            roadPart2.Sides.Add(Side.left);
            Parts.Add(roadPart2);

            var FarmPart0 = new FarmPart(FarmPart0Name, this);
            FarmPart0.Sides.Add(Side.side_1);
            FarmPart0.Sides.Add(Side.side_6);
            Parts.Add(FarmPart0);

            var FarmPart1 = new FarmPart(FarmPart1Name, this);
            FarmPart1.Sides.Add(Side.side_2);
            FarmPart1.Sides.Add(Side.side_3);
            Parts.Add(FarmPart1);

            var FarmPart2 = new FarmPart(FarmPart2Name, this);
            FarmPart2.Sides.Add(Side.side_4);
            FarmPart2.Sides.Add(Side.side_5);
            Parts.Add(FarmPart2);


            FarmToCityParts.Add(FarmPart0, new List<CityPart>() { CityPart });
        }
    }
}