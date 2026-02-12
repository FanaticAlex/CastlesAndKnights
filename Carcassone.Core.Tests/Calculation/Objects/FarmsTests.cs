using Carcassone.Core.Calculation.Base.Cities;
using Carcassone.Core.Players;
using System.Drawing;
using System.Linq;
using Xunit;

namespace Carcassone.Core.Tests.Buisness
{
    public class FarmsTests
    {
        /// <summary>
        ///       F
        ///   |_______|
        /// R | _____ | R
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
        public void GetScore()
        {
            var room = TestHelper.GetDefaultGame(TestHelper.Bob);

            var gameMove1 = new GameMove()
            {
                PlayerName = TestHelper.Bob,
                TileId = "CRFR(0)",
                TileRotation = 2,
                Location = new Point(0, 0),
                PartName = "Farm_0"
            };
            room.MakeMove(gameMove1);

            var gameMove2 = new GameMove()
            {
                PlayerName = TestHelper.Bob,
                TileId = "CFFF(0)",
                TileRotation = 0,
                Location = new Point(0, -1),
                PartName = "City_0"
            };
            room.MakeMove(gameMove2);

            // check castles
            Assert.Single(room.GetAllCities());
            var city = room.GetAllCities().Single();
            Assert.True(city.IsComplete());
            Assert.Equal(4, city.GetScore());
            Assert.True(city.IsPlayerOwner(TestHelper.Bob));
            
            // check farms
            Assert.Equal(3, room.GetAllFarms().Count());
            var farm0 = room.GetAllFarms().ElementAt(0);
            Assert.True(farm0.IsPlayerOwner(TestHelper.Bob));
            Assert.Equal(3, farm0.GetScore());

            var farm1 = room.GetAllFarms().ElementAt(1);
            Assert.False(farm1.IsPlayerOwner(TestHelper.Bob));
            Assert.Equal(0, farm1.GetScore());

            var farm2 = room.GetAllFarms().ElementAt(2);
            Assert.False(farm2.IsPlayerOwner(TestHelper.Bob));
            Assert.Equal(3, farm2.GetScore());

            Assert.Equal(7, room.GetPlayerScore(TestHelper.Bob).OverallScore);
        }

        /// <summary>
        ///       R
        ///   |    \  |
        /// W |+++  \_| R
        ///   |   +   |
        ///       W
        /// 
        ///       W
        ///   |   +  /|
        /// W |++ /   | C
        ///   |/      |
        ///       C
        /// </summary>
        [Fact]
        public void GetScore1()
        {
            var room = TestHelper.GetDefaultGame(TestHelper.Bob);

            var gameMove1 = new GameMove()
            {
                PlayerName = TestHelper.Bob,
                TileId = "RRWW(0)",
                TileRotation = 0,
                Location = new Point(0, 0),
                PartName = null
            };
            room.MakeMove(gameMove1);

            var gameMove2 = new GameMove()
            {
                PlayerName = TestHelper.Bob,
                TileId = "WCCW(0)",
                TileRotation = 0,
                Location = new Point(0, -1),
                PartName = "Farm_0"
            };
            room.MakeMove(gameMove2);

            var score = room.GetPlayerScore(TestHelper.Bob);
            Assert.Equal(0, score.OverallScore);

            Assert.Single(room.GetAllCities());
            Assert.Single(room.GetAllRoads());
            Assert.Equal(3, room.GetAllFarms().Count());

            Assert.Equal(2, room.GetAllFarms().ElementAt(0).OpenBorders.Count);
            Assert.Equal(0, room.GetAllFarms().ElementAt(0).GetScore());

            Assert.Equal(4, room.GetAllFarms().ElementAt(1).OpenBorders.Count);
            Assert.Equal(0, room.GetAllFarms().ElementAt(1).GetScore());

            Assert.Equal(2, room.GetAllFarms().ElementAt(2).OpenBorders.Count);
            Assert.Equal(0, room.GetAllFarms().ElementAt(2).GetScore());
        }

