using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Players;
using Carcassone.Core.Tiles;
using System.Linq;

namespace Carcassone.Core.Calculation.Base.Roads
{
    public class Road : MergableObject, ICompletableGameObject
    {
        public override int GetScore(GameState state)
        {
            var score = Parts.Count;

            if (IsComplete(state))
                score = score * 2;

            return score;
        }

        public bool IsComplete(GameState state)
        {
            return ObjectCompletitionHelper.IsCompleteByBorder(this, state);
        }

        public Road Copy()
        {
            var copy = new Road();
            copy.Parts.AddRange(Parts);
            return copy;
        }
    }
}