using System.Collections;
using System.Collections.Generic;

namespace Carcassone.Core.Cards
{
    public class CastlePart : ObjectPart
    {
        public CastlePart(string partName, string cardName, bool isThereShield = false) : base(partName, cardName)
        {
            PartType = "Castle";
            IsThereShield = isThereShield;
        }

        /// <summary>
        /// Есть ли на карте города щит.
        /// </summary>
        public bool IsThereShield { get; }
    }
}