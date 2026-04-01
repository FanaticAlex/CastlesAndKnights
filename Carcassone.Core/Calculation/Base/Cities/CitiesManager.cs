using Carcassone.Core.Tiles;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Carcassone.Core.Calculation.Base.Cities
{
    public class CitiesManager : IGameObjectsManager
    {
        private ObjectsMerger<City> _merger = new ObjectsMerger<City>();

        public void CalculateObjects(Tile tile, MoveState state)
        {
            foreach (var part in tile.Parts)
            {
                if (part is CityPart)
                    _merger.ConnectPartWithObjects(state.Calculated, part, state);
            }
        }
    }
}

