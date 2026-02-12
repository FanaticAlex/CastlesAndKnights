using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.Base.Monasteries;
using Carcassone.Core.Calculation.Base.Roads;
using Carcassone.Core.Tiles;

namespace Carcassone.Core.Calculation.Base.Tiles
{
    /// <summary>
    ///       F
    ///   |       |
    /// F |   O   | F
    ///   |   |   |
    ///       R
    /// </summary>
    public class FFRF : Tile
    {
        protected string roadPartName = "Road_0";
        protected string churchPartName = "Church_0";
        protected string FarmPartName = "Farm_0";

        public FFRF(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var roadPart3 = new RoadPart(roadPartName, this);
            roadPart3.Sides.Add(Side.bottom);
            Parts.Add(roadPart3);

            var churchPart = new MonasteryPart(churchPartName, this);
            Parts.Add(churchPart);

            var FarmPart1 = new FarmPart(FarmPartName, this);
            FarmPart1.Sides.Add(Side.top);
            FarmPart1.Sides.Add(Side.right);
            FarmPart1.Sides.Add(Side.left);
            FarmPart1.Sides.Add(Side.side_3);
            FarmPart1.Sides.Add(Side.side_4);
            Parts.Add(FarmPart1);
        }
    }
}