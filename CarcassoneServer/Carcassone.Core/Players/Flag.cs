namespace Carcassone.Core.Players
{
    public class Flag
    {
        public string OwnerName { get; set; }

        public Flag(BasePlayer owner)
        {
            OwnerName = owner?.Name;
        }
    }
}
