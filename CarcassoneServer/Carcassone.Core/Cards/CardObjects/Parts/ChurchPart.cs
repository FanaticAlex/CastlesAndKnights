using System.Collections.Generic;
using Carcassone.Core.Fields;

namespace Carcassone.Core.Cards
{
    public class ChurchPart : ObjectPart
    {
        public Field? ChurchField { get; set; }

        public ChurchPart(string partName, string cardName)
            : base(partName, cardName)
        {
            PartType = "Church";
        }
    }
}