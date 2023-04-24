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
            
            pool.AddHumanPlayer("bob");
            Assert.True("bob" == pool.GetPlayer("bob")?.Name, "Игрок не добавляется");
            
            pool.AddAIPlayerEasy();
            Assert.True(pool.Players.OfType<PlayerAI>().Any(), "Игрок AI не добавляется");

            pool.MoveToNextPlayer();
            Assert.True("bob" == pool.GetCurrentPlayer().Name, "Неверно установлен первый игрок");

            pool.MoveToNextPlayer();
            Assert.True(pool.GetCurrentPlayer() == pool.Players[1], "Игроки не переключаются");
        }
    }
}
