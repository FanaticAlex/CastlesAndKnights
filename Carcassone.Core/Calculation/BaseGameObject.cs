using Carcassone.Core.Players;
using System.Collections.Generic;

namespace Carcassone.Core.Calculation
{
    public abstract class BaseGameObject
    {
        public List<ObjectPart> Parts { get; set; } = new List<ObjectPart>();
        public abstract int GetScore();
        public virtual bool IsPlayerOwner(string playerName)
        {
            return HasOwnerHelper.IsPlayerOwner(playerName, this);
        }
    }
}

