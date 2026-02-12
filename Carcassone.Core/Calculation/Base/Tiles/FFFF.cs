using Carcassone.Core.Calculation.Base.Monasteries;
using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Tiles;

namespace Carcassone.Core.Calculation.Base.Tiles
{
    /// <summary>
    ///       F
    ///   |       |
    /// F |   O   | F
    ///   |       |
    ///       F
    /// </summary>
    public class FFFF : Tile
    {
        protected string churchPartName = "Church_0";
        protected string FarmPartName = "Farm_0";

        public FFFF(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var churchPart = new MonasteryPart(churchPartName, this);
            Parts.Add(churchPart);

            var FarmPart1 = new FarmPart(FarmPartName, this);
            FarmPart1.Sides.Add(Side.top);
            FarmPart1.Sides.Add(Side.right);
            FarmPart1.Sides.Add(Side.bottom);
            FarmPart1.Sides.Add(Side.left);
            Parts.Add(FarmPart1);
        }
    }
}