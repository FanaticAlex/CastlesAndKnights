using Carcassone.Core.Cards;
using Carcassone.Core.Players;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Carcassone.Core.Calculation.Objects
{
    /// <summary>
    /// Объект замок для подсчета очков. состоит из частей на картах. Завершаемый.
    /// </summary>
    public class Castle : IClosingObject, IMultipartObject
    {
        /// <summary>
        /// Открытые границы замка.
        /// </summary>
        private List<Border> openBorders = new List<Border>();

        /// <summary>
        /// Если у замка есть открытые границы то он не закончен.
        /// </summary>
        public bool IsFinished { get; private set; }

        /// <summary>
        /// Часть замка, на одной карте может быть несколько частей замка.
        /// </summary>
        public List<ObjectPart> Parts { get; private set; } = new List<ObjectPart>();

        public List<ObjectPart> GetParts() => Parts;


        public bool IsPlayerOwner(BasePlayer player)
        {
            return GetOwners().Select(p => p.Name).Contains(player.Name);
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
        /// Возвращает количество очков за замок.
        /// </summary>
        /// <returns></returns>
        public int GetPoints()
        {
            // тест
            foreach (var part in Parts)
            {
                try
                {
                    var part1 = (CastlePart)part;
                }
                catch (InvalidCastException ex)
                {
                    throw new Exception($"Неверный тип присоединенной части замка {part.PartName}. Карта {part.CardName}. Тип {part.GetType()}", ex);
                }
            }

            // за каждую часть замка по 1 очку, за карту замка со щитом 2 очка.
            // если замок завершен очки удваиваются
            var score = Parts.Sum(part => ((CastlePart)part).IsThereShield ? 2 : 1);
            if (IsFinished)
                score = score * 2;

            return score;
        }

        public void AddPart(ObjectPart part)
        {
            Parts.Add(part);
            openBorders.AddRange(part.Borders);

            if (GetOwners().Count > 0)
            {
                foreach (var part1 in Parts)
                    part1.IsOwned = true;
            }
        }

        public bool CanConnect(ObjectPart part)
        {
            foreach (Border border in part.Borders)
            {
                var sameBorder = openBorders.Find(border2 => Border.Equial(border, border2));
                if (sameBorder != null)
                    return true;
            }

            return false;
        }

        private List<BasePlayer> GetOwnersByFlags()
        {
            return Parts
                .Where(part => part.Flag?.Owner != null)
                .Select(part => part.Flag.Owner)
                .Distinct().ToList();
        }

        private List<BasePlayer> GetOwnersByChips()
        {
            // количество фишек у каждого игрока
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

            // замок без владельцев
            if (!ownersToChipCount.Any())
                return new List<BasePlayer>();

            // владельцы - игроки у которых максимальное число фишек на замке.
            var maxChip = ownersToChipCount.Values.Max();
            var owners = ownersToChipCount.Where(pair => pair.Value == maxChip).Select(pair => pair.Key).ToList();
            return owners;
        }

        private bool IsClosed()
        {
            var isClosed1 = true;
            foreach (Border border in openBorders)
            {
                var isClosed = openBorders.FindAll(border2 => Border.Equial(border, border2)).Count == 2;
                if (!isClosed)
                {
                    isClosed1 = false;
                }
            }

            return isClosed1;
        }

        private List<BasePlayer> GetOwners()
        {
            // если замок закончен считаем по флагам владельцев
            // если не закончен считаем количество фишек
            // если у какого то игрока фишек в замке больше,
            // то он считается владельцем (может быть несколько владельцев)
            if (IsFinished)
                return GetOwnersByFlags();
            else
                return GetOwnersByChips();
        }
    }
}