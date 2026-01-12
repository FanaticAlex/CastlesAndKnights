using Carcassone.Core.Tiles;
using System.Linq;

namespace Carcassone.Core.Calculation.Base.Roads
{
    public class Road : ClosingMultipartObject
    {
        public int GetPoints(Stack cardPool)
        {
            var score = 0;
            var parts = PartsIds.Select(id => cardPool.GetPart(id));
            foreach (RoadPart part in parts)
            {
                score++;
            }

            if (IsFinished)
            {
                score = score * 2;
            }

            return score;
        }
    }
}