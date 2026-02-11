using Carcassone.Core.Board;
using Carcassone.Core.Calculation.Base.Cities;
using Carcassone.Core.Calculation.Base.Roads;
using Carcassone.Core.Players;
using System.Collections.Generic;
using System.Linq;

namespace Carcassone.Core.Calculation.Base.Monasteries
{
    public class MonasteriesManager : IGameObjectsManager
    {
        public List<Monastery> Monasteries { get; set; } = new List<Monastery>();
        public Grid _grid;

        public MonasteriesManager(Grid grid)
        {
            _grid = grid;
        }

        public void ProcessPart(ObjectPart part, Cell cell)
        {
            if (!(part is MonasteryPart)) return;

            var church = new Monastery((MonasteryPart)part, cell, _grid);
            Monasteries.Add(church);

            foreach (var monastery in Monasteries)
                monastery.TryCompleteAndReturnChips();
        }

        public IEnumerable<BaseGameObject> GetGameObjects()
        {
            return Monasteries;
        }
    }
}

