using Carcassone.Core.Calculation.Base.Roads;
using Carcassone.Core.Calculation.Base.Tiles;
using Carcassone.Core.Players;
using Carcassone.Core.Tiles;
using System.Drawing;
using System.Linq;
using Xunit;

namespace Carcassone.Core.Tests.Buisness
{
    public class RoadTests
    {

        /// <summary>
        ///       R
        ///   |   |   |
        /// R |---|---| R
        ///   |   |   |
        ///       R
        /// </summary>
        [Fact]
        public void CalculationTest_CrossRoad()
        {
            var room = TestHelper.GetDefaultGame(TestHelper.Bob);
            room.ForTests_ResetTilesStack();
            room.ForTests_CustomTile(typeof(RRRR), 1, 0);

            var gameMove0 = new GameMove
            (
                playerName: TestHelper.Bob,
                tileId: "RRRR(0)",
                tileRotation: 0,
                tileLocation: new Point(0, 0),
                partName: null
            );
            room.MakeMove(gameMove0);

            var roads = room.State.Calculated.OfType<Road>();
            Assert.Equal(4, roads.Count());
            foreach (var road in roads)
            {
                Assert.Equal(1, road.GetScore(room.State));
                Assert.False(road.IsComplete(room.State));
            }
        }

        /// <summary>
        ///       F               F
        ///   |       |       |       |
        /// F |     --| R   R |--     | F
        ///   |   /   |       |   \   |
        ///       R              R
        ///       
        ///       R               R
        ///   |   \   |       |   /   |
        /// F |     --| R   R |--     | F
        ///   |       |       |       |
        ///       F               F
        /// </summary>
        [Fact]
        public void CalculationTest_RoadRing()
        {
            var room = TestHelper.GetDefaultGame(TestHelper.Bob);
            room.ForTests_ResetTilesStack();
            room.ForTests_CustomTile(typeof(FFRR), 4, 0);

            var gameMove0 = new GameMove
            (
                playerName: TestHelper.Bob,
                tileId: "FFRR(0)",
                tileRotation: 2,
                tileLocation: new Point(0, 0),
                partName: "Road_0"
            );
            room.MakeMove(gameMove0);

            var gameMove1 = new GameMove
            (
                playerName: TestHelper.Bob,
                tileId: "FFRR(1)",
                tileRotation: 3,
                tileLocation: new Point(0, 1),
                partName: null
            );
            room.MakeMove(gameMove1);

            var gameMove2 = new GameMove
            (
                playerName: TestHelper.Bob,
                tileId: "FFRR(2)",
                tileRotation: 0,
                tileLocation: new Point(1, 1),
                partName: null
            );
            room.MakeMove(gameMove2);

            var gameMove3 = new GameMove
            (
                playerName: TestHelper.Bob,
                tileId: "FFRR(3)",
                tileRotation: 1,
                tileLocation: new Point(1, 0),
                partName: null
            );
            room.MakeMove(gameMove3);

            var roads = room.State.Calculated.OfType<Road>();
            Assert.Single(roads);
            var road = roads.Single();
            Assert.Equal(8, road.GetScore(room.State));
            Assert.True(road.IsComplete(room.State));
            Assert.Single(road.Parts.Where(p => p.GetPartOwner(room.State) != null));
        }

        /// <summary>
        ///       F
        ///   |       |
        /// F |   o   | F
        ///   |   |   |
        ///       R
        /// 
        ///       R
        ///   |   |   |
        /// R |-------| R
        ///   |       |
        ///       F
        /// </summary>
        [Fact]
        public void CalculationTest_FlatRoad()
        {
            var room = TestHelper.GetDefaultGame(TestHelper.Bob);
            room.ForTests_ResetTilesStack();
            room.ForTests_CustomTile(typeof(FFRF), 1, 0);
            room.ForTests_CustomTile(typeof(FRRR), 1, 0);

            var gameMove0 = new GameMove
            (
                playerName: TestHelper.Bob,
                tileId: "FFRF(0)",
                tileRotation: 0,
                tileLocation: new Point(0, 0),
                partName: "Road_0"
            );
            room.MakeMove(gameMove0);

            var gameMove1 = new GameMove
            (
                playerName: TestHelper.Bob,
                tileId: "FRRR(0)",
                tileRotation: 2,
                tileLocation: new Point(0, -1),
                partName: null
            );
            room.MakeMove(gameMove1);

            var roads = room.State.Calculated.OfType<Road>();
            Assert.Equal(3, roads.Count());
            var road = roads.First(r => r.IsComplete(room.State));
            Assert.Equal(4, road.GetScore(room.State));
            Assert.True(road.IsComplete(room.State));
            Assert.Single(road.Parts.Where(p => p.GetPartOwner(room.State) != null));
        }
    }
}
