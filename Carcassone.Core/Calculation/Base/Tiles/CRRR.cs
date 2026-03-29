using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.Base.Farms;
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
            CityPart.AddSide(Side.top);
            Parts.Add(CityPart);

            var roadPart0 = new RoadPart(roadPart0Name, this);
            roadPart0.AddSide(Side.right);
            Parts.Add(roadPart0);

            var roadPart1 = new RoadPart(roadPart1Name, this);
            roadPart1.AddSide(Side.bottom);
            Parts.Add(roadPart1);

            var roadPart2 = new RoadPart(roadPart2Name, this);
            roadPart2.AddSide(Side.left);
            Parts.Add(roadPart2);

            var FarmPart0 = new FarmPart(FarmPart0Name, this);
            FarmPart0.AddSide(Side.side_1);
            FarmPart0.AddSide(Side.side_6);
            Parts.Add(FarmPart0);

            var FarmPart1 = new FarmPart(FarmPart1Name, this);
            FarmPart1.AddSide(Side.side_2);
            FarmPart1.AddSide(Side.side_3);
            Parts.Add(FarmPart1);

            var FarmPart2 = new FarmPart(FarmPart2Name, this);
            FarmPart2.AddSide(Side.side_4);
            FarmPart2.AddSide(Side.side_5);
            Parts.Add(FarmPart2);


            FarmPart0.ConnectedCityParts.Add(CityPart);
        }
    }
}