using Carcassone.Core.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carcassone.Core.Calculation.Objects
{
    public interface IMultipartObject
    {
        bool CanConnect(ObjectPart part);
        void AddPart(ObjectPart part);
        List<ObjectPart> GetParts();
    }
}
