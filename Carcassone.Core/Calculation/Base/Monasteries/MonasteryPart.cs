using Carcassone.Core.Tiles;

namespace Carcassone.Core.Calculation.Base.Monasteries
{
    public class MonasteryPart : ObjectPart
    {
        public MonasteryPart(string partName, Tile tile)
            : base(partName, tile)
        {
            PartType = "Monastery";
        }
    }
}