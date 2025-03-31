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
            var player1 = new Player() { Name = "AI_1", PlayerType = PlayerType.AI_Easy };
            var player2 = new Player() { Name = "AI_2", PlayerType = PlayerType.AI_Easy };
            room.PlayersPool.AddPlayer(player1);
            room.PlayersPool.AddPlayer(player2);
            room.Start();

            Assert.True(room.IsFinished);
            Assert.NotEqual(0, room.GetPlayerScore(room.PlayersPool.GamePlayers[0]).GetOverallScore());
            Assert.NotEqual(0, room.GetPlayerScore(room.PlayersPool.GamePlayers[1]).GetOverallScore());
        }

        [Fact]
        public void GetCurrentCardTest()
        {
            var room = new GameRoom();
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