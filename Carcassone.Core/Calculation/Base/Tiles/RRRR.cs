using Carcassone.Core.Board;
using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.Base.Roads;
using Carcassone.Core.Tiles;

namespace Carcassone.Core.Calculation.Base.Tiles
{

    /// <summary>
    ///       R
    ///   |   |   |
    /// R |---|---| R
    ///   |   |   |
    ///       R
    /// </summary>
    public class RRRR : Tile
    {
        protected string roadPartName = "Road_0";
        protected string roadPart1Name = "Road_1";
        protected string roadPart2Name = "Road_2";
        protected string roadPart3Name = "Road_3";
        protected string FarmPartName = "Farm_0";
        protected string FarmPart1Name = "Farm_1";
        protected string FarmPart2Name = "Farm_2";
        protected string FarmPart3Name = "Farm_3";

        public RRRR(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var roadPart1 = new RoadPart(roadPartName, this);
            roadPart1.Sides.Add(Side.top);
            Parts.Add(roadPart1);

            var roadPart2 = new RoadPart(roadPart1Name, this);
            roadPart2.Sides.Add(Side.right);
            Parts.Add(roadPart2);

            var roadPart3 = new RoadPart(roadPart2Name, this);
            roadPart3.Sides.Add(Side.bottom);
            Parts.Add(roadPart3);

            var roadPart4 = new RoadPart(roadPart3Name, this);
            roadPart4.Sides.Add(Side.left);
            Parts.Add(roadPart4);

            var FarmPart1 = new FarmPart(FarmPartName, this);
            FarmPart1.Sides.Add(Side.side_0);
            FarmPart1.Sides.Add(Side.side_1);
            Parts.Add(FarmPart1);

            var FarmPart2 = new FarmPart(FarmPart1Name, this);
            FarmPart2.Sides.Add(Side.side_2);
            FarmPart2.Sides.Add(Side.side_3);
            Parts.Add(FarmPart2);

            var FarmPart3 = new FarmPart(FarmPart2Name, this);
            FarmPart3.Sides.Add(Side.side_4);
            FarmPart3.Sides.Add(Side.side_5);
            Parts.Add(FarmPart3);

            var FarmPart4 = new FarmPart(FarmPart3Name, this);
            FarmPart4.Sides.Add(Side.side_6);
            FarmPart4.Sides.Add(Side.side_7);
            Parts.Add(FarmPart4);
        }
    }
}