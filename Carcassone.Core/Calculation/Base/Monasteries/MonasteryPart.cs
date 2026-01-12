namespace Carcassone.Core.Calculation.Base.Monasteries
{
    public class MonasteryPart : ObjectPart
    {
        public string? CellId { get; set; }

        public MonasteryPart(string partName, string cardName)
            : base(partName, cardName)
        {
            PartType = "Monastery";
        }
    }
}