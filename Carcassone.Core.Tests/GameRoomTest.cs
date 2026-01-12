using Carcassone.Core.Board;
using Carcassone.Core.Extensions;
using Carcassone.Core.Players;
using Carcassone.Core.Tiles;
using System.Xml.Linq;
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
            room.PlayersPool.AddPlayer("player1", PlayerType.Human);

            Assert.Single(room.GameGrid.Cells);

            var gameMove0 = new GameMove()
            {
                PlayerName = "player1",
                CardId = "FFWF(0)",
                CardRotation = 0,
                FieldId = $"{0}_{0}",
                PartName = null
            };
            room.MakeMove(gameMove0);

            var save = room.Save();
            room.Load(save);

            Assert.Equal(5, room.GameGrid.Cells.Count);
        }
    }
}