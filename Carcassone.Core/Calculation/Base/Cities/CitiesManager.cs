using Carcassone.Core.Tiles;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Carcassone.Core.Calculation.Base.Farms
{
    public class CitiesManager : IGameObjectsManager
    {
        private ObjectsMerger<City> _merger = new ObjectsMerger<City>();

        public IEnumerable<BaseGameObject> CalculateObjects(Tile tile, GameState state)
        {
            var cities = state.GetLastState().Calculated.OfType<City>();
            var copies = cities.Select(o => o.Copy()).ToList();

            foreach (var part in tile.Parts)
            {
                if (part is CityPart)
                    _merger.ConnectPartWithObjects(copies, part, state);
            }

            return copies;
        }

        public IEnumerable<BaseGameObject> GetGameObjects(GameState state)
        {
            return state.GetLastState().Calculated.OfType<City>();
        }
    }
}

