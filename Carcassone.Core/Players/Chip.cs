using Newtonsoft.Json;

namespace Carcassone.Core.Players
{
    public class Chip
    {
        [JsonConstructor]
        public Chip()
        { }

        public Chip(GamePlayer owner)
        {
            Type = ChipType.None;
            OwnerName = owner.Name;
        }

        public ChipType Type { get; set; }

        public string OwnerName { get; set; }
    }
}