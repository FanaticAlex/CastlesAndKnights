using System;
using System.Collections.Generic;
using System.Text;

namespace Carcassone.Core.Calculation.RiverExtension.Rivers
{
    public class RiverPart : ObjectPart
    {
        public RiverPart(string partName, string cardName) : base(partName, cardName)
        {
            PartType = "River";
        }
    }
}
