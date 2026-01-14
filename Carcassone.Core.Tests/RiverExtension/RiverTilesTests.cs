using Carcassone.Core.Players;
using Carcassone.Core.Tiles;
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
            // 1. river card do not connects if rotation is wrong

            var room = new GameRoom();
            var name = "bob";
            room.PlayersPool.AddPlayer(name, PlayerType.Human);
            room.Start();

            var gameMove1 = new GameMove()
            {
                PlayerName = name,
                TileId = "FWRW(0)",
                TileRotation = 0,
                CellId = $"{0}_{-1}",
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
            // 1. river card connects correctly to another river card

            var room = new GameRoom();
            var name = "bob";
            room.PlayersPool.AddPlayer(name, PlayerType.Human);
            room.Start();

            var gameMove1 = new GameMove()
            {
                PlayerName = name,
                TileId = "FWRW(0)",
                TileRotation = 1,
                CellId = $"{0}_{-1}",
                PartName = "Farm_0"
            };
            room.MakeMove(gameMove1);

            Assert.Equal(1, room.TileStack.GetTile("FWRW(0)").RotationsCount);
        }

        [Fact]
        public void TileStackCompositionTest()
        {
            // 1. the first river card is always FFWF
            // 2. all river cards are at the begining of the pile
            // 3. the last river card is WFFF
            // 4. there is no river cards after WFFF

            var room = new GameRoom();
            Assert.Equal("FFWF(0)", room.TileStack.GetTopTile().Id);

            Tile tile = null;
            do
            {
                tile = room.TileStack.GetTopTile();
                room.TileStack.DiscardTile(tile);
            }
            while (room.TileStack.GetTopTile().Id.Contains("W"));

            Assert.Equal("WFFF(0)", tile.Id);

            do
            {
                tile = room.TileStack.GetTopTile();
                if (tile.Id.Contains("W"))
                    throw new Exception("Missplaced river card " + tile.Id);

                room.TileStack.DiscardTile(tile);
            }
            while (room.TileStack.GetTopTile() != null) ;
        }
    }
}
