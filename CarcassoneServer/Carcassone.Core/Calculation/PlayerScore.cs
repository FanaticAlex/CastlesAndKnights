using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carcassone.Core.Calculation
{
    public struct PlayerScore
    {
        public int Churches { get; set; }
        public int Cornfields { get; set; }
        public int Roads { get; set; }
        public int Castles { get; set; }
        public int ChipCount { get; set; }

        public int GetOverallScore()
        {
            return Churches + Cornfields + Roads + Castles;
        }
    }
}
