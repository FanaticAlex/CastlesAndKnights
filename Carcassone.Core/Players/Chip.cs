using Newtonsoft.Json;

namespace Carcassone.Core.Players
{
    public class Chip
    {
        public Chip(GamePlayer owner)
        {
            Type = ChipType.None;
            Owner = owner;
        }

        public ChipType Type { get; set; }

        public GamePlayer Owner { get; set; }
    }
}