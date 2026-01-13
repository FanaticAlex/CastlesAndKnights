using Carcassone.Core.Tiles;
using Carcassone.Core.Players;
using System;
using System.Collections.Generic;
using System.Text;

namespace Carcassone.Core.Calculation
{
    public interface IOwnedObject
    {
        public bool IsPlayerOwner(GamePlayer player, TileStack cardPool);
    }
}
