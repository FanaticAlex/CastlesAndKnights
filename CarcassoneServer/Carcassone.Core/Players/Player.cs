using Carcassone.Core.Cards;
using Carcassone.Core.Players.AI;
using System.Collections.Generic;
using System.Drawing;

namespace Carcassone.Core.Players
{
    public class Player
    {
        private PlayerAI _playerAI;
        private List<Chip> _chipList = new List<Chip>();

        public Player()
        {
        }

        public Player(string name, string color, bool isBot, int chipCount)
        {
            Name = name;
            Color = color;

            for (var i = 0; i < chipCount; i++)
            {
                var chip = new Chip();
                chip.Owner = this;
                _chipList.Add(chip);
            }

            if (isBot)
            {
                _playerAI = new PlayerAI();
            }
        }

        public string Name { get; set; }

        public string Color { get; set; }

        public string LastCardId { get; set; }

        public int ChipCount => _chipList.Count;

        public bool IsBot()
        {
            return _playerAI != null;
        }

        public void MakeMoveAI(GameRoom room)
        {
            _playerAI.MakeMove(room, this);
        }

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
            var chip = part.Chip;
            chip.Owner = this;
            _chipList.Add(chip);

            part.Chip = null;
            part.Flag = new Flag(this);
        }
    }
}