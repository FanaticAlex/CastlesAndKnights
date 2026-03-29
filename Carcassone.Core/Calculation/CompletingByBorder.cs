using Carcassone.Core.Players;
using Carcassone.Core.Tiles;
using System.Linq;

namespace Carcassone.Core.Calculation
{
    interface ICompletableGameObject
    {
        bool IsComplete(GameState state);
    }

    static class ObjectCompletitionHelper
    {
        public static bool IsCompleteByBorder(MergableObject obj, GameState state)
        {
            return (obj.GetOpenBorders(state).Count == 0);
        }
    }
}

