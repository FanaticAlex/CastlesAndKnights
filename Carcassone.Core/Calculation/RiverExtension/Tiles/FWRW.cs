using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.Base.Monasteries;
using Carcassone.Core.Calculation.Base.Roads;
using Carcassone.Core.Calculation.RiverExtension.Rivers;
using Carcassone.Core.Tiles;

namespace Carcassone.Core.Calculation.RiverExtension.Tiles
{

    /// <summary>
    ///       F
    ///   |       |
    /// W |+++O+++| W
    ///   |   |   |
    ///       R
    /// </summary>
    public class FWRW : Tile
    {
        protected string RiverPart0Name = "River_0";
        protected string monasteryPartName = "Monastery_0";
        protected string FarmPart0Name = "Farm_0";
        protected string FarmPart1Name = "Farm_1";
        protected string FarmPart2Name = "Farm_2";
        protected string roadPartName = "Road_0";

        public FWRW(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var RiverPart0 = new RiverPart(RiverPart0Name, this);
            RiverPart0.Sides.Add(Side.right);
            RiverPart0.Sides.Add(Side.left);
            Parts.Add(RiverPart0);

            var monasteryPart = new MonasteryPart(monasteryPartName, this);
            Parts.Add(monasteryPart);

            var FarmPart1 = new FarmPart(FarmPart0Name, this);
            FarmPart1.Sides.Add(Side.top);
            FarmPart1.Sides.Add(Side.side_1);
            FarmPart1.Sides.Add(Side.side_6);
            Parts.Add(FarmPart1);

            var FarmPart2 = new FarmPart(FarmPart1Name, this);
            FarmPart2.Sides.Add(Side.side_2);
            FarmPart2.Sides.Add(Side.side_3);
            Parts.Add(FarmPart2);

            var roadPart = new RoadPart(roadPartName, this);
            roadPart.Sides.Add(Side.bottom);
            Parts.Add(roadPart);

            var FarmPart3 = new FarmPart(FarmPart2Name, this);
            FarmPart3.Sides.Add(Side.side_4);
            FarmPart3.Sides.Add(Side.side_5);
            Parts.Add(FarmPart3);
        }
    }
}