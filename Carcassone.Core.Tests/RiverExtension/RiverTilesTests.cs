using Carcassone.Core.Players;
using System;
using Xunit;

namespace Carcassone.Core.Tests.RiverExtension
{
    public class RiverTilesTests
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
        /// W |+++O+++| W 
        ///   |   |   |
        ///       R
        /// </summary>

        [Fact]
        public void RiverTileCantConnect()
        {
            var room = new GameRoom();
            var name = "bob";
            room.PlayersPool.AddPlayer(name, PlayerType.Human);
            room.Start();

            var gameMove1 = new GameMove()
            {
                PlayerName = name,
                CardId = "FWRW(0)",
                CardRotation = 0,
                FieldId = $"{0}_{-1}",
                PartName = "Farm_0"
            };
            Assert.Throws<Exception>(() => room.MakeMove(gameMove1));
        }


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
        public void RiverTileConnect()
        {
            var room = new GameRoom();
            var name = "bob";
            room.PlayersPool.AddPlayer(name, PlayerType.Human);
            room.Start();

            var gameMove1 = new GameMove()
            {
                PlayerName = name,
                CardId = "FWRW(0)",
                CardRotation = 1,
                FieldId = $"{0}_{-1}",
                PartName = "Farm_0"
            };
            room.MakeMove(gameMove1);

            Assert.Equal(1, room.TileStack.GetCard("FWRW(0)").RotationsCount);
        }
    }
}
