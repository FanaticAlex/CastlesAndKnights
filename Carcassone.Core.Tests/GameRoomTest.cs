using Carcassone.Core.Tiles;
using Carcassone.Core.Extensions;
using Carcassone.Core.Board;
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
            Assert.NotEqual(0, room.GetPlayerScore(room.PlayersPool.GamePlayers[0].Name).GetOverallScore());
            Assert.NotEqual(0, room.GetPlayerScore(room.PlayersPool.GamePlayers[1].Name).GetOverallScore());
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
            Assert.Equal(82, room.CardsPool.GetRemainTiles().Count);
        }

        [Fact]
        public void SaveLoadTest()
        {
            var room = new GameRoom();
            Assert.Single(room.GameGrid.Cells);
            room.GameGrid.Cells[0].CardName = "CCCC";
            var save = room.Save();
            room.Load(save);
            Assert.Single(room.GameGrid.Cells);
            Assert.Equal("CCCC", room.GameGrid.Cells[0].CardName);
        }
    }
}