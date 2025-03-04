using Carcassone.Core.Players;
using Carcassone.Core.Players.AI;
using System.Linq;
using Xunit;

namespace Carcassone.Core.Tests.Players
{
    public class PlayersPoolTests
    {
        [Fact]
        public void Workflow()
        {
            var pool = new PlayersPool();
            
            pool.AddPlayer("bob", PlayerType.Human);
            var bob = pool.GetPlayer("bob");
            Assert.Equal("bob", bob.Name);
            
            pool.AddPlayer("AI_1", PlayerType.AI_Easy);
            var ai_1 = pool.GetPlayer("AI_1");
            Assert.Equal(PlayerType.AI_Easy, ai_1.PlayerType);

            pool.MoveToNextPlayer();
            Assert.True("bob" == pool.GetCurrentPlayer().Name, "Неверно установлен первый игрок");

            pool.MoveToNextPlayer();
            Assert.True(pool.GetCurrentPlayer() == pool.Players[1], "Игроки не переключаются");
        }
    }
}
