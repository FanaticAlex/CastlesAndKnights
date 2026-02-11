using Carcassone.Core.Players;
using Carcassone.Core.Tiles;
using System;
using System.Linq;

namespace Carcassone.Core.Calculation.Base.Cities
{
    public class City : MergableObject, ICompletableGameObject
    {
        public override int GetScore()
        {
            // one part gives one score point
            // part with shield gives two score point
            var score = Parts.Sum(part => ((CityPart)part).IsThereShield ? 2 : 1);
            
            if (IsComplete())
                score *= 2;

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