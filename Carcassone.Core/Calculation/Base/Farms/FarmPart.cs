using Carcassone.Core.Tiles;

namespace Carcassone.Core.Calculation.Base.Farms
{
    // farm part is also called field
    public class FarmPart : ObjectPart
    {
        public FarmPart(string partName, Tile tile) : base(partName, tile)
        {
            PartType = "Farm";
        }
    }
}