using Carcassone.Core.Tiles;
using Carcassone.Core.Players;

namespace Carcassone.Core.Calculation
{
    public interface IClosingObject
    {
        public bool IsFinished { get; }
        public void TryToClose(GamePlayersPool playersPool, TileStack cardPool);
    }
}
