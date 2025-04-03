using Carcassone.Core.Players;
using System.Linq;
using Xunit;

namespace Carcassone.Core.Tests.Players
{
    public class PlayersPoolTests
    {
        [Fact]
        public void Workflow()
        {
            var pool = new GamePlayersPool();

            var bobName = "bob";
            pool.AddPlayer(bobName, PlayerType.Human);
            var bob = pool.GetPlayer(bobName);
            Assert.Equal(bobName, bob.Name);

            var player1Name = "AI_1";
            pool.AddPlayer(player1Name, PlayerType.AI_Easy);
            var ai_1 = pool.GetPlayer(player1Name);
            Assert.Equal(PlayerType.AI_Easy, ai_1.PlayerType);

            pool.MoveToNextPlayer();
            Assert.True("bob" == pool.GetCurrentPlayer().Name, "Неверно установлен первый игрок");

            pool.MoveToNextPlayer();
            Assert.True(pool.GetCurrentPlayer() == pool.GamePlayers[1], "Игроки не переключаются");
        }
    }
}
