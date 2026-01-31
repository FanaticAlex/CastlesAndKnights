using Carcassone.Core.Board;
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
            RiverPart0.Sides.Add(Side.bottom);
            Parts.Add(RiverPart0);

            var FarmPart = new FarmPart(FarmPartName, this);
            FarmPart.Sides.Add(Side.left);
            FarmPart.Sides.Add(Side.top);
            FarmPart.Sides.Add(Side.right);
            FarmPart.Sides.Add(Side.side_3);
            FarmPart.Sides.Add(Side.side_4);
            Parts.Add(FarmPart);
        }
    }
}