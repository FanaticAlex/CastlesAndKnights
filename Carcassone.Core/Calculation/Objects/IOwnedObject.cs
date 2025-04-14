using Carcassone.Core.Cards;
using Carcassone.Core.Players;
using System;
using System.Collections.Generic;
using System.Text;

namespace Carcassone.Core.Calculation.Objects
{
    public interface IOwnedObject
    {
        public bool IsPlayerOwner(GamePlayer player, CardPool cardPool);
    }
}
