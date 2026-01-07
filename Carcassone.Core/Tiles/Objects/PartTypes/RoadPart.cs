using System.Collections.Generic;

namespace Carcassone.Core.Tiles
{
    public class RoadPart : ObjectPart
    {
        public RoadPart(string partName, string cardName) : base(partName, cardName)
        {
            PartType = "Road";
        }
    }
}