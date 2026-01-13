using Carcassone.Core.Tiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Carcassone.Core.Calculation.RiverExtension.Rivers
{
    public class RiversManager
    {
        public River River { get; set; } = new River();

        public void ProcessPart(ObjectPart part, TileStack tilesStack)
        {
            if (River.PartsIds.Count == 0) // starting the river
            {
                River.AddPart(part, tilesStack);
                return;
            }

            if (River.CanConnect(part))
            {
                River.AddPart(part, tilesStack);
                return;
            }
             
            throw new Exception("Can't connect river part");
        }
    }
}
