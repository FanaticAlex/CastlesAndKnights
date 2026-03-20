using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.Base.Roads;
using Carcassone.Core.Tiles;

namespace Carcassone.Core.Calculation.Base.Tiles
{

    /// <summary>
    ///       R
    ///   |   |   |
    /// F |   |   | F
    ///   |   |   |
    ///       R
    /// </summary>
    public class RFRF : Tile
    {
        protected string roadPartName = "Road_0";
        protected string FarmPartName = "Farm_0";
        protected string FarmPart1Name = "Farm_1";

        public RFRF(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var roadPart1 = new RoadPart(roadPartName, this);
            roadPart1.AddSide(Side.top);
            roadPart1.AddSide(Side.bottom);
            Parts.Add(roadPart1);

            var FarmPart1 = new FarmPart(FarmPartName, this);
            FarmPart1.AddSide(Side.side_0);
            FarmPart1.AddSide(Side.right);
            FarmPart1.AddSide(Side.side_3);
            Parts.Add(FarmPart1);

            var FarmPart2 = new FarmPart(FarmPart1Name, this);
            FarmPart2.AddSide(Side.side_4);
            FarmPart2.AddSide(Side.left);
            FarmPart2.AddSide(Side.side_7);
            Parts.Add(FarmPart2);
        }
    }
}