using Carcassone.Core.Players;
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
            Assert.True(PlayersPool.EasyBotName == pool.GetPlayer(PlayersPool.EasyBotName)?.Name, "Игрок AI не добавляется");
            
            Assert.True("bob" == pool.CurrentPlayer.Name, "Неверно установлен первый игрок");

            pool.MoveNextPlayer();
            Assert.True(PlayersPool.EasyBotName == pool.CurrentPlayer.Name, "Игроки не переключаются");
        }
    }
}
