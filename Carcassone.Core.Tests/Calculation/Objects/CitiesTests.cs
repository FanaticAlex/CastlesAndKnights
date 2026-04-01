using Carcassone.Core.Calculation.Base.Tiles;
using Carcassone.Core.Calculation.RiverExtension.Tiles;
using Carcassone.Core.Players;
using Carcassone.Core.Tiles;
using System;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using Xunit;

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
            room.ForTests_ResetTilesStack();
            room.ForTests_CustomTile(typeof(FFWF), 1, 0);
            room.ForTests_CustomTile(typeof(CWCW), 1, 0);

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

            var Cities = room.State.GetAllCities();
            Assert.True(Cities.Count() == 2);
            Assert.False(Cities.ElementAt(0).IsComplete(room.State));
            Assert.False(Cities.ElementAt(1).IsComplete(room.State));

            var bob = room.PlayersPool.GetPlayer(TestHelper.Bob);
            var scores = room.PlayersPool.GetPlayersScores(room.State);
            Assert.True(Cities.ElementAt(0).IsPlayerOwner(TestHelper.Bob, room.State));
            Assert.Equal(1, scores.ElementAt(0).OverallScore);
            Assert.Equal(6, scores.ElementAt(0).MeeplesCount);
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
            room.ForTests_ResetTilesStack();
            room.ForTests_CustomTile(typeof(CFFF), 2, 0);

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

            var Cities = room.State.GetAllCities();
            Assert.Single(Cities);
            var City = Cities.Single();
            Assert.True(City.IsComplete(room.State));
            Assert.Equal(4, City.GetScore(room.State));
            Assert.True(City.IsPlayerOwner(TestHelper.Bob, room.State));
            Assert.Contains(City.Parts, p => p.GetPartOwner(room.State) != null);

            var bob = room.PlayersPool.GetPlayer(TestHelper.Bob);
            var scores = room.PlayersPool.GetPlayersScores(room.State);
            Assert.Equal(4, scores.ElementAt(0).OverallScore);
            Assert.Equal(7, scores.ElementAt(0).MeeplesCount);
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
            room.ForTests_ResetTilesStack();
            room.ForTests_CustomTile(typeof(FFWF), 1, 0);
            room.ForTests_CustomTile(typeof(CWCW), 1, 0);
            room.ForTests_CustomTile(typeof(CFFF), 2, 0);

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

            var Cities = room.State.GetAllCities();
            Assert.Equal(2, Cities.Count());

            var City = Cities.ElementAt(1);
            Assert.Equal(4, City.GetScore(room.State));
            Assert.True(City.IsComplete(room.State));
            Assert.True(City.IsPlayerOwner(TestHelper.Bob, room.State));
            Assert.Contains(City.Parts, p => p.GetPartOwner(room.State) != null);

            var bob = room.PlayersPool.GetPlayer(TestHelper.Bob);
            var scores = room.PlayersPool.GetPlayersScores(room.State);
            Assert.Equal(4, scores.ElementAt(0).OverallScore);
            Assert.Equal(7, scores.ElementAt(0).MeeplesCount);
        }
    }
}
