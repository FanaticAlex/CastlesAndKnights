
namespace Carcassone.Core.Tiles
{
    public class FieldPart : ObjectPart
    {
        public FieldPart(string partName, string cardId) : base(partName, cardId)
        {
            PartType = "Field";
        }
    }
}