        /// <summary>
        ///       F                F
        ///   |       |        |       |
        /// F |   ++++| W    W |++++   | F
        ///   |   +   |        |       |
        ///       W                F
        ///       
        /// 
        ///       W
        ///   |   +   |
        /// F |   +   | F
        ///   |   +   |
        ///       W
        /// </summary>
        [Fact]
        public void GetScore2()
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
                TileId = "FWWF(0)",
                TileRotation = 0,
                Location = new Point(-1, 0),
                PartName = null
            };
            room.MakeMove(gameMove1);

            var gameMove2 = new GameMove()
            {
                PlayerName = TestHelper.Bob,
                TileId = "WFWF(0)",
                TileRotation = 0,
                Location = new Point(-1, -1),
                PartName = null
            };
            room.MakeMove(gameMove2);

            Assert.Single(room.GetAllFarms());
            Assert.Equal(9, room.GetAllFarms().ElementAt(0).OpenBorders.Count);
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
        /// R |---O   | F  (rotated)
        ///   |   +   |
        ///       W
        /// </summary>
        [Fact]
        public void GetScore3()
        {
            var room = TestHelper.GetDefaultGame(TestHelper.Bob);

            var gameMove1 = new GameMove()
            {
                PlayerName = TestHelper.Bob,
                TileId = "FFWF(0)",
                TileRotation = 0,
                Location = new Point(0, 0),
                PartName = null
            };
            room.MakeMove(gameMove1);


            var gameMove2 = new GameMove()
            {
                PlayerName = TestHelper.Bob,
                TileId = "FWRW(0)",
                TileRotation = 1,
                Location = new Point(0, -1),
                PartName = null
            };
            room.MakeMove(gameMove2);

            Assert.Equal(2, room.GetAllFarms().Count());

            Assert.Equal(2, room.GetAllFarms().ElementAt(0).OpenBorders.Count);
            Assert.Equal(6, room.GetAllFarms().ElementAt(1).OpenBorders.Count);
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
        /// W |++++   | F   (rotated 2)
        ///   |       |
        ///       F
        /// </summary>
        [Fact]
        public void GetScore4()
        {
            var room = TestHelper.GetDefaultGame(TestHelper.Bob);

            var gameMove1 = new GameMove()
            {
                PlayerName = TestHelper.Bob,
                TileId = "FFWF(0)",
                TileRotation = 0,
                Location = new Point(0, 0),
                PartName = null
            };
            room.MakeMove(gameMove1);

            var gameMove2 = new GameMove()
            {
                PlayerName = TestHelper.Bob,
                TileId = "FWWF(0)",
                TileRotation = 2,
                Location = new Point(0, -1),
                PartName = null
            };
            room.MakeMove(gameMove2);

            Assert.Single(room.GetAllFarms());
            Assert.Equal(7, room.GetAllFarms().ElementAt(0).OpenBorders.Count);
        }

        /// <summary>
        ///       R
        ///   |    \  |
        /// W |+++  \_| R
        ///   |   +   |
        ///       W
        /// 
        ///       W
        ///   |   +  /|
        /// R |-----| | C
        ///   |   +  \|
        ///       W
        /// </summary>
        [Fact]
        public void GetScore5()
        {
            var room = TestHelper.GetDefaultGame(TestHelper.Bob);

            var gameMove1 = new GameMove()
            {
                PlayerName = TestHelper.Bob,
                TileId = "RRWW(0)",
                TileRotation = 0,
                Location = new Point(0, 0),
                PartName = "Farm_1"
            };
            room.MakeMove(gameMove1);

            var gameMove2 = new GameMove()
            {
                PlayerName = TestHelper.Bob,
                TileId = "WCWR(0)",
                TileRotation = 0,
                Location = new Point(0, -1),
                PartName = null
            };
            room.MakeMove(gameMove2);

            var card2 = room.GetTile("WCWR(0)");

            Assert.Equal(5, room.GetAllFarms().Count());

            Assert.Equal(2, room.GetAllFarms().ElementAt(0).OpenBorders.Count);
            Assert.Equal(3, room.GetAllFarms().ElementAt(1).OpenBorders.Count);
            Assert.Equal(1, room.GetAllFarms().ElementAt(2).OpenBorders.Count);
            Assert.Equal(2, room.GetAllFarms().ElementAt(3).OpenBorders.Count);
            Assert.Equal(2, room.GetAllFarms().ElementAt(4).OpenBorders.Count);
        }
    }
}
