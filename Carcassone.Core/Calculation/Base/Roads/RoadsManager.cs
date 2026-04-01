using Carcassone.Core.Tiles;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Carcassone.Core.Calculation.Base.Roads
{
    /// <summary>
    /// rules of managing roads
    /// </summary>
    public class RoadsManager : IGameObjectsManager
    {
        private ObjectsMerger<Road> _merger = new ObjectsMerger<Road>();

        public void CalculateObjects(Tile tile, MoveState state)
        {
            foreach (var part in tile.Parts)
            {
                if (part is RoadPart)
                    _merger.ConnectPartWithObjects(state.Calculated, part, state);
            }
        }
    }
}

