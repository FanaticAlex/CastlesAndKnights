using Carcassone.Core.Players;
using Carcassone.Core.Tiles;
using System;
using System.Linq;
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

            var room = TestHelper.GetDefaultGame(TestHelper.Bob);

            var gameMove0 = new GameMove()
            {
                PlayerName = TestHelper.Bob,
                TileId = "FFWF(0)",
                TileRotation = 0,
                CellId = $"{0}_{0}",
                PartName = "Farm_0"
            };
            room.MakeMove(gameMove0);

            var gameMove1 = new GameMove()
            {
                PlayerName = TestHelper.Bob,
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
        ///       W
        ///   |   +   |
        /// R |---O   | F   (поворот на 1)
        ///   |   +   |
        ///       W
        /// </summary>

        [Fact]
        public void RiverTileConnect()
        {
            // 1. river card connects correctly to another river card

            var room = TestHelper.GetDefaultGame(TestHelper.Bob);
            
            var gameMove0 = new GameMove()
            {
                PlayerName = TestHelper.Bob,
                TileId = "FFWF(0)",
                TileRotation = 0,
                CellId = $"{0}_{0}",
                PartName = "Farm_0"
            };
            room.MakeMove(gameMove0);

            var gameMove1 = new GameMove()
            {
                PlayerName = TestHelper.Bob,
                TileId = "FWRW(0)",
                TileRotation = 1,
                CellId = $"{0}_{-1}",
                PartName = "Farm_0"
            };
            room.MakeMove(gameMove1);
        }

        [Fact]
        public void TileStackCompositionTest()
        {
            // 1. the first river card is always FFWF
            // 2. all river cards are at the begining of the pile
            // 3. the last river card is WFFF
            // 4. there is no river cards after WFFF

            var room = TestHelper.GetDefaultGame(TestHelper.Bob);

            Assert.Equal("FFWF(0)", room.GetCurrentTile().Id);

            Tile tile = null;
            do
            {
                tile = room.GetCurrentTile();
                var moves = room.GetAvailableMoves();
                room.MakeMove(moves.First());
            }
            while (room.GetCurrentTile().Id.Contains("W"));

            Assert.Equal("WFFF(0)", tile.Id);

            do
            {
                tile = room.GetCurrentTile();
                if (tile.Id.Contains("W"))
                    throw new Exception("Missplaced river card " + tile.Id);

                var moves = room.GetAvailableMoves();
                room.MakeMove(moves.First());
            }
            while (room.GetCurrentTile() != null) ;
        }
    }
}
