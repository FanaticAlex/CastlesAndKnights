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

        public void CalculateObjects(Tile tile, MoveState state)
        {
            foreach (var part in tile.Parts)
            {
                if (part is FarmPart)
                    _merger.ConnectPartWithObjects(state.Calculated, part, state);
            }
        }
    }
}

