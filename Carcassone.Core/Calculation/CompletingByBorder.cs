using Carcassone.Core.Players;
using Carcassone.Core.Tiles;
using System.Linq;

namespace Carcassone.Core.Calculation
{
    interface ICompletableGameObject
    {
        bool IsComplete();
        void TryCompleteAndReturnMeeples();
    }

    static class ObjectCompletitionHelper
    {
        public static void TryCompleteAndReturnMeeples<T>(T obj) where T : BaseGameObject, ICompletableGameObject
        {
            if (obj.IsComplete())
            {
                foreach (var part in obj.Parts)
                {
                    part.Meeple?.Owner?.ReturnMeepleAndSetFlag(part);
                }
            }
        }

        public static bool IsCompleteByBorder(MergableObject obj)
        {
            return (obj.OpenBorders.Count == 0);
        }
    }
}

