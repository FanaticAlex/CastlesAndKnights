using Carcassone.Core.Players;
using System;
using System.Linq;
using Xunit;

namespace Carcassone.Core.Tests.Players
{
    public class PlayersPoolTests
    {
        [Fact]
        public void Workflow()
        {
            var room = TestHelper.GetDefaultGame(TestHelper.Bob, TestHelper.AI_1);

            Assert.Equal(room.PlayersPool.GetCurrentPlayer().Info.Name, TestHelper.Bob);
            var tile1Id = room.GetCurrentTile()?.Id;
            room.MakeMove(room.GetAvailableMoves(tile1Id).First());

            Assert.Equal(room.PlayersPool.GetCurrentPlayer().Info.Name, TestHelper.AI_1);
            var tile2Id = room.GetCurrentTile()?.Id;
            room.MakeMove(room.GetAvailableMoves(tile2Id).First());

            Assert.Equal(room.PlayersPool.GetCurrentPlayer().Info.Name, TestHelper.Bob);
        }
    }
}
