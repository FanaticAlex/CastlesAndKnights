using System.Collections.Generic;
using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.Base.Cities;
using Carcassone.Core.Calculation.Base.Roads;
using Carcassone.Core.Tiles;

namespace Carcassone.Core.Calculation.Base.Tiles
{

    /// <summary>
    ///       C
    ///   |    __/|
    /// C | __/  _| R
    ///   |/   /  |
    ///       R
    /// </summary>
    public class CRRC : Tile
    {
        protected string CityPartName = "City_0";
        protected string roadPartName = "Road_0";
        protected string FarmPartName = "Farm_0";
        protected string FarmPart1Name = "Farm_1";

        public CRRC(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var CityPart = new CityPart(CityPartName, this);
            CityPart.Sides.Add(Side.top);
            CityPart.Sides.Add(Side.left);
            Parts.Add(CityPart);

            var roadPart = new RoadPart(roadPartName, this);
            roadPart.Sides.Add(Side.right);
            roadPart.Sides.Add(Side.bottom);
            Parts.Add(roadPart);

            var FarmPart1 = new FarmPart(FarmPartName, this);
            FarmPart1.Sides.Add(Side.side_1);
            FarmPart1.Sides.Add(Side.side_4);
            Parts.Add(FarmPart1);

            var FarmPart2 = new FarmPart(FarmPart1Name, this);
            FarmPart2.Sides.Add(Side.side_2);
            FarmPart2.Sides.Add(Side.side_3);
            Parts.Add(FarmPart2);


            FarmToCityParts.Add(FarmPart1, new List<CityPart>() { CityPart });
        }
    }
}