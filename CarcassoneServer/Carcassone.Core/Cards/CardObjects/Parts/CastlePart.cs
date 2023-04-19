namespace Carcassone.Core.Cards
{
    public class CastlePart : ObjectPart
    {
        public CastlePart(string partName, string cardId, bool isThereShield = false)
            : base(partName, cardId)
        {
            PartType = "Castle";
            IsThereShield = isThereShield;
        }

        /// <summary>
        /// Есть ли на карте города щит.
        /// </summary>
        public bool IsThereShield { get; set; }
    }
}