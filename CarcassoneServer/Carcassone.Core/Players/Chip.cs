
namespace Carcassone.Core.Players
{
    public class Chip
    {
        public Chip(BasePlayer owner)
        {
            Type = ChipType.None;
            OwnerName = owner?.Name;
        }

        public ChipType Type { get; set; }

        public string OwnerName { get; set; }
    }
}