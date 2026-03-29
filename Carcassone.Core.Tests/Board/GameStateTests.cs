using Carcassone.Core.Calculation.Base.Tiles;
using Carcassone.Core.Tiles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Xunit;

namespace Carcassone.Core.Tests.Board
{
    public class GameStateTests
    {
        [Fact]
        public void GetCenterTest()
        {
            var room = TestHelper.GetDefaultGame(TestHelper.Bob);
            room.ForTests_ResetTilesStack();
            room.ForTests_CustomTile(typeof(CCCC), 2, 0);

            var gameMove0 = new GameMove()
            {
                PlayerName = TestHelper.Bob,
                TileId = "CCCC(0)",
                TileRotation = 0,
                Location = new Point(0, 0),
                PartName = null
            };
            room.MakeMove(gameMove0);

            var move = room.State.GetMove(new Point(0, 0));
            Assert.Equal(new Point(0, 0), room.State.GetTileLocation(gameMove0.TileId));
            Assert.Equal(gameMove0.TileId, move?.TileId);
            Assert.Equal(4, room.State.GetEmptyCells().Count());
        }

        /// <summary>
        /// 
        ///                    F
        ///                |       |
        ///              F | _____ | F
        ///                |/     \|
        ///                    C
        /// 
        ///       C
        ///   |\_____/|
        /// F |       | F
        ///   |       |
        ///       F
        /// </summary>
        [Fact]
        public void MisplaceTile()
        {
            var room = TestHelper.GetDefaultGame(TestHelper.Bob);
            room.ForTests_ResetTilesStack();
            room.ForTests_CustomTile(typeof(CFFF), 2, 0);

            var gameMove0 = new GameMove()
            {
                PlayerName = TestHelper.Bob,
                TileId = "CFFF(0)",
                TileRotation = 2,
                Location = new Point(0, 0),
                PartName = null
            };
            room.MakeMove(gameMove0);

            var gameMove1 = new GameMove()
            {
                PlayerName = TestHelper.Bob,
                TileId = "CFFF(1)",
                TileRotation = 0,
                Location = new Point(1, 1),
                PartName = "City_0"
            };
            Assert.Throws<Exception>(() => room.MakeMove(gameMove1));
        }
    }
}
