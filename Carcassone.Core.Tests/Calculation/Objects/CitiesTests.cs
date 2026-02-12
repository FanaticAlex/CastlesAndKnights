using Carcassone.Core.Tiles;
using Carcassone.Core.Players;
using System.Linq;
using Xunit;
using System.Drawing;

namespace Carcassone.Core.Tests.Buisness
{
    public class CitiesTests
    {

        /// <summary>
        ///       F
        ///   |       |
        /// F |   +   | F
        ///   |   +   |
        ///       W
        /// 
        ///       W
        ///   |\  +  /|
        /// C | | + | | C
        ///   |/  +  \|
        ///       W
        /// </summary>
        [Fact]
        public void CalculationTest_NotClosedCity()
        {
            var room = TestHelper.GetDefaultGame(TestHelper.Bob);

            var gameMove0 = new GameMove()
            {
                PlayerName = TestHelper.Bob,
                TileId = "FFWF(0)",
                TileRotation = 0,
                Location = new Point(0, 0),
                PartName = null
            };
            room.MakeMove(gameMove0);

            var gameMove1 = new GameMove()
            {
                PlayerName = TestHelper.Bob,
                TileId = "CWCW(0)",
                TileRotation = 1,
                Location = new Point(0, -1),
                PartName = "City_0"
            };
            room.MakeMove(gameMove1);

            var Citys = room.GetAllCities();
            Assert.True(Citys.Count() == 2);
            Assert.False(Citys.ElementAt(0).IsComplete());
            Assert.False(Citys.ElementAt(1).IsComplete());
            
            var score = room.GetPlayerScore(TestHelper.Bob);
            Assert.True(Citys.ElementAt(0).IsPlayerOwner(TestHelper.Bob));
            Assert.Equal(1, score.OverallScore);
            Assert.Equal(6, score.ChipCount);
        }

        /// <summary>
        /// 
        ///       F
        ///   |       |
        /// F | _____ | F
        ///   |/     \|
        ///       C
        /// 
        ///       C
        ///   |\_____/|
        /// F |       | F
        ///   |       |
        ///       F
        /// </summary>
        [Fact]
        public void CalculationTest_DoubleCity()
        {
            var room = TestHelper.GetDefaultGame(TestHelper.Bob);

            var gameMove0 = new GameMove()
            {
                PlayerName = TestHelper.Bob,
                TileId = "CFFF(0)",
                TileRotation = 2,
                Location = new Point(0,0),
                PartName = null
            };
            room.MakeMove(gameMove0);

            var gameMove1 = new GameMove()
            {
                PlayerName = TestHelper.Bob,
                TileId = "CFFF(1)",
                TileRotation = 0,
                Location = new Point(0, -1),
                PartName = "City_0"
            };
            room.MakeMove(gameMove1);

            var Citys = room.GetAllCities();
            Assert.Single(Citys);
            var City = Citys.Single();
            Assert.True(City.IsComplete());
            Assert.Equal(4, City.GetScore());
            Assert.True(City.IsPlayerOwner(TestHelper.Bob));
            Assert.Contains(City.Parts, p => p.Flag != null);

            var score = room.GetPlayerScore(TestHelper.Bob);
            Assert.Equal(4, score.OverallScore);
            Assert.Equal(7, score.ChipCount);
        }


        /// <summary>
        /// 
        ///       F
        ///   |       |
        /// F | _____ | F
        ///   |/     \|
        ///       C
        ///       
        ///       C               F
        ///   |\-----/|       |       |   
        /// W |+++++++| W   W |++++   | F
        ///   |/-----\|       |       |
        ///       C               F 
        /// 
        ///       C
        ///   |\_____/|
        /// F |       | F
        ///   |       |
        ///       F
        /// </summary>
        [Fact]
        public void CalculationTest_TwoRiverCities()
        {
            var room = TestHelper.GetDefaultGame(TestHelper.Bob);

            var gameMove0 = new GameMove()
            {
                PlayerName = TestHelper.Bob,
                TileId = "FFWF(0)",
                TileRotation = 1,
                Location = new Point(0, 0),
                PartName = null
            };
            room.MakeMove(gameMove0);

            var gameMove1 = new GameMove()
            {
                PlayerName = TestHelper.Bob,
                TileId = "CWCW(0)",
                TileRotation = 0,
                Location = new Point(-1, 0),
                PartName = null
            };
            room.MakeMove(gameMove1);

            var gameMove2 = new GameMove()
            {
                PlayerName = TestHelper.Bob,
                TileId = "CFFF(0)",
                TileRotation = 2,
                Location = new Point(-1, 1),
                PartName = null
            };
            room.MakeMove(gameMove2);

            var gameMove3 = new GameMove()
            {
                PlayerName = TestHelper.Bob,
                TileId = "CFFF(1)",
                TileRotation = 0,
                Location = new Point(-1, -1),
                PartName = "City_0"
            };
            room.MakeMove(gameMove3);

            var Citys = room.GetAllCities();
            Assert.Equal(2, Citys.Count());

            var City = Citys.ElementAt(1);
            Assert.Equal(4, City.GetScore());
            Assert.True(City.IsComplete());
            Assert.True(City.IsPlayerOwner(TestHelper.Bob));
            Assert.Contains(City.Parts, p => p.Flag != null);

            var score = room.GetPlayerScore(TestHelper.Bob);
            Assert.Equal(4, score.OverallScore);
            Assert.Equal(7, score.ChipCount);
        }
    }
}
