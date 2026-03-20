using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.Base.Roads;
using Carcassone.Core.Tiles;

namespace Carcassone.Core.Calculation.Base.Tiles
{

    /// <summary>
    ///       F
    ///   |       |
    /// R |_      | F
    ///   |  \    |
    ///       R
    /// </summary>
    public class FFRR : Tile
    {
        protected string roadPartName = "Road_0";
        protected string FarmPartName = "Farm_0";
        protected string FarmPart1Name = "Farm_1";

        public FFRR(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var roadPart = new RoadPart(roadPartName, this);
            roadPart.AddSide(Side.bottom);
            roadPart.AddSide(Side.left);
            Parts.Add(roadPart);

            var FarmPart1 = new FarmPart(FarmPartName, this);
            FarmPart1.AddSide(Side.top);
            FarmPart1.AddSide(Side.right);
            FarmPart1.AddSide(Side.side_3);
            FarmPart1.AddSide(Side.side_6);
            Parts.Add(FarmPart1);

            var FarmPart2 = new FarmPart(FarmPart1Name, this);
            FarmPart2.AddSide(Side.side_4);
            FarmPart2.AddSide(Side.side_5);
            Parts.Add(FarmPart2);
        }
    }
}