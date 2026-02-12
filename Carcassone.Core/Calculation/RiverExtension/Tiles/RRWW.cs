using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.Base.Roads;
using Carcassone.Core.Calculation.RiverExtension.Rivers;
using Carcassone.Core.Tiles;

namespace Carcassone.Core.Calculation.RiverExtension.Tiles
{
    /// <summary>
    ///       R
    ///   |    \  |
    /// W |+++  \_| R
    ///   |   +   |
    ///       W
    /// </summary>
    public class RRWW : Tile
    {
        protected string RiverPart0Name = "River_0";
        protected string FarmPart0Name = "Farm_0";
        protected string FarmPart1Name = "Farm_1";
        protected string FarmPart2Name = "Farm_2";
        protected string roadPartName = "Road_0";

        public RRWW(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var RiverPart0 = new RiverPart(RiverPart0Name, this);
            RiverPart0.Sides.Add(Side.bottom);
            RiverPart0.Sides.Add(Side.left);
            Parts.Add(RiverPart0);

            var FarmPart0 = new FarmPart(FarmPart0Name, this);
            FarmPart0.Sides.Add(Side.side_0);
            FarmPart0.Sides.Add(Side.side_1);
            Parts.Add(FarmPart0);

            var FarmPart1 = new FarmPart(FarmPart1Name, this);
            FarmPart1.Sides.Add(Side.side_2);
            FarmPart1.Sides.Add(Side.side_3);
            FarmPart1.Sides.Add(Side.side_6);
            FarmPart1.Sides.Add(Side.side_7);
            Parts.Add(FarmPart1);

            var FarmPart2 = new FarmPart(FarmPart2Name, this);
            FarmPart2.Sides.Add(Side.side_4);
            FarmPart2.Sides.Add(Side.side_5);
            Parts.Add(FarmPart2);

            var roadPart = new RoadPart(roadPartName, this);
            roadPart.Sides.Add(Side.top);
            roadPart.Sides.Add(Side.right);
            Parts.Add(roadPart);
        }
    }
}