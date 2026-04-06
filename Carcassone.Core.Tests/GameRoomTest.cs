using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using Xunit;

namespace Carcassone.Core.Tests.Room
{
    public class GameRoomTest
    {
        [Fact]
        public void WorkflowAI()
        {
            var room = TestHelper.GetDefaultGame(TestHelper.AI_1, TestHelper.AI_2);

            do
            {
                var player = room.PlayersPool.GetCurrentPlayer();
                player.ProcessMove(room);
            }
            while (!room.IsFinished());

            Assert.True(room.IsFinished());
            var ai1 = room.PlayersPool.GetPlayer(TestHelper.AI_1);
            var ai2 = room.PlayersPool.GetPlayer(TestHelper.AI_2);
            Assert.NotEqual(0, ai1?.GetPlayerOverallScore(room.State));
            Assert.NotEqual(0, ai2?.GetPlayerOverallScore(room.State));
        }

        [Fact]
        public void GetCurrentCardTest()
        {
            var room = TestHelper.GetDefaultGame(TestHelper.Bob);
            var card = room.GetCurrentTile();
            Assert.NotNull(card);
        }

        [Fact]
        public void GetCardsRemainInPoolTest()
        {
            var room = TestHelper.GetDefaultGame(TestHelper.Bob);
            Assert.Equal(70, room.GetRemainTilesCount());
        }

        [Fact]
        public void GetCardsRemainInPoolRiverTest()
        {
            var room = TestHelper.GetRiverGame(TestHelper.Bob);
            Assert.Equal(82, room.GetRemainTilesCount());
        }
    }
}