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

        public IEnumerable<BaseGameObject> CalculateObjects(Tile tile, GameState state)
        {
            var roads = state.GetLastState().Calculated.OfType<Road>();
            var copies = roads.Select(r => r.Copy()).ToList();

            foreach (var part in tile.Parts)
            {
                if (part is RoadPart)
                    _merger.ConnectPartWithObjects(copies, part, state);
            }

            return copies;
        }

        public IEnumerable<BaseGameObject> GetGameObjects(GameState state)
        {
            return state.GetLastState().Calculated.OfType<Road>();
        }
    }
}

