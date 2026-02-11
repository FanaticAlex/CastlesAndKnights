using Carcassone.Core.Players;
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
            
            Assert.Equal(room.GetCurrentPlayer().Info.Name, TestHelper.Bob);
            room.MakeMove(room.GetAvailableMoves().First());

            Assert.Equal(room.GetCurrentPlayer().Info.Name, TestHelper.AI_1);
            room.MakeMove(room.GetAvailableMoves().First());

            Assert.Equal(room.GetCurrentPlayer().Info.Name, TestHelper.Bob);
        }
    }
}
