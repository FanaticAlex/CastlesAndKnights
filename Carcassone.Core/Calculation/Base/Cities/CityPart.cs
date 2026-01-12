namespace Carcassone.Core.Calculation.Base.Cities
{
    public class CityPart : ObjectPart
    {
        public CityPart(string partName, string cardId, bool isThereShield = false)
            : base(partName, cardId)
        {
            PartType = "City";
            IsThereShield = isThereShield;
        }

        /// <summary>
        /// Есть ли на карте города щит.
        /// </summary>
        public bool IsThereShield { get; set; }
    }
}