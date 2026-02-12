using Carcassone.Core.Calculation.Base.Cities;
using Carcassone.Core.Tiles;
using System.Collections.Generic;
using System.Linq;

namespace Carcassone.Core.Calculation.Base.Farms
{
    public class FarmsManager : IGameObjectsManager
    {
        private ObjectsMerger<Farm> _merger = new ObjectsMerger<Farm>();
        private CitiesManager _citiesManager;

        public List<Farm> Farms { get; set; } = new List<Farm>();

        public FarmsManager(CitiesManager citiesManager)
        {
            _citiesManager = citiesManager;
        }

        public void ProcessPart(ObjectPart part, Tile tile)
        {
            if (!(part is FarmPart)) return;

            _merger.ProcessPart(Farms, part);

            // we need to set manager to count score at farm, there is no other way to do it
            foreach (var farm in Farms)
                farm.SetCitiesManager(_citiesManager);
        }

        public IEnumerable<BaseGameObject> GetGameObjects()
        {
            return Farms;
        }
    }
}

