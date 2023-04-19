using Carcassone.Core.Cards;
using Carcassone.Core.Players;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Carcassone.Core.Calculation.Objects
{
    public class ClosingMultipartObject: IClosingObject, IMultipartObject
    {
        public List<Border> OpenBorders = new List<Border>();

        [JsonProperty(ItemConverterType = typeof(PartConverter))]
        public List<ObjectPart> Parts { get; set; } = new List<ObjectPart>();

        public void AddPart(ObjectPart part)
        {
            Parts.Add(part);
            OpenBorders.AddRange(part.Borders);

            if (GetOwnersNames().Count > 0)
            {
                foreach (var part1 in Parts)
                    part1.IsPartOfOwnedObject = true;
            }
        }

        public bool CanConnect(ObjectPart part)
        {
            foreach (Border border in part.Borders)
            {
                var sameBorder = OpenBorders.Find(border2 => Border.Equial(border, border2));
                if (sameBorder != null)
                    return true;
            }

            return false;
        }

        public bool IsFinished { get; set; }

        public void TryToClose(PlayersPool playersPool)
        {
            IsFinished = IsClosed();
            if (IsFinished)
            {
                foreach (var part in Parts)
                {
                    var player = playersPool.GetPlayer(part.Chip?.OwnerName);
                    player?.ReturnChipAndSetFlag(part);
                }
            }
        }

        private bool IsClosed()
        {
            var isClosed1 = true;
            foreach (Border border in OpenBorders)
            {
                var isClosed = OpenBorders.FindAll(border2 => Border.Equial(border, border2)).Count == 2;
                if (!isClosed)
                {
                    isClosed1 = false;
                }
            }

            return isClosed1;
        }

        public bool IsPlayerOwner(BasePlayer player)
        {
            return GetOwnersNames().Contains(player.Name);
        }

        private List<string> GetOwnersNames()
        {
            // если объект закончен считаем по флагам владельцев
            // если не закончен считаем количество фишек
            // если у какого то игрока фишек в замке больше,
            // то он считается владельцем (может быть несколько владельцев)
            if (IsFinished)
                return GetOwnersByFlags();
            else
                return GetOwnersByChips();
        }

        private List<string> GetOwnersByFlags()
        {
            return Parts
                .Where(part => part.Flag?.OwnerName != null)
                .Select(part => part.Flag.OwnerName)
                .Distinct().ToList();
        }

        private List<string> GetOwnersByChips()
        {
            // количество фишек у каждого игрока
            var ownersToChipCount = new Dictionary<string, int>();
            foreach (var part in Parts)
            {
                if (part.Chip != null)
                {
                    if (ownersToChipCount.ContainsKey(part.Chip.OwnerName))
                    {
                        ownersToChipCount[part.Chip.OwnerName] += 1;
                    }
                    else
                    {
                        ownersToChipCount[part.Chip.OwnerName] = 1;
                    }
                }
            }

            // без владельцев
            if (!ownersToChipCount.Any())
                return new List<string>();

            // владельцы - игроки у которых максимальное число фишек на замке.
            var maxChip = ownersToChipCount.Values.Max();
            var owners = ownersToChipCount
                .Where(pair => pair.Value == maxChip)
                .Select(pair => pair.Key)
                .ToList();
            return owners;
        }
    }
}
