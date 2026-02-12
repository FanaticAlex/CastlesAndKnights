
namespace Carcassone.Core.Players
{
    public class Meeple
    {
        public Meeple(GamePlayer owner)
        {
            Type = MeepleType.None;
            Owner = owner;
        }

        public MeepleType Type { get; set; }

        public GamePlayer Owner { get; set; }
    }
}