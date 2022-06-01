using System.Collections.Generic;

namespace Carcassone.Core.Cards
{
    public class RoadPart : ObjectPart
    {
        public RoadPart(string partName, string cardName) : base(partName, cardName)
        {
            PartType = "Road";
        }
    }
}