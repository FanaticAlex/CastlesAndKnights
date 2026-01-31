using Carcassone.Core.Tiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Carcassone.Core.Calculation.RiverExtension.Rivers
{
    public class RiverPart : ObjectPart
    {
        public RiverPart(string partName, Tile tile) : base(partName, tile)
        {
            PartType = "River";
        }
    }
}
