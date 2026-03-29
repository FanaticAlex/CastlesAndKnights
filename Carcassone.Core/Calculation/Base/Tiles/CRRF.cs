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
    /// F |      _| R
    ///   |    /  |
    ///       R
    /// </summary>
    public class CRRF : Tile
    {
        protected string CityPartName = "City_0";
        protected string roadPartName = "Road_0";
        protected string FarmPartName = "Farm_0";
        protected string FarmPart1Name = "Farm_1";

        public CRRF(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var CityPart = new CityPart(CityPartName, this);
            CityPart.AddSide(Side.top);
            Parts.Add(CityPart);

            var roadPart = new RoadPart(roadPartName, this);
            roadPart.AddSide(Side.right);
            roadPart.AddSide(Side.bottom);
            Parts.Add(roadPart);

            var FarmPart1 = new FarmPart(FarmPartName, this);
            FarmPart1.AddSide(Side.side_1);
            FarmPart1.AddSide(Side.side_4);
            FarmPart1.AddSide(Side.left);
            Parts.Add(FarmPart1);

            var FarmPart2 = new FarmPart(FarmPart1Name, this);
            FarmPart2.AddSide(Side.side_2);
            FarmPart2.AddSide(Side.side_3);
            Parts.Add(FarmPart2);

            FarmPart1.ConnectedCityParts.Add(CityPart);
        }
    }
}