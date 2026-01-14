namespace Carcassone.Core.Calculation.Base.Farms
{
    // farm part is also called field
    public class FarmPart : ObjectPart
    {
        public FarmPart(string partName, string cardId) : base(partName, cardId)
        {
            PartType = "Farm";
        }
    }
}