using Carcassone.Core.Tiles;
using System.Collections.Generic;

namespace Carcassone.Core.Calculation
{
    public interface IMultipartObject
    {
        public List<string> PartsIds { get; set; }

        bool CanConnect(ObjectPart part);
        void AddPart(ObjectPart part, Stack cardPool);
    }
}
