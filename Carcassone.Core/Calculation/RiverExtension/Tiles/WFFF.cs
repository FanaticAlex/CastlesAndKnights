using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.RiverExtension.Rivers;
using Carcassone.Core.Tiles;

namespace Carcassone.Core.Calculation.RiverExtension.Tiles
{
    /// <summary>
    ///       W
    ///   |   +   |
    /// F |   +   | F
    ///   |       |
    ///       F
    /// </summary>
    public class WFFF : Tile
    {
        protected string RiverPart0Name = "River_0";
        protected string FarmPartName = "Farm_0";

        public WFFF(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var RiverPart0 = new RiverPart(RiverPart0Name, this);
            RiverPart0.AddSide(Side.top);
            Parts.Add(RiverPart0);

            var FarmPart1 = new FarmPart(FarmPartName, this);
            FarmPart1.AddSide(Side.side_0);
            FarmPart1.AddSide(Side.side_7);
            FarmPart1.AddSide(Side.right);
            FarmPart1.AddSide(Side.bottom);
            FarmPart1.AddSide(Side.left);
            Parts.Add(FarmPart1);
        }
    }
}