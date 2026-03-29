using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Tiles;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Carcassone.Core.Calculation.Base.Farms
{
    public class FarmsManager : IGameObjectsManager
    {
        private ObjectsMerger<Farm> _merger = new ObjectsMerger<Farm>();

        public IEnumerable<BaseGameObject> CalculateObjects(Tile tile, GameState state)
        {
            var farms = state.GetLastState().Calculated.OfType<Farm>();
            var copies = farms.Select(f => f.Copy()).ToList();

            foreach (var part in tile.Parts)
            {
                if (part is FarmPart)
                    _merger.ConnectPartWithObjects(copies, part, state);
            }

            return copies;
        }

        public IEnumerable<BaseGameObject> GetGameObjects(GameState state)
        {
            return state.GetLastState().Calculated.OfType<Farm>();
        }
    }
}

