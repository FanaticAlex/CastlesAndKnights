using System.Collections;
using System.Collections.Generic;

namespace Carcassone.Core.Cards
{
    public class CastlePart : ObjectPart
    {
        public CastlePart(string partName, string cardName) : base(partName, cardName)
        {
            PartType = "Castle";
        }

        /// <summary>
        /// Есть ли на карте города щит.
        /// </summary>
        public bool IsThereShield;
    }
}