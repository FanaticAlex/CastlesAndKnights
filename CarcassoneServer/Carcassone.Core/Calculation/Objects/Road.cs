using Carcassone.Core.Cards;
using Carcassone.Core.Players;
using System.Collections.Generic;
using System.Linq;

namespace Carcassone.Core.Calculation.Objects
{
    public class Road : IClosingObject, IMultipartObject
    {
        public bool IsFinished { get; set; }

        private List<Border> OpenBorders = new List<Border>();

        public List<ObjectPart> Parts { get; set; } = new List<ObjectPart>();

        public List<ObjectPart> GetParts() => Parts;

        public bool IsPlayerOwner(BasePlayer player)
        {
            return GetOwners().Select(p => p.Name).Contains(player.Name);
        }

        public int GetPoints()
        {
            var score = 0;
            foreach (RoadPart part in Parts)
            {
                score++;
            }

            if (IsFinished)
            {
                score = score * 2;
            }

            return score;
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

        public void AddPart(ObjectPart part)
        {
            Parts.Add(part);
            OpenBorders.AddRange(part.Borders);

            if (GetOwners().Count > 0)
            {
                foreach (var part1 in Parts)
                    part1.IsOwned = true;
            }
        }

        public void TryToClose()
        {
            IsFinished = IsClosed();
            if (IsFinished)
            {
                foreach (var part in GetParts())
                    part.Chip?.Owner.ReturnChipAndSetFlag(part);
            }
        }

        /// <summary>
        /// Зактрыто если нет открытых границ
        /// </summary>
        /// <returns></returns>
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

        private List<BasePlayer> GetOwners()
        {
            var owners = new List<BasePlayer>();
            if (IsFinished)
            {
                foreach (var part in Parts)
                {
                    if (part.Flag != null)
                    {
                        owners.Add(part.Flag.Owner);
                    }
                }

                return owners;
            }

            var ownersToChipCount = new Dictionary<BasePlayer, int>();
            foreach (var part in Parts)
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

            var maxChip = 0;
            foreach (int value in ownersToChipCount.Values)
            {
                if (value > maxChip)
                {
                    maxChip = value;
                }
            }

            foreach (var player in ownersToChipCount.Keys)
            {
                if (ownersToChipCount[player] == maxChip)
                {
                    owners.Add(player);
                }
            }

            return owners;
        }
    }
}