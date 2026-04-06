using Carcassone.Core.Calculation.Base.Tiles;
using Carcassone.Core.Calculation.RiverExtension.Tiles;
using Carcassone.Core.Players;
using Carcassone.Core.Tiles;
using System;
using System.Drawing;
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
            room.ForTests_ResetTilesStack();
            room.ForTests_CustomTile(typeof(FFWF), 1, 0);
            room.ForTests_CustomTile(typeof(FWRW), 1, 0);

            var gameMove0 = new GameMove
            (
                playerName: TestHelper.Bob,
                tileId: "FFWF(0)",
                tileRotation: 0,
                tileLocation: new Point(0, 0),
                partName: "Farm_0"
            );
            room.MakeMove(gameMove0);

            var gameMove1 = new GameMove
            (
                playerName: TestHelper.Bob,
                tileId: "FWRW(0)",
                tileRotation: 0,
                tileLocation: new Point(0, -1),
                partName: "Farm_0"
            );

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
            room.ForTests_ResetTilesStack();
            room.ForTests_CustomTile(typeof(FFWF), 1, 0);
            room.ForTests_CustomTile(typeof(FWRW), 1, 0);

            var gameMove0 = new GameMove
            (
                playerName: TestHelper.Bob,
                tileId: "FFWF(0)",
                tileRotation: 0,
                tileLocation: new Point(0, 0),
                partName: "Farm_0"
            );
            room.MakeMove(gameMove0);

            var gameMove1 = new GameMove
            (
                playerName: TestHelper.Bob,
                tileId: "FWRW(0)",
                tileRotation: 1,
                tileLocation: new Point(0, -1),
                partName: null
            );
            room.MakeMove(gameMove1);
        }

        [Fact]
        public void TileStackCompositionTest()
        {
            // 1. the first river card is always FFWF
            // 2. all river cards are at the begining of the pile
            // 3. the last river card is WFFF
            // 4. there is no river cards after WFFF

            var room = TestHelper.GetRiverGame(TestHelper.Bob);

            Assert.Equal("FFWF(0)", room.GetCurrentTile()?.Id);

            Tile? tile;
            while(true)
            {
                tile = room.GetCurrentTile();
                if (tile == null)
                    throw new Exception("Not enough water tiles!");

                if (!tile.Id.Contains("W"))
                    break; // water tiles is no more

                var moves = room.GetAvailableMoves(tile.Id);
                room.MakeMove(moves.First());
            };

            Assert.Equal("WFFF(0)", tile.Id);

            while(true)
            {
                tile = room.GetCurrentTile();
                if (tile == null)
                    break;

                if (tile.Id.Contains("W"))
                    throw new Exception("Missplaced river card " + tile.Id);

                var moves = room.GetAvailableMoves(tile.Id);
                room.MakeMove(moves.First());
            };
        }
    }
}
