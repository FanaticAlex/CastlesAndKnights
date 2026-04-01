using System.Linq;

namespace Carcassone.Core.Calculation.Base.Roads
{
    public class Road : MergableObject, ICompletableGameObject
    {
        public override int GetScore(MoveState state)
        {
            var score = Parts.Count;

            if (IsComplete(state))
                score = score * 2;

            return score;
        }

        public bool IsComplete(MoveState state)
        {
            return ObjectCompletitionHelper.IsCompleteByBorder(this, state);
        }

        public override BaseGameObject Copy()
        {
            var copy = new Road();
            copy.Parts.AddRange(Parts);
            return copy;
        }
    }
}