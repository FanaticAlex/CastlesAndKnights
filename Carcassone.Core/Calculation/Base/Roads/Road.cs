using Carcassone.Core.Players;
using Carcassone.Core.Tiles;
using System.Linq;

namespace Carcassone.Core.Calculation.Base.Roads
{
    public class Road : MergableObject, ICompletableGameObject
    {
        public override int GetScore()
        {
            var score = Parts.Count;

            if (IsComplete())
                score = score * 2;

            return score;
        }

        public bool IsComplete()
        {
            return ObjectCompletitionHelper.IsCompleteByBorder(this);
        }

        public void TryCompleteAndReturnChips()
        {
            ObjectCompletitionHelper.TryCompleteAndReturnChips(this);
        }
    }
}