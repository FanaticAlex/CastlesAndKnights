using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.Base.Roads;
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
            var FarmPart0 = new FarmPart(FarmPart0Name, this);
            FarmPart0.AddSide(Side.side_0);
            FarmPart0.AddSide(Side.side_1);
            Parts.Add(FarmPart0);

            var FarmPart1 = new FarmPart(FarmPart1Name, this);
            FarmPart1.AddSide(Side.side_2);
            FarmPart1.AddSide(Side.side_3);
            FarmPart1.AddSide(Side.side_6);
            FarmPart1.AddSide(Side.side_7);
            Parts.Add(FarmPart1);

            var FarmPart2 = new FarmPart(FarmPart2Name, this);
            FarmPart2.AddSide(Side.side_4);
            FarmPart2.AddSide(Side.side_5);
            Parts.Add(FarmPart2);

            var roadPart = new RoadPart(roadPartName, this);
            roadPart.AddSide(Side.top);
            roadPart.AddSide(Side.right);
            Parts.Add(roadPart);
        }
    }
}