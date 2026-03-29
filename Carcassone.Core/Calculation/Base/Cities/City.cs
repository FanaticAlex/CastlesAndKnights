using Carcassone.Core.Players;
using Carcassone.Core.Tiles;
using System;
using System.Linq;

namespace Carcassone.Core.Calculation.Base.Farms
{
    public class City : MergableObject, ICompletableGameObject
    {
        public override int GetScore(GameState state)
        {
            // one part gives one score point
            // part with shield gives two score point
            var score = Parts.Sum(part => ((CityPart)part).IsThereShield ? 2 : 1);
            
            if (IsComplete(state))
                score *= 2;

            return score;
        }

        public bool IsComplete(GameState state)
        {
            return ObjectCompletitionHelper.IsCompleteByBorder(this, state);
        }

        public City Copy()
        {
            var copy = new City();
            copy.Parts.AddRange(Parts);
            return copy;
        }
    }
}