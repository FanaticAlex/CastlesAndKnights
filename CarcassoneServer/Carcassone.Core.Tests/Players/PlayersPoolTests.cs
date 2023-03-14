using Carcassone.Core.Players;
using Carcassone.Core.Players.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            pool.MoveNextPlayer(null);
            Assert.True("bob" == pool.CurrentPlayer.Name, "Неверно установлен первый игрок");

            pool.MoveNextPlayer(null);
            Assert.True(pool.CurrentPlayer == pool.Players[1], "Игроки не переключаются");
        }
    }
}
