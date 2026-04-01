using System.Linq;

namespace Carcassone.Core.Calculation
{
    public interface ICompletableGameObject
    {
        bool IsComplete(MoveState state);
    }

    static class ObjectCompletitionHelper
    {
        public static bool IsCompleteByBorder(MergableObject obj, MoveState state)
        {
            return (obj.GetOpenBorders(state).Count == 0);
        }
    }
}

