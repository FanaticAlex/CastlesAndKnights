using Carcassone.Core.Calculation.Base.Farms;
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
        protected string FarmPart0Name = "Farm_0";
        protected string FarmPart1Name = "Farm_1";

        public WFWF(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var FarmPart0 = new FarmPart(FarmPart0Name, this);
            FarmPart0.AddSide(Side.side_0);
            FarmPart0.AddSide(Side.right);
            FarmPart0.AddSide(Side.side_3);
            Parts.Add(FarmPart0);

            var FarmPart1 = new FarmPart(FarmPart1Name, this);
            FarmPart1.AddSide(Side.side_4);
            FarmPart1.AddSide(Side.left);
            FarmPart1.AddSide(Side.side_7);
            Parts.Add(FarmPart1);
        }
    }
}