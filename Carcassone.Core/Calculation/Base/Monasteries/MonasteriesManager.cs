using Carcassone.Core.Tiles;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Carcassone.Core.Calculation.Base.Monasteries
{
    public class MonasteriesManager : IGameObjectsManager
    {
        public void CalculateObjects(Tile tile, MoveState state)
        {
            foreach (var part in tile.Parts)
            {
                if (part is MonasteryPart)
                {
                    var item = new Monastery((MonasteryPart)part, tile);
                    state.Calculated.Add(item);
                }
            }
        }
    }
}

