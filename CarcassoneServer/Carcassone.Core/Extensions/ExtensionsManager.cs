using Carcassone.Core.Extensions.River;

namespace Carcassone.Core.Extensions
{
    public class ExtensionsManager
    {
        public bool EnableRiverExtension { get; set; }

        public ExtensionsManager(bool enableRiverExtension)
        {
            if (enableRiverExtension)
            {
                EnableRiverExtension = enableRiverExtension;
                RiverExtension = new RiverExtension();
            }
        }

        public RiverExtension RiverExtension { get; private set; }
    }
}
