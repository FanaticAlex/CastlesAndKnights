
namespace Carcassone.Core.Players
{
    public class Flag
    {
        public GamePlayer Owner { get; set; }

        public Flag(GamePlayer owner)
        {
            Owner = owner;
        }
    }
}
