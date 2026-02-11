using Carcassone.Core.Players;
using Carcassone.Core.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carcassone.Core.Calculation
{
    public static class HasOwnerHelper
    {
        public static bool HasOwner(BaseGameObject obj)
        {
            return GetOwners(obj).Any();
        }

        public static bool IsPlayerOwner(string playerName, BaseGameObject obj)
        {
            return GetOwners(obj).Contains(playerName);
        }

        private static IEnumerable<string> GetOwners(BaseGameObject obj)
        {
            // if there is flags count only flags (object completed)
            var flagOwners = obj.Parts
                .Where(part => part.Flag?.Owner != null)
                .Select(part => part.Flag.Owner)
                .Distinct()
                .ToList();

            if (flagOwners.Any()) return flagOwners.Select(p => p.Info.Name);


            // if object is not completed count chips
            var ownersToChipCount = new Dictionary<GamePlayer, int>();
            foreach (var part in obj.Parts)
            {
                if (part.Chip != null)
                {
                    if (ownersToChipCount.ContainsKey(part.Chip.Owner))
                    {
                        ownersToChipCount[part.Chip.Owner] += 1;
                    }
                    else
                    {
                        ownersToChipCount[part.Chip.Owner] = 1;
                    }
                }
            }

            // without owners
            if (!ownersToChipCount.Any())
                return new List<string>();

            // владельцы - игроки у которых максимальное число фишек на обьекте.
            var maxChip = ownersToChipCount.Values.Max();
            var owners = ownersToChipCount
                .Where(pair => pair.Value == maxChip)
                .Select(pair => pair.Key)
                .ToList();
            return owners.Select(p => p.Info.Name);
        }
    }
}

