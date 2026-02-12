using Carcassone.Core.Tiles;
using Carcassone.Core.Players;
using Xunit;
using System.Linq;
using System.Drawing;

namespace Carcassone.Core.Tests.Buisness
{
    public class MonasteryTests
    {
        /// <summary>
        /// 9 полей с картой церкви в центальной фишка игрока.
        /// 
        ///       F               F               F
        ///   |       |       |       |       |       |
        /// F |    ___| R   R |_______| R   R |___    | F
        ///   |   |   |       |       |       |   |   |
        ///       R               F               R
        /// 
        ///       R               F               R
        ///   |   |   |       |       |       |   |   |
        /// F |   |   | F   F |   O   | F   F |   |   | F
        ///   |   |   |       |       |       |   |   |
        ///       R               F               R
        /// 
        ///       R               F               R
        ///   |   |   |       |       |       |   |   |
        /// F |   \___| R   R |_______| R   R |__/    | F
        ///   |       |       |       |       |       |
        ///       F               F               F
        /// 
        /// </summary>
        [Fact]
        public void GetPointsTest()
        {
            var room = TestHelper.GetDefaultGame(TestHelper.Bob);

            var gameMove0 = new GameMove()
            {
                PlayerName = TestHelper.Bob,
                TileId = "FFFF(0)",
                TileRotation = 0,
                Location = new Point(0, 0),
                PartName = "Church_0"
            };
            room.MakeMove(gameMove0);

            Assert.Single(room.GetAllMonastery());
            Assert.Equal(1, room.GetPlayerScore(TestHelper.Bob).OverallScore);

            var gameMove1 = new GameMove()
            {
                PlayerName = TestHelper.Bob,
                TileId = "RFRF(0)",
                TileRotation = 1,
                Location = new Point(0, 1),
                PartName = null
            };
            room.MakeMove(gameMove1);
            Assert.Single(room.GetAllMonastery());
            Assert.Equal(2, room.GetPlayerScore(TestHelper.Bob).OverallScore);

            var gameMove2 = new GameMove()
            {
                PlayerName = TestHelper.Bob,
                TileId = "FFRR(0)",
                TileRotation = 0,
                Location = new Point(1, 1),
                PartName = null
            };
            room.MakeMove(gameMove2);
            Assert.Single(room.GetAllMonastery());
            Assert.Equal(3, room.GetPlayerScore(TestHelper.Bob).OverallScore);

            var gameMove3 = new GameMove()
            {
                PlayerName = TestHelper.Bob,
                TileId = "FFRR(1)",
                TileRotation = 3,
                Location = new Point(-1, 1),
                PartName = null
            };
            room.MakeMove(gameMove3);
            Assert.Single(room.GetAllMonastery());
            Assert.Equal(4, room.GetPlayerScore(TestHelper.Bob).OverallScore);

            var gameMove4 = new GameMove()
            {
                PlayerName = TestHelper.Bob,
                TileId = "RFRF(1)",
                TileRotation = 0,
                Location = new Point(-1, 0),
                PartName = null
            };
            room.MakeMove(gameMove4);
            Assert.Single(room.GetAllMonastery());
            Assert.Equal(5, room.GetPlayerScore(TestHelper.Bob).OverallScore);

            var gameMove5 = new GameMove()
            {
                PlayerName = TestHelper.Bob,
                TileId = "RFRF(2)",
                TileRotation = 0,
                Location = new Point(1, 0),
                PartName = null
            };
            room.MakeMove(gameMove5);
            Assert.Single(room.GetAllMonastery());
            Assert.Equal(6, room.GetPlayerScore(TestHelper.Bob).OverallScore);

            var gameMove6 = new GameMove()
            {
                PlayerName = TestHelper.Bob,
                TileId = "RFRF(3)",
                TileRotation = 1,
                Location = new Point(0, -1),
                PartName = null
            };
            room.MakeMove(gameMove6);
            Assert.Single(room.GetAllMonastery());
            Assert.Equal(7, room.GetPlayerScore(TestHelper.Bob).OverallScore);

            var gameMove7 = new GameMove()
            {
                PlayerName = TestHelper.Bob,
                TileId = "FFRR(2)",
                TileRotation = 1,
                Location = new Point(1, -1),
                PartName = null
            };
            room.MakeMove(gameMove7);
            Assert.Single(room.GetAllMonastery());
            Assert.Equal(8, room.GetPlayerScore(TestHelper.Bob).OverallScore);

            var gameMove8 = new GameMove()
            {
                PlayerName = TestHelper.Bob,
                TileId = "FFRR(3)",
                TileRotation = 2,
                Location = new Point(-1, -1),
                PartName = null
            };
            room.MakeMove(gameMove8);
            Assert.Single(room.GetAllMonastery());
            Assert.Equal(18, room.GetPlayerScore(TestHelper.Bob).OverallScore);

            Assert.True(room.GetAllMonastery().ElementAt(0).IsComplete());
        }
    }
}
