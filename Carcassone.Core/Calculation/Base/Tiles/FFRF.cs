using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.Base.Monasteries;
using Carcassone.Core.Calculation.Base.Roads;
using Carcassone.Core.Tiles;

namespace Carcassone.Core.Calculation.Base.Tiles
{
    /// <summary>
    ///       F
    ///   |       |
    /// F |   O   | F
    ///   |   |   |
    ///       R
    /// </summary>
    public class FFRF : Tile
    {
        protected string roadPartName = "Road_0";
        protected string monasteryPartName = "Monastery_0";
        protected string FarmPartName = "Farm_0";

        public FFRF(string cardType, int cardNumber) : base(cardType, cardNumber)
        {
            var roadPart3 = new RoadPart(roadPartName, this);
            roadPart3.AddSide(Side.bottom);
            Parts.Add(roadPart3);

            var monasteryPart = new MonasteryPart(monasteryPartName, this);
            Parts.Add(monasteryPart);

            var FarmPart1 = new FarmPart(FarmPartName, this);
            FarmPart1.AddSide(Side.top);
            FarmPart1.AddSide(Side.right);
            FarmPart1.AddSide(Side.left);
            FarmPart1.AddSide(Side.side_3);
            FarmPart1.AddSide(Side.side_4);
            Parts.Add(FarmPart1);
        }
    }
}