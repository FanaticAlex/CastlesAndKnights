using Carcassone.Core.Calculation.Base.Cities;
using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.Base.Roads;
using Carcassone.Core.Calculation.Base.Tiles;
using Carcassone.Core.Calculation.RiverExtension.Tiles;
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
            room.ForTests_ResetTilesStack();
            room.ForTests_CustomTile(typeof(CRFR), 1, 0);
            room.ForTests_CustomTile(typeof(CFFF), 1, 0);

            var gameMove1 = new GameMove
            (
                playerName: TestHelper.Bob,
                tileId: "CRFR(0)",
                tileRotation: 2,
                tileLocation: new Point(0, 0),
                partName: "Farm_0"
            );
            room.MakeMove(gameMove1);

            var gameMove2 = new GameMove
            (
                playerName: TestHelper.Bob,
                tileId: "CFFF(0)",
                tileRotation: 0,
                tileLocation: new Point(0, -1),
                partName: "City_0"
            );
            room.MakeMove(gameMove2);

            // check castles
            var Cities = room.State.Calculated.OfType<City>();
            Assert.Single(Cities);
            var city = Cities.Single();
            Assert.True(city.IsComplete(room.State));
            Assert.Equal(4, city.GetScore(room.State));
            Assert.True(city.IsPlayerOwner(TestHelper.Bob, room.State));

            // check farms
            var farms = room.State.Calculated.OfType<Farm>();
            Assert.Equal(3, farms.Count());
            var farm0 = farms.ElementAt(0);
            Assert.True(farm0.IsPlayerOwner(TestHelper.Bob, room.State));
            Assert.Equal(3, farm0.GetScore(room.State));

            var farm1 = farms.ElementAt(1);
            Assert.False(farm1.IsPlayerOwner(TestHelper.Bob, room.State));
            Assert.Equal(0, farm1.GetScore(room.State));

            var farm2 = farms.ElementAt(2);
            Assert.False(farm2.IsPlayerOwner(TestHelper.Bob, room.State));
            Assert.Equal(3, farm2.GetScore(room.State));

            var bob = room.PlayersPool.GetPlayer(TestHelper.Bob);
            Assert.Equal(7, bob?.GetPlayerOverallScore(room.State));
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
            room.ForTests_ResetTilesStack();
            room.ForTests_CustomTile(typeof(RRWW), 1, 0);
            room.ForTests_CustomTile(typeof(WCCW), 1, 0);

            var gameMove1 = new GameMove
            (
                playerName: TestHelper.Bob,
                tileId: "RRWW(0)",
                tileRotation: 0,
                tileLocation: new Point(0, 0),
                partName: null
            );
            room.MakeMove(gameMove1);

            var gameMove2 = new GameMove
            (
                playerName: TestHelper.Bob,
                tileId: "WCCW(0)",
                tileRotation: 0,
                tileLocation: new Point(0, -1),
                partName: "Farm_0"
            );
            room.MakeMove(gameMove2);

            var bob = room.PlayersPool.GetPlayer(TestHelper.Bob);
            var score = bob?.GetPlayerOverallScore(room.State);
            Assert.Equal(0, score);

            var cities = room.State.Calculated.OfType<City>();
            var roads = room.State.Calculated.OfType<Road>();
            var farms = room.State.Calculated.OfType<Farm>();
            Assert.Single(cities);
            Assert.Single(roads);
            Assert.Equal(3, farms.Count());

            Assert.Equal(2, farms.ElementAt(0).GetOpenBorders(room.State).Count);
            Assert.Equal(0, farms.ElementAt(0).GetScore(room.State));

            Assert.Equal(4, farms.ElementAt(1).GetOpenBorders(room.State).Count);
            Assert.Equal(0, farms.ElementAt(1).GetScore(room.State));

            Assert.Equal(2, farms.ElementAt(2).GetOpenBorders(room.State).Count);
            Assert.Equal(0, farms.ElementAt(2).GetScore(room.State));
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
            room.ForTests_ResetTilesStack();
            room.ForTests_CustomTile(typeof(FFWF), 1, 0);
            room.ForTests_CustomTile(typeof(FWWF), 1, 0);
            room.ForTests_CustomTile(typeof(WFWF), 1, 0);

            var gameMove0 = new GameMove
            (
                playerName: TestHelper.Bob,
                tileId: "FFWF(0)",
                tileRotation: 1,
                tileLocation: new Point(0, 0),
                partName: null
            );
            room.MakeMove(gameMove0);

            var gameMove1 = new GameMove
            (
                playerName: TestHelper.Bob,
                tileId: "FWWF(0)",
                tileRotation: 0,
                tileLocation: new Point(-1, 0),
                partName: null
            );
            room.MakeMove(gameMove1);

            var gameMove2 = new GameMove
            (
                playerName: TestHelper.Bob,
                tileId: "WFWF(0)",
                tileRotation: 0,
                tileLocation: new Point(-1, -1),
                partName: null
            );
            room.MakeMove(gameMove2);

            var farms = room.State.Calculated.OfType<Farm>();
            Assert.Single(farms);
            Assert.Equal(9, farms.ElementAt(0).GetOpenBorders(room.State).Count);
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
            room.ForTests_ResetTilesStack();
            room.ForTests_CustomTile(typeof(FFWF), 1, 0);
            room.ForTests_CustomTile(typeof(FWRW), 1, 0);

            var gameMove1 = new GameMove
            (
                playerName: TestHelper.Bob,
                tileId: "FFWF(0)",
                tileRotation: 0,
                tileLocation: new Point(0, 0),
                partName: null
            );
            room.MakeMove(gameMove1);


            var gameMove2 = new GameMove
            (
                playerName: TestHelper.Bob,
                tileId: "FWRW(0)",
                tileRotation: 1,
                tileLocation: new Point(0, -1),
                partName: null
            );
            room.MakeMove(gameMove2);

            var farms = room.State.Calculated.OfType<Farm>();
            Assert.Equal(2, farms.Count());

            Assert.Equal(2, farms.ElementAt(0).GetOpenBorders(room.State).Count);
            Assert.Equal(6, farms.ElementAt(1).GetOpenBorders(room.State).Count);
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
            room.ForTests_ResetTilesStack();
            room.ForTests_CustomTile(typeof(FFWF), 1, 0);
            room.ForTests_CustomTile(typeof(FWWF), 1, 0);

            var gameMove1 = new GameMove
            (
                playerName: TestHelper.Bob,
                tileId: "FFWF(0)",
                tileRotation: 0,
                tileLocation: new Point(0, 0),
                partName: null
            );
            room.MakeMove(gameMove1);

            var gameMove2 = new GameMove
            (
                playerName: TestHelper.Bob,
                tileId: "FWWF(0)",
                tileRotation: 2,
                tileLocation: new Point(0, -1),
                partName: null
            );
            room.MakeMove(gameMove2);

            var farms = room.State.Calculated.OfType<Farm>();
            Assert.Single(farms);
            Assert.Equal(7, farms.ElementAt(0).GetOpenBorders(room.State).Count);
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
            room.ForTests_ResetTilesStack();
            room.ForTests_CustomTile(typeof(RRWW), 1, 0);
            room.ForTests_CustomTile(typeof(WCWR), 1, 0);

            var gameMove1 = new GameMove
            (
                playerName: TestHelper.Bob,
                tileId: "RRWW(0)",
                tileRotation: 0,
                tileLocation: new Point(0, 0),
                partName: "Farm_1"
            );
            room.MakeMove(gameMove1);

            var gameMove2 = new GameMove
            (
                playerName: TestHelper.Bob,
                tileId: "WCWR(0)",
                tileRotation: 0,
                tileLocation: new Point(0, -1),
                partName: null
            );
            room.MakeMove(gameMove2);

            var card2 = room.GetTile("WCWR(0)");

            var farms = room.State.Calculated.OfType<Farm>();
            Assert.Equal(5, farms.Count());

            Assert.Equal(2, farms.ElementAt(0).GetOpenBorders(room.State).Count);
            Assert.Equal(3, farms.ElementAt(1).GetOpenBorders(room.State).Count);
            Assert.Single(farms.ElementAt(2).GetOpenBorders(room.State));
            Assert.Equal(2, farms.ElementAt(3).GetOpenBorders(room.State).Count);
            Assert.Equal(2, farms.ElementAt(4).GetOpenBorders(room.State).Count);
        }
    }
}
