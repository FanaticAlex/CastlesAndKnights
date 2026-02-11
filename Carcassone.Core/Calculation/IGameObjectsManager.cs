using Carcassone.Core.Board;
using Carcassone.Core.Players;
using Carcassone.Core.Tiles;
using System.Collections.Generic;

namespace Carcassone.Core.Calculation
{
    public interface IGameObjectsManager
    {
        IEnumerable<BaseGameObject> GetGameObjects();
        void ProcessPart(ObjectPart part, Cell cell);
    }
}

