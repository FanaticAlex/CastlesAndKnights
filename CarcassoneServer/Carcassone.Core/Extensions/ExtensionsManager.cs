using Carcassone.Core.Extensions.River;

namespace Carcassone.Core.Extensions
{
    public class ExtensionsManager
    {
        public ExtensionsManager()
        {
            RiverExtension = new RiverExtension();
        }

        public RiverExtension RiverExtension { get; private set; }
    }
}
