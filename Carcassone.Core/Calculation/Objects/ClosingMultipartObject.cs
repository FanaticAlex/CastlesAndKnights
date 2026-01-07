using Carcassone.Core.Tiles;
using Carcassone.Core.Players;
using System.Collections.Generic;
using System.Linq;

namespace Carcassone.Core.Calculation.Objects
{
    public class ClosingMultipartObject: IClosingObject, IMultipartObject, IOwnedObject
    {
        public List<CardBorder> OpenBorders = new List<CardBorder>();

        public List<string> PartsIds { get; set; } = new List<string>();

        public void AddPart(ObjectPart part, Stack cardPool)
        {
            PartsIds.Add(part.PartId);
            OpenBorders.AddRange(part.Borders);

            if (GetOwnersNames(cardPool).Count > 0)
            {
                foreach (var partId in PartsIds)
                {
                    var part1 = cardPool.GetPart(partId);
                    part1.IsPartOfOwnedObject = true;
                }
            }
        }

        public bool CanConnect(ObjectPart part)
        {
            foreach (CardBorder border in part.Borders)
            {
                var sameBorder = OpenBorders.Find(border2 => CardBorder.Equial(border, border2));
                if (sameBorder != null)
                    return true;
            }

            return false;
        }

        public bool IsFinished { get; set; }

        public void TryToClose(GamePlayersPool playersPool, Stack cardPool)
        {
            IsFinished = IsClosed();
            if (IsFinished)
            {
                foreach (var partId in PartsIds)
                {
                    var part = cardPool.GetPart(partId);
                    if (part.Chip?.OwnerName != null)
                    {
                        var player = playersPool.GetPlayer(part.Chip.OwnerName);
                        player.ReturnChipAndSetFlag(part);
                    }
                }
            }
        }

        private bool IsClosed()
        {
            var isClosed1 = true;
            foreach (CardBorder border in OpenBorders)
            {
                var isClosed = OpenBorders.FindAll(border2 => CardBorder.Equial(border, border2)).Count == 2;
                if (!isClosed)
                {
                    isClosed1 = false;
                }
            }

            return isClosed1;
        }

        public bool IsPlayerOwner(GamePlayer player, Stack cardPool)
        {
            return GetOwnersNames(cardPool).Contains(player.Name);
        }

        private List<string> GetOwnersNames(Stack cardPool)
        {
            // если объект закончен считаем по флагам владельцев
            // если не закончен считаем количество фишек
            // если у какого то игрока фишек в замке больше,
            // то он считается владельцем (может быть несколько владельцев)
            if (IsFinished)
                return GetOwnersByFlags(cardPool);
            else
                return GetOwnersByChips(cardPool);
        }

        private List<string> GetOwnersByFlags(Stack cardPool)
        {
            var parts = PartsIds.Select(id => cardPool.GetPart(id));
            return parts
                .Where(part => part.Flag?.OwnerName != null)
                .Select(part => part.Flag.OwnerName)
                .Distinct().ToList();
        }

        private List<string> GetOwnersByChips(Stack cardPool)
        {
            // количество фишек у каждого игрока
            var ownersToChipCount = new Dictionary<string, int>();
            var parts = PartsIds.Select(id => cardPool.GetPart(id));
            foreach (var part in parts)
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
