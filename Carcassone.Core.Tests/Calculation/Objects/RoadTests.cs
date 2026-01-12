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
                CardId = "RRRR(0)",
                CardRotation = 0,
                FieldId = $"{0}_{0}",
                PartName = null
            };
            room.MakeMove(gameMove0);
                
            var roads = room.ScoreCalculator.RoadsManager.Objects;
            Assert.Equal(4, roads.Count);
            foreach (var road in roads)
            {
                Assert.Equal(1, road.GetPoints(room.CardsPool));
                Assert.False(road.IsFinished);
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
                CardId = "FFRR(0)",
                CardRotation = 2,
                FieldId = $"{0}_{0}",
                PartName = "Road_0"
            };
            room.MakeMove(gameMove0);

            var gameMove1 = new GameMove()
            {
                PlayerName = name,
                CardId = "FFRR(1)",
                CardRotation = 3,
                FieldId = $"{0}_{1}",
                PartName = null
            };
            room.MakeMove(gameMove1);

            var gameMove2 = new GameMove()
            {
                PlayerName = name,
                CardId = "FFRR(2)",
                CardRotation = 0,
                FieldId = $"{1}_{1}",
                PartName = null
            };
            room.MakeMove(gameMove2);

            var gameMove3 = new GameMove()
            {
                PlayerName = name,
                CardId = "FFRR(3)",
                CardRotation = 1,
                FieldId = $"{1}_{0}",
                PartName = null
            };
            room.MakeMove(gameMove3);

            var roads = room.ScoreCalculator.RoadsManager.Objects;
            Assert.Single(roads);
            var road = roads.Single();
            Assert.Equal(8, road.GetPoints(room.CardsPool));
            Assert.True(road.IsFinished);
            Assert.Single(road.PartsIds.Select(id => room.CardsPool.GetPart(id)).Where(p => p.Flag != null));
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
                CardId = "FFRF(0)",
                CardRotation = 0,
                FieldId = $"{0}_{0}",
                PartName = "Road_0"
            };
            room.MakeMove(gameMove0);

            var gameMove1 = new GameMove()
            {
                PlayerName = name,
                CardId = "FRRR(0)",
                CardRotation = 2,
                FieldId = $"{0}_{-1}",
                PartName = null
            };
            room.MakeMove(gameMove1);

            var roads = room.ScoreCalculator.RoadsManager.Objects;
            Assert.Equal(3, roads.Count);
            var road = roads.First(r => r.IsFinished);
            Assert.Equal(4, road.GetPoints(room.CardsPool));
            Assert.True(road.IsFinished);
            Assert.Single(road.PartsIds.Select(id => room.CardsPool.GetPart(id)).Where(p => p.Flag != null));
        }
    }
}
