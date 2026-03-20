using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.RiverExtension.Rivers;
using Carcassone.Core.Tiles;

namespace Carcassone.Core.Calculation.RiverExtension.Tiles
{

    /// <summary>
    ///       F
    ///   |       |
    /// F |   +   | F
    ///   |   +   |
    ///       W
    /// </summary>
    public class FFWF : Tile
    {
        protected string RiverPart0Name = "River_0";
        protected string FarmPartName = "Farm_0";

        public FFWF(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var RiverPart0 = new RiverPart(RiverPart0Name, this);
            RiverPart0.AddSide(Side.bottom);
            Parts.Add(RiverPart0);

            var FarmPart = new FarmPart(FarmPartName, this);
            FarmPart.AddSide(Side.left);
            FarmPart.AddSide(Side.top);
            FarmPart.AddSide(Side.right);
            FarmPart.AddSide(Side.side_3);
            FarmPart.AddSide(Side.side_4);
            Parts.Add(FarmPart);
        }
    }
}