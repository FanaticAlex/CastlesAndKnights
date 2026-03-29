using Carcassone.Core.Tiles;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Carcassone.Core.Calculation.Base.Monasteries
{
    public class MonasteriesManager : IGameObjectsManager
    {
        public IEnumerable<BaseGameObject> CalculateObjects(Tile tile, GameState state)
        {
            var monasteries = state.GetLastState().Calculated.OfType<Monastery>();
            var copies = monasteries.Select(m => m.Copy()).ToList();

            foreach (var part in tile.Parts)
            {
                if (part is MonasteryPart)
                {
                    var item = new Monastery((MonasteryPart)part, tile);
                    copies.Add(item);
                }
            }

            return copies;
        }

        public IEnumerable<BaseGameObject> GetGameObjects(GameState state)
        {
            return state.GetLastState().Calculated.OfType<Monastery>();
        }
    }
}

