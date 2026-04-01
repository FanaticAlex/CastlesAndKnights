using Carcassone.Core.Tiles;
using System.Collections.Generic;

namespace Carcassone.Core.Calculation
{
    public interface IGameObjectsManager
    {
        void CalculateObjects(Tile tile, MoveState state);
    }
}

