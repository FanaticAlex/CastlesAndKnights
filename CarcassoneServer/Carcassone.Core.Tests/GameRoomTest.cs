using Xunit;

namespace Carcassone.Core.Tests.Room
{
    public class GameRoomTest
    {
        [Fact]
        public void WorkflowAI()
        {
            var room = new GameRoom();
            room.AddAIPlayer();
            room.AddAIPlayer();
            room.Start();

            while (!room.IsFinished)
            {
            }

            Assert.NotEqual(0, room.GetPlayerScore(room.GetPlayers()[0]).GetOverallScore());
            Assert.NotEqual(0, room.GetPlayerScore(room.GetPlayers()[1]).GetOverallScore());
        }
    }
}