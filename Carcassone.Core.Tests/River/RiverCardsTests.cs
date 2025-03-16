using Carcassone.Core.Players;
using Xunit;

namespace Carcassone.Core.Tests.Buisness
{
    public class RiverCardsTests
    {
        /// <summary>
        ///       F
        ///   |       |
        /// F |   +   | F
        ///   |   +   |
        ///       W
        ///       
        ///       F
        ///   |       |
        /// W |+++O+++| W   (поворот на 1)
        ///   |   |   |
        ///       R
        /// </summary>

        [Fact]
        public void RiverCardPut()
        {
            var room = new GameRoom();
            room.PlayersPool.AddPlayer("bob", PlayerType.Human);
            room.Start();

            var gameMove1 = new GameMove()
            {
                PlayerName = "bob",
                CardId = "FWRW(0)",
                CardRotation = 1,
                FieldId = $"{0}_{-1}",
                PartName = "Cornfield_0"
            };
            room.MakeMove(gameMove1);

            Assert.Equal(1, room.CardsPool.GetCard("FWRW(0)").RotationsCount);
        }
    }
}
