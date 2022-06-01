using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carcassone.Core.Players
{
    public class Flag
    {
        public Player Owner { get; set; }

        public Flag(Player owner)
        {
            Owner = owner;
        }
    }
}
