using Carcassone.Core.Calculation.Base.Cities;
using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.Base.Roads;
using Carcassone.Core.Calculation.RiverExtension.Rivers;
using Carcassone.Core.Tiles;
using System.Collections.Generic;

namespace Carcassone.Core.Calculation.RiverExtension.Tiles
{
    /// <summary>
    ///       W
    ///   |   +  /|
    /// R |-----| | C
    ///   |   +  \|
    ///       W
    /// </summary>
    public class WCWR : Tile
    {
        protected string RiverPart0Name = "River_0";
        protected string CityPart0Name = "City_0";
        protected string FarmPart0Name = "Farm_0";
        protected string FarmPart1Name = "Farm_1";
        protected string FarmPart2Name = "Farm_2";
        protected string FarmPart3Name = "Farm_3";
        protected string roadPartName = "Road_0";

        public WCWR(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var RiverPart0 = new RiverPart(RiverPart0Name, this);
            RiverPart0.Sides.Add(Side.top);
            RiverPart0.Sides.Add(Side.bottom);
            Parts.Add(RiverPart0);

            var CityPart0 = new CityPart(CityPart0Name, this);
            CityPart0.Sides.Add(Side.right);
            Parts.Add(CityPart0);

            var FarmPart0 = new FarmPart(FarmPart0Name, this);
            FarmPart0.Sides.Add(Side.side_0);
            Parts.Add(FarmPart0);

            var FarmPart1 = new FarmPart(FarmPart1Name, this);
            FarmPart1.Sides.Add(Side.side_3);
            Parts.Add(FarmPart1);

            var FarmPart2 = new FarmPart(FarmPart2Name, this);
            FarmPart2.Sides.Add(Side.side_4);
            FarmPart2.Sides.Add(Side.side_5);
            Parts.Add(FarmPart2);

            var FarmPart3 = new FarmPart(FarmPart3Name, this);
            FarmPart3.Sides.Add(Side.side_6);
            FarmPart3.Sides.Add(Side.side_7);
            Parts.Add(FarmPart3);

            var roadPart = new RoadPart(roadPartName, this);
            roadPart.Sides.Add(Side.left);
            Parts.Add(roadPart);

            FarmToCityParts.Add(FarmPart0, new List<CityPart>() { CityPart0 });
            FarmToCityParts.Add(FarmPart1, new List<CityPart>() { CityPart0 });
        }
    }
}