using Newtonsoft.Json;

namespace Carcassone.Core.Players
{
    public class Flag
    {
        public string OwnerName { get; set; }

        [JsonConstructor]
        public Flag() { }

        public Flag(GamePlayer owner)
        {
            OwnerName = owner.Name;
        }
    }
}
