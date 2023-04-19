using Carcassone.Core.Cards;
using Carcassone.Core.Players;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Carcassone.Core.Calculation.Objects
{
    public class Road : ClosingMultipartObject
    {
        public int GetPoints()
        {
            var score = 0;
            foreach (RoadPart part in Parts)
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