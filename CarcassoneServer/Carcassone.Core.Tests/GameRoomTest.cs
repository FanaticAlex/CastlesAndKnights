using Carcassone.Core.Cards;
using Carcassone.Core.Extensions;
using Carcassone.Core.Fields;
using Xunit;

namespace Carcassone.Core.Tests.Room
{
    public class GameRoomTest
    {
        [Fact]
        public void WorkflowAI()
        {
            var room = new GameRoom();
            room.PlayersPool.AddAIPlayerEasy();
            room.PlayersPool.AddAIPlayerEasy();
            room.Start();

            Assert.True(room.IsFinished);
            Assert.NotEqual(0, room.GetPlayerScore(room.PlayersPool.Players[0]).GetOverallScore());
            Assert.NotEqual(0, room.GetPlayerScore(room.PlayersPool.Players[1]).GetOverallScore());
        }

        [Fact]
        public void GetCurrentCardTest()
        {
            var room = new GameRoom();
            var card = room.GetCurrentCard();
            Assert.NotNull(card);
        }

        [Fact]
        public void GetCardsRemainInPoolTest()
        {
            var room = new GameRoom();
            var count = room.GetCardsRemain();
            Assert.Equal(82, count);
        }

        [Fact]
        public void SaveLoadTest()
        {
            var room = new GameRoom();
            Assert.Single(room.FieldBoard.Fields);
            var save = room.Save();
            room.Load(save);
            Assert.Single(room.FieldBoard.Fields);
            Assert.Equal("CCCC", room.FieldBoard.Fields[0].CardName);
        }
    }
}