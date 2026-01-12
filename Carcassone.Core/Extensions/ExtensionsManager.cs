using Carcassone.Core.Calculation.River;

namespace Carcassone.Core.Extensions
{
    /// <summary>
    /// Define extensions of the game rules and cards.
    /// </summary>
    public class ExtensionsManager
    {
        public ExtensionsManager(bool enableRiverExtension)
        {
            if (enableRiverExtension)
                RiverExtension = new RiverExtension();
        }

        public RiverExtension? RiverExtension { get; private set; }
    }
}
