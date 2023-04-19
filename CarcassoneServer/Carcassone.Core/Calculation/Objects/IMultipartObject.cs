using Carcassone.Core.Cards;
using System.Collections.Generic;

namespace Carcassone.Core.Calculation.Objects
{
    public interface IMultipartObject
    {
        public List<ObjectPart> Parts { get; set; }

        bool CanConnect(ObjectPart part);
        void AddPart(ObjectPart part);
    }
}
