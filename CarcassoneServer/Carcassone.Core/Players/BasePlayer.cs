using Carcassone.Core.Cards;
using System;
using System.Collections.Generic;

namespace Carcassone.Core.Players
{
    public abstract class BasePlayer
    {
        private List<Chip> _chipList = new List<Chip>();

        public BasePlayer(string name, string color, int chipCount)
        {
            Name = name;
            Color = color;

            for (var i = 0; i < chipCount; i++)
            {
                var chip = new Chip(this);
                _chipList.Add(chip);
            }
        }

        public string Name { get; set; }

        public string Color { get; set; }

        public string? LastCardId { get; set; }

        public int ChipCount => _chipList.Count;

        public Chip? TakeChip()
        {
            if (_chipList.Count == 0)
                return null;

            var chip = _chipList[0];
            _chipList.Remove(chip);
            return chip;
        }

        public void ReturnChipAndSetFlag(ObjectPart part)
        {
            if (part.Chip == null)
                throw new Exception("Объект не принадлежит игроку, невозможно установить флаг.");

            var chip = part.Chip;
            chip.OwnerName = Name;
            _chipList.Add(chip);

            part.Chip = null;
            part.Flag = new Flag(this);
        }
    }
}
