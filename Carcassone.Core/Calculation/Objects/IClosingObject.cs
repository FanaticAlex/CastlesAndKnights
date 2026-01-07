using Carcassone.Core.Tiles;
using Carcassone.Core.Players;

namespace Carcassone.Core.Calculation.Objects
{
    public interface IClosingObject
    {
        public bool IsFinished { get; }
        public void TryToClose(GamePlayersPool playersPool, Stack cardPool);
    }
}
