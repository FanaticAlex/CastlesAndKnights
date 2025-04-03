using Carcassone.Core.Cards;
using Carcassone.Core.Extensions;
using Carcassone.Core.Fields;
using Carcassone.Core.Players;
using Xunit;

namespace Carcassone.Core.Tests.Room
{
    public class GameRoomTest
    {
        [Fact]
        public void WorkflowAI()
        {
            var room = new GameRoom();
            room.PlayersPool.AddPlayer("AI_1", PlayerType.AI_Easy);
            room.PlayersPool.AddPlayer("AI_2", PlayerType.AI_Easy);
            room.Start();

            do
            {
                var player = room.PlayersPool.GetCurrentPlayer();
                player.ProcessMove(room);
            }
            while (!room.IsFinished);

            Assert.True(room.IsFinished);
            Assert.NotEqual(0, room.GetPlayerScore(room.PlayersPool.GamePlayers[0]).GetOverallScore());
            Assert.NotEqual(0, room.GetPlayerScore(room.PlayersPool.GamePlayers[1]).GetOverallScore());
        }

        [Fact]
        public void GetCurrentCardTest()
        {
            var room = new GameRoom();
            room.PlayersPool.AddPlayer("bob", PlayerType.Human);
            room.Start();
            var card = room.CardsPool.CurrentCard;
            Assert.NotNull(card);
        }

        [Fact]
        public void GetCardsRemainInPoolTest()
        {
            var room = new GameRoom();
            Assert.Equal(82, room.CardsPool.CardsDeck.Count);
        }

        [Fact]
        public void SaveLoadTest()
        {
            var room = new GameRoom();
            Assert.Single(room.FieldBoard.Fields);
            room.FieldBoard.Fields[0].CardName = "CCCC";
            var save = room.Save();
            room.Load(save);
            Assert.Single(room.FieldBoard.Fields);
            Assert.Equal("CCCC", room.FieldBoard.Fields[0].CardName);
        }
    }
}