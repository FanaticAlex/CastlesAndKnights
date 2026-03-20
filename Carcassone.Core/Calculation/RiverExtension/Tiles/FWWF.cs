using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.RiverExtension.Rivers;
using Carcassone.Core.Tiles;

namespace Carcassone.Core.Calculation.RiverExtension.Tiles
{
    /// <summary>
    ///       F
    ///   |       |
    /// F |   ++++| W
    ///   |   +   |
    ///       W
    /// </summary>
    public class FWWF : Tile
    {
        protected string RiverPart0Name = "River_0";
        protected string FarmPart0Name = "Farm_0";
        protected string FarmPart1Name = "Farm_1";

        public FWWF(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var RiverPart0 = new RiverPart(RiverPart0Name, this);
            RiverPart0.AddSide(Side.right);
            RiverPart0.AddSide(Side.bottom);
            Parts.Add(RiverPart0);

            var FarmPart1 = new FarmPart(FarmPart0Name, this);
            FarmPart1.AddSide(Side.top);
            FarmPart1.AddSide(Side.side_1);
            FarmPart1.AddSide(Side.side_4);
            FarmPart1.AddSide(Side.left);
            Parts.Add(FarmPart1);

            var FarmPart2 = new FarmPart(FarmPart1Name, this);
            FarmPart2.AddSide(Side.side_2);
            FarmPart2.AddSide(Side.side_3);
            Parts.Add(FarmPart2);
        }
    }
}