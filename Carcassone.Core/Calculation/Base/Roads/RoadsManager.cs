using Carcassone.Core.Board;
using Carcassone.Core.Calculation.Base.Cities;
using Carcassone.Core.Calculation.RiverExtension.Rivers;
using Carcassone.Core.Players;
using Carcassone.Core.Tiles;
using System.Collections.Generic;
using System.Linq;

namespace Carcassone.Core.Calculation.Base.Roads
{
    /// <summary>
    /// rules of managing roads
    /// </summary>
    public class RoadsManager : IGameObjectsManager
    {
        private ObjectsMerger<Road> _merger = new ObjectsMerger<Road>();

        public List<Road> Roads { get; set; } = new List<Road>();

        public IEnumerable<BaseGameObject> GetGameObjects()
        {
            return Roads;
        }

        public int GetPlayerScore(GamePlayer player)
        {
            return Roads
                .Where(r => r.IsPlayerOwner(player))
                .Select(r => r.GetScore())
                .Sum();
        }

        public void ProcessPart(ObjectPart part, Cell cell)
        {
            if (!(part is RoadPart)) return;

            _merger.ProcessPart(Roads, part);

            foreach (var road in Roads)
                road.TryCompleteAndReturnChips();
        }
    }
}

