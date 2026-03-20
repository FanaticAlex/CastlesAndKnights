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
        protected string monasteryPartName = "Monastery_0";
        protected string FarmPartName = "Farm_0";

        public FFFF(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var monasteryPart = new MonasteryPart(monasteryPartName, this);
            Parts.Add(monasteryPart);

            var FarmPart1 = new FarmPart(FarmPartName, this);
            FarmPart1.AddSide(Side.top);
            FarmPart1.AddSide(Side.right);
            FarmPart1.AddSide(Side.bottom);
            FarmPart1.AddSide(Side.left);
            Parts.Add(FarmPart1);
        }
    }
}