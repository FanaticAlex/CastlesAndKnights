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
            Assert.True("bob" == pool.GetPlayer("bob")?.Name, "Игрок не добавляется");
            
            pool.AddPlayer("AI_1", PlayerType.AI_Easy);
            Assert.Single(pool.Players.Where(p => p.PlayerType == PlayerType.AI_Easy), "Игрок AI не добавляется");

            pool.MoveToNextPlayer();
            Assert.True("bob" == pool.GetCurrentPlayer().Name, "Неверно установлен первый игрок");

            pool.MoveToNextPlayer();
            Assert.True(pool.GetCurrentPlayer() == pool.Players[1], "Игроки не переключаются");
        }
    }
}
