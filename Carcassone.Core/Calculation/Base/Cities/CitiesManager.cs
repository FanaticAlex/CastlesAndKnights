using Carcassone.Core.Board;
using Carcassone.Core.Calculation.Base.Roads;
using Carcassone.Core.Players;
using Carcassone.Core.Tiles;
using System.Collections.Generic;
using System.Linq;

namespace Carcassone.Core.Calculation.Base.Cities
{
    public class CitiesManager : IGameObjectsManager
    {
        private ObjectsMerger<City> _merger = new ObjectsMerger<City>();

        public List<City> Cities { get; set; } = new List<City>();

        public int GetPlayerScore(GamePlayer player)
        {
            return Cities
                .Where(c => c.IsPlayerOwner(player))
                .Select(c => c.GetScore())
                .Sum();
        }

        public void ProcessPart(ObjectPart part, Cell cell)
        {
            if (!(part is CityPart)) return;

            _merger.ProcessPart(Cities, part);

            foreach (City city in Cities)
                city.TryCompleteAndReturnChips();
        }

        public List<City> GetCompleteCities()
        {
            return Cities.Where(c => c.IsComplete()).ToList();
        }

        public IEnumerable<BaseGameObject> GetGameObjects()
        {
            return Cities;
        }
    }
}

