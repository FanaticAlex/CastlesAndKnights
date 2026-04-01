using Carcassone.Core.Players;
using System.Collections.Generic;
using System.Linq;

namespace Carcassone.Core.Calculation
{
    public abstract class BaseGameObject
    {
        public List<ObjectPart> Parts { get; set; } = new List<ObjectPart>();
        
        public abstract int GetScore(MoveState state); // abstract: different types of objects could count scores differently

        public abstract BaseGameObject Copy();

        public virtual bool IsPlayerOwner(string playerName, MoveState state)
        {
            return GetObjectOwners(state).Contains(playerName);
        }

        public bool HasOwner(MoveState state)
        {
            return GetObjectOwners(state).Any();
        }

        private IEnumerable<string> GetObjectOwners(MoveState state)
        {
            // owners are players with a maximum meeples on an object's parts

            var ownersToMeepleCount = new Dictionary<string, int>();
            foreach (var part in Parts)
            {
                var owner = part.GetPartOwner(state);
                if (owner != null)
                {
                    if (ownersToMeepleCount.ContainsKey(owner))
                        ownersToMeepleCount[owner] += 1;
                    else
                        ownersToMeepleCount[owner] = 1;
                }
            }

            // without owners
            if (!ownersToMeepleCount.Any())
                return new List<string>();

            var maxMeeples = ownersToMeepleCount.Values.Max();
            var owners = ownersToMeepleCount
                .Where(pair => pair.Value == maxMeeples)
                .Select(pair => pair.Key)
                .ToList();
            return owners;
        }
    }
}

