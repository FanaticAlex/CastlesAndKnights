using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.RiverExtension.Rivers;
using Carcassone.Core.Tiles;

namespace Carcassone.Core.Calculation.RiverExtension.Tiles
{
    /// <summary>
    ///       W
    ///   |   +   |
    /// F |   +   | F
    ///   |   +   |
    ///       W
    /// </summary>
    public class WFWF : Tile
    {
        protected string RiverPart0Name = "River_0";
        protected string FarmPart0Name = "Farm_0";
        protected string FarmPart1Name = "Farm_1";

        public WFWF(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var RiverPart0 = new RiverPart(RiverPart0Name, this);
            RiverPart0.Sides.Add(Side.top);
            RiverPart0.Sides.Add(Side.bottom);
            Parts.Add(RiverPart0);

            var FarmPart0 = new FarmPart(FarmPart0Name, this);
            FarmPart0.Sides.Add(Side.side_0);
            FarmPart0.Sides.Add(Side.right);
            FarmPart0.Sides.Add(Side.side_3);
            Parts.Add(FarmPart0);

            var FarmPart1 = new FarmPart(FarmPart1Name, this);
            FarmPart1.Sides.Add(Side.side_4);
            FarmPart1.Sides.Add(Side.left);
            FarmPart1.Sides.Add(Side.side_7);
            Parts.Add(FarmPart1);
        }
    }
}