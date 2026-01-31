using Carcassone.Core.Tiles;
using System.Collections.Generic;

namespace Carcassone.Core.Calculation.Base.Roads
{
    public class RoadPart : ObjectPart
    {
        public RoadPart(string partName, Tile tile) : base(partName, tile)
        {
            PartType = "Road";
        }
    }
}