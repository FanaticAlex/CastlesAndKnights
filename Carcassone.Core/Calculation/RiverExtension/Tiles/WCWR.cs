using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.Base.Roads;
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
        protected string CityPart0Name = "City_0";
        protected string FarmPart0Name = "Farm_0";
        protected string FarmPart1Name = "Farm_1";
        protected string FarmPart2Name = "Farm_2";
        protected string FarmPart3Name = "Farm_3";
        protected string roadPartName = "Road_0";

        public WCWR(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var CityPart0 = new CityPart(CityPart0Name, this);
            CityPart0.AddSide(Side.right);
            Parts.Add(CityPart0);

            var FarmPart0 = new FarmPart(FarmPart0Name, this);
            FarmPart0.AddSide(Side.side_0);
            Parts.Add(FarmPart0);

            var FarmPart1 = new FarmPart(FarmPart1Name, this);
            FarmPart1.AddSide(Side.side_3);
            Parts.Add(FarmPart1);

            var FarmPart2 = new FarmPart(FarmPart2Name, this);
            FarmPart2.AddSide(Side.side_4);
            FarmPart2.AddSide(Side.side_5);
            Parts.Add(FarmPart2);

            var FarmPart3 = new FarmPart(FarmPart3Name, this);
            FarmPart3.AddSide(Side.side_6);
            FarmPart3.AddSide(Side.side_7);
            Parts.Add(FarmPart3);

            var roadPart = new RoadPart(roadPartName, this);
            roadPart.AddSide(Side.left);
            Parts.Add(roadPart);

            FarmPart0.ConnectedCityParts.Add(CityPart0);
            FarmPart1.ConnectedCityParts.Add(CityPart0);
        }
    }
}