
namespace Carcassone.Core.Players
{
    public class Meeple
    {
        public Meeple(GamePlayer owner)
        {
            Owner = owner;
        }

        public GamePlayer Owner { get; set; }
    }
}