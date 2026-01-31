using Carcassone.Core.Board;
using Carcassone.Core.Players;
using Carcassone.Core.Tiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Carcassone.Core.Calculation.RiverExtension.Rivers
{
    // TODO: Redundant
    public class RiversManager : IGameObjectsManager
    {
        public River River { get; set; } = new River();

        public IEnumerable<BaseGameObject> GetGameObjects()
        {
            return new List<River>() { River };
        }

        public int GetPlayerScore(GamePlayer player)
        {
            return 0;
        }

        public void ProcessPart(ObjectPart part, Cell cell)
        {
            if (!(part is RiverPart)) return;

            River.Parts.Add(part);
        }
    }
}
