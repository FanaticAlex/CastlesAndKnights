using Carcassone.Core.Calculation.Objects;
using Carcassone.Core.Cards;
using Carcassone.Core.Fields;
using Carcassone.Core.Players;
using System.Linq;
using Xunit;

namespace Carcassone.Core.Tests.Calculation.Objects
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
            var bob = room.AddHumanPlayer("bob");

            var card0 = room.GetCard("RRRR_0");

            room.PutCardInField(card0, room.GetField($"{0}_{0}"));
            room.EndTurn();
                
            var roads = room.GetRoads();
            Assert.Equal(4, roads.Count);
            foreach (var road in roads)
            {
                Assert.Equal(1, road.GetPoints());
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
            var bob = room.AddHumanPlayer("bob");

            var card0 = room.GetCard("FFRR_0");
            card0.RotationsCount = 2;
            var card1 = room.GetCard("FFRR_1");
            card1.RotationsCount = 3;
            var card2 = room.GetCard("FFRR_2");
            card2.RotationsCount = 0;
            var card3 = room.GetCard("FFRR_3");
            card3.RotationsCount = 1;

            room.PutCardInField(card0, room.GetField($"{0}_{0}"));
            room.EndTurn();
            room.PutCardInField(card1, room.GetField($"{0}_{1}"));
            room.EndTurn();
            room.PutCardInField(card2, room.GetField($"{1}_{1}"));
            room.EndTurn();
            room.PutCardInField(card3, room.GetField($"{1}_{0}"));
            room.EndTurn();

            var roads = room.GetRoads();
            Assert.Single(roads);
            Assert.Equal(8, roads.Single().GetPoints());
            Assert.True(roads.Single().IsFinished);
        }
    }
}
