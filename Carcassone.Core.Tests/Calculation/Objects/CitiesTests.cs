using Carcassone.Core.Calculation.Base.Tiles;
using Carcassone.Core.Calculation.RiverExtension.Tiles;
using Carcassone.Core.Players;
using Carcassone.Core.Tiles;
using System;
using System.Drawing;
using System.Linq;
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

            var Cities = room.GetLastState().GetAllCities();
            Assert.True(Cities.Count() == 2);
            Assert.False(Cities.ElementAt(0).IsComplete(room.State));
            Assert.False(Cities.ElementAt(1).IsComplete(room.State));
            
            var score = room.GetPlayerScore(TestHelper.Bob);
            Assert.True(Cities.ElementAt(0).IsPlayerOwner(TestHelper.Bob));
            Assert.Equal(1, score.OverallScore);
            Assert.Equal(6, score.MeeplesCount);
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

            var state = room.GetLastState();
            var Cities = state.GetAllCities();
            Assert.Single(Cities);
            var City = Cities.Single();
            Assert.True(City.IsComplete(room.State));
            Assert.Equal(4, City.GetScore(room.State));
            Assert.True(City.IsPlayerOwner(TestHelper.Bob));
            Assert.Contains(City.Parts, p => p.Meeple != null);

            var score = room.GetPlayerScore(TestHelper.Bob);
            Assert.Equal(4, score.OverallScore);
            Assert.Equal(7, score.MeeplesCount);
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

            var state = room.GetLastState();
            var Cities = state.GetAllCities();
            Assert.Equal(2, Cities.Count());

            var City = Cities.ElementAt(1);
            Assert.Equal(4, City.GetScore(room.State));
            Assert.True(City.IsComplete(room.State));
            Assert.True(City.IsPlayerOwner(TestHelper.Bob));
            Assert.Contains(City.Parts, p => p.Meeple != null);

            var score = room.GetPlayerScore(TestHelper.Bob);
            Assert.Equal(4, score.OverallScore);
            Assert.Equal(7, score.MeeplesCount);
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
        ///   |\     /|     
        /// F | |   | | F  
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
        public void CalculationTest_SeveralOwners()
        {
            var room = TestHelper.GetDefaultGame(TestHelper.Bob, TestHelper.Jack);
            room.ForTests_ResetTilesStack();
            room.ForTests_CustomTile(typeof(FCFC1), 1, 0);
            room.ForTests_CustomTile(typeof(CFFF), 2, 0);

            var gameMove0 = new GameMove()
            {
                PlayerName = TestHelper.Bob,
                TileId = "FCFC1(0)",
                TileRotation = 1,
                Location = new Point(0, 0),
                PartName = "City_0"
            };
            room.MakeMove(gameMove0);

            var gameMove2 = new GameMove()
            {
                PlayerName = TestHelper.Jack,
                TileId = "CFFF(0)",
                TileRotation = 2,
                Location = new Point(-1, 1),
                PartName = "City_0"
            };
            Assert.Throws<Exception>(() => room.MakeMove(gameMove2));
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
        ///   |\     /|     
        /// F | |   | | F  
        ///   |/     \|      
        ///       C           
        ///
        /// </summary>
        [Fact]
        public void CalculationTest_OwnedObject()
        {
            var room = TestHelper.GetDefaultGame(TestHelper.Bob, TestHelper.Jack);
            room.ForTests_ResetTilesStack();
            room.ForTests_CustomTile(typeof(FCFC1), 1, 0);
            room.ForTests_CustomTile(typeof(CFFF), 1, 0);

            var gameMove0 = new GameMove()
            {
                PlayerName = TestHelper.Bob,
                TileId = "FCFC1(0)",
                TileRotation = 1,
                Location = new Point(0, 0),
                PartName = "City_0"
            };
            room.MakeMove(gameMove0);

            var gameMove2 = new GameMove()
            {
                PlayerName = TestHelper.Jack,
                TileId = "CFFF(0)",
                TileRotation = 2,
                Location = new Point(0, 1),
                PartName = "City_0"
            };
            Assert.Throws<Exception>(() => room.MakeMove(gameMove2));
        }
    }
}
