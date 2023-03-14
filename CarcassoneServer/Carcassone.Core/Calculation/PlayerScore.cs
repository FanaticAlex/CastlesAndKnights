using Carcassone.Core.Calculation.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carcassone.Core.Calculation
{
    public class PlayerScore
    {
        public PlayerScore(int churches, int cornfields, int roads, int castles, int chipCount)
        {
            Churches = churches;
            Cornfields = cornfields;
            Roads = roads;
            Castles = castles;
            ChipCount = chipCount;
        }

        public int Churches { get; }
        public int Cornfields { get; }
        public int Roads { get; }
        public int Castles { get; }
        public int ChipCount { get; }

        public int GetOverallScore()
        {
            return Churches + Cornfields + Roads + Castles;
        }
    }
}
