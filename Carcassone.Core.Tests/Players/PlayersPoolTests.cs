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

            var player = new Player() { Name = "bob", PlayerType = PlayerType.Human };
            pool.AddPlayer(player);
            var bob = pool.GetPlayer(player.Name);
            Assert.Equal("bob", bob.Name);

            var player1 = new Player() { Name = "AI_1", PlayerType = PlayerType.AI_Easy };
            pool.AddPlayer(player1);
            var ai_1 = pool.GetPlayer(player1.Name);
            Assert.Equal(PlayerType.AI_Easy, ai_1.PlayerType);

            pool.MoveToNextPlayer();
            Assert.True("bob" == pool.GetCurrentPlayer().Name, "Неверно установлен первый игрок");

            pool.MoveToNextPlayer();
            Assert.True(pool.GetCurrentPlayer() == pool.GamePlayers[1], "Игроки не переключаются");
        }
    }
}
