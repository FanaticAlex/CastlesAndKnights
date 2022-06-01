
namespace Carcassone.Core.Players
{
    public class Chip
    {
        public Chip()
        {
        }

        public ChipType Type { get; set; }

        public Player Owner { get; set; }
    }
}