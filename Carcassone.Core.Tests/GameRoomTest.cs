using Carcassone.Core.Board;
using Carcassone.Core.Calculation.RiverExtension.Tiles;
using Carcassone.Core.Players;
using Carcassone.Core.Tiles;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using Xunit;

namespace Carcassone.Core.Tests.Room
{
    public class GameRoomTest
    {
        [Fact]
        public void WorkflowAI()
        {
            var room = TestHelper.GetDefaultGame(TestHelper.AI_1, TestHelper.AI_2);

            do
            {
                var player = room.GetCurrentPlayer();
                player.ProcessMove(room);
            }
            while (!room.IsFinished);

            Assert.True(room.IsFinished);
            var scores = room.GetPlayersScores();
            Assert.Equal(2, scores.Count());
            Assert.NotEqual(0, scores.ElementAt(0).OverallScore);
            Assert.NotEqual(0, scores.ElementAt(1).OverallScore);
        }

        [Fact]
        public void GetCurrentCardTest()
        {
            var room = TestHelper.GetDefaultGame(TestHelper.Bob);
            var card = room.GetCurrentTile();
            Assert.NotNull(card);
        }

        [Fact]
        public void GetCardsRemainInPoolTest()
        {
            var room = TestHelper.GetDefaultGame(TestHelper.Bob);
            Assert.Equal(82, room.GetRemainTilesCount());
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
        /// R |---O   | F
        ///   |   +   |
        ///       W
        /// </summary>
        [Fact]
        public void SaveLoadTest()
        {
            var room = TestHelper.GetDefaultGame(TestHelper.Bob);

            var gameMove0 = new GameMove()
            {
                PlayerName = TestHelper.Bob,
                TileId = "FFWF(0)",
                TileRotation = 0,
                Location = new Point(0, 0),
                PartName = "Farm_0"
            };
            room.MakeMove(gameMove0);

            var gameMove1 = new GameMove()
            {
                PlayerName = TestHelper.Bob,
                TileId = "FWRW(0)",
                TileRotation = 1,
                Location = new Point(0, -1),
                PartName = "Monastery_0"
            };
            room.MakeMove(gameMove1);
            Assert.Equal(2, room.GetPlayerScore(TestHelper.Bob).OverallScore);

            var copy = room.Copy();
            Assert.Equal(2, copy.GetPlayerScore(TestHelper.Bob).OverallScore);
        }
    }
}