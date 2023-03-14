
namespace Carcassone.Core.Players
{
    public class Chip
    {
        public Chip(BasePlayer owner)
        {
            Type = ChipType.None;
            Owner = owner;
        }

        public ChipType Type { get; set; }

        public BasePlayer Owner { get; set; }
    }
}