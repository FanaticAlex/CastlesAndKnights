using Carcassone.Core.Cards;
using Carcassone.Core.Players;

namespace Carcassone.Core.Calculation.Objects
{
    public interface IClosingObject
    {
        public bool IsFinished { get; }
        public void TryToClose(PlayersPool playersPool, CardPool cardPool);
    }
}
