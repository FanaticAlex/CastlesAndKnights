using Carcassone.Core.Tiles;
using Carcassone.Core.Board;
using Carcassone.Core.Players;
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
            var room = new GameRoom();
            var name = "bob";
            var bob = room.PlayersPool.AddPlayer(name, PlayerType.Human);

            var gameMove0 = new GameMove()
            {
                PlayerName = name,
                TileId = "RRRR(0)",
                TileRotation = 0,
                CellId = $"{0}_{0}",
                PartName = null
            };
            room.MakeMove(gameMove0);
                
            var roads = room.GetAllRoads();
            Assert.Equal(4, roads.Count());
            foreach (var road in roads)
            {
                Assert.Equal(1, road.GetScore());
                Assert.False(road.IsComplete());
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
            var room = new GameRoom();
            var name = "bob";
            var bob = room.PlayersPool.AddPlayer(name, PlayerType.Human);

            var gameMove0 = new GameMove()
            {
                PlayerName = name,
                TileId = "FFRR(0)",
                TileRotation = 2,
                CellId = $"{0}_{0}",
                PartName = "Road_0"
            };
            room.MakeMove(gameMove0);

            var gameMove1 = new GameMove()
            {
                PlayerName = name,
                TileId = "FFRR(1)",
                TileRotation = 3,
                CellId = $"{0}_{1}",
                PartName = null
            };
            room.MakeMove(gameMove1);

            var gameMove2 = new GameMove()
            {
                PlayerName = name,
                TileId = "FFRR(2)",
                TileRotation = 0,
                CellId = $"{1}_{1}",
                PartName = null
            };
            room.MakeMove(gameMove2);

            var gameMove3 = new GameMove()
            {
                PlayerName = name,
                TileId = "FFRR(3)",
                TileRotation = 1,
                CellId = $"{1}_{0}",
                PartName = null
            };
            room.MakeMove(gameMove3);

            var roads = room.GetAllRoads();
            Assert.Single(roads);
            var road = roads.Single();
            Assert.Equal(8, road.GetScore());
            Assert.True(road.IsComplete());
            Assert.Single(road.Parts.Where(p => p.Flag != null));
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
            var room = new GameRoom();
            var name = "bob";
            room.PlayersPool.AddPlayer(name, PlayerType.Human);

            var gameMove0 = new GameMove()
            {
                PlayerName = name,
                TileId = "FFRF(0)",
                TileRotation = 0,
                CellId = $"{0}_{0}",
                PartName = "Road_0"
            };
            room.MakeMove(gameMove0);

            var gameMove1 = new GameMove()
            {
                PlayerName = name,
                TileId = "FRRR(0)",
                TileRotation = 2,
                CellId = $"{0}_{-1}",
                PartName = null
            };
            room.MakeMove(gameMove1);

            var roads = room.GetAllRoads();
            Assert.Equal(3, roads.Count());
            var road = roads.First(r => r.IsComplete());
            Assert.Equal(4, road.GetScore());
            Assert.True(road.IsComplete());
            Assert.Single(road.Parts.Where(p => p.Flag != null));
        }
    }
}
