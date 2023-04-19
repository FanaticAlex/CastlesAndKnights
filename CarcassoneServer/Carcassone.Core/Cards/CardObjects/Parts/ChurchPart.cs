namespace Carcassone.Core.Cards
{
    public class ChurchPart : ObjectPart
    {
        public string? ChurchFieldId { get; set; }

        public ChurchPart(string partName, string cardName)
            : base(partName, cardName)
        {
            PartType = "Church";
        }
    }
}