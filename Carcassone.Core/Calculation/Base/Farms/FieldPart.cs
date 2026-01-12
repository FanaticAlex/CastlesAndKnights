namespace Carcassone.Core.Calculation.Base.Farms
{
    public class FieldPart : ObjectPart
    {
        public FieldPart(string partName, string cardId) : base(partName, cardId)
        {
            PartType = "Field";
        }
    }
}