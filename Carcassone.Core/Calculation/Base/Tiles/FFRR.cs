using Carcassone.Core.Board;
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
            roadPart.Sides.Add(Side.bottom);
            roadPart.Sides.Add(Side.left);
            Parts.Add(roadPart);

            var FarmPart1 = new FarmPart(FarmPartName, this);
            FarmPart1.Sides.Add(Side.top);
            FarmPart1.Sides.Add(Side.right);
            FarmPart1.Sides.Add(Side.side_3);
            FarmPart1.Sides.Add(Side.side_6);
            Parts.Add(FarmPart1);

            var FarmPart2 = new FarmPart(FarmPart1Name, this);
            FarmPart2.Sides.Add(Side.side_4);
            FarmPart2.Sides.Add(Side.side_5);
            Parts.Add(FarmPart2);
        }
    }
}