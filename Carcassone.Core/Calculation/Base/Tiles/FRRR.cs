using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.Base.Roads;
using Carcassone.Core.Tiles;

namespace Carcassone.Core.Calculation.Base.Tiles
{

    /// <summary>
    ///       F
    ///   |       |
    /// R |_______| R
    ///   |   |   |
    ///       R
    /// </summary>
    public class FRRR : Tile
    {
        protected string roadPartName = "Road_0";
        protected string roadPart1Name = "Road_1";
        protected string roadPart2Name = "Road_2";
        protected string FarmPartName = "Farm_0";
        protected string FarmPart1Name = "Farm_1";
        protected string FarmPart2Name = "Farm_2";

        public FRRR(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var roadPart1 = new RoadPart(roadPartName, this);
            roadPart1.Sides.Add(Side.right);
            Parts.Add(roadPart1);

            var roadPart3 = new RoadPart(roadPart1Name, this);
            roadPart3.Sides.Add(Side.bottom);
            Parts.Add(roadPart3);

            var roadPart4 = new RoadPart(roadPart2Name, this);
            roadPart4.Sides.Add(Side.left);
            Parts.Add(roadPart4);

            var FarmPart1 = new FarmPart(FarmPartName, this);
            FarmPart1.Sides.Add(Side.side_1);
            FarmPart1.Sides.Add(Side.top);
            FarmPart1.Sides.Add(Side.side_6);
            Parts.Add(FarmPart1);

            var FarmPart2 = new FarmPart(FarmPart1Name, this);
            FarmPart2.Sides.Add(Side.side_2);
            FarmPart2.Sides.Add(Side.side_3);
            Parts.Add(FarmPart2);

            var FarmPart3 = new FarmPart(FarmPart2Name, this);
            FarmPart3.Sides.Add(Side.side_4);
            FarmPart3.Sides.Add(Side.side_5);
            Parts.Add(FarmPart3);
        }
    }
}