using System;
using System.Linq;

namespace Carcassone.Core.Calculation.Base.Cities
{
    public class City : MergableObject, ICompletableGameObject
    {
        public override int GetScore(MoveState state)
        {
            // one part gives one score point
            // part with shield gives two score point
            var score = Parts.Sum(part => ((CityPart)part).IsThereShield ? 2 : 1);
            
            if (IsComplete(state))
                score *= 2;

            return score;
        }

        public bool IsComplete(MoveState state)
        {
            return ObjectCompletitionHelper.IsCompleteByBorder(this, state);
        }

        public override BaseGameObject Copy()
        {
            var copy = new City();
            copy.Parts.AddRange(Parts);
            return copy;
        }
    }
}