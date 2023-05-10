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
            var bob = room.PlayersPool.AddPlayer("bob", PlayerType.Human);

            var card0 = room.GetCard("RRRR(0)");

            room.PutCardInField(card0, room.FieldBoard.GetField($"{0}_{0}"));
            room.EndTurn();
                
            var roads = room.ScoreCalculator.Roads;
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
            var bob = room.PlayersPool.AddPlayer("bob", PlayerType.Human);

            var card0 = room.GetCard("FFRR(0)");
            card0.RotationsCount = 2;
            var card1 = room.GetCard("FFRR(1)");
            card1.RotationsCount = 3;
            var card2 = room.GetCard("FFRR(2)");
            card2.RotationsCount = 0;
            var card3 = room.GetCard("FFRR(3)");
            card3.RotationsCount = 1;

            var chip = new Chip(bob);
            card0.Parts.First(p => p is RoadPart).Chip = chip;

            room.PutCardInField(card0, room.FieldBoard.GetField($"{0}_{0}"));
            room.EndTurn();
            room.PutCardInField(card1, room.FieldBoard.GetField($"{0}_{1}"));
            room.EndTurn();
            room.PutCardInField(card2, room.FieldBoard.GetField($"{1}_{1}"));
            room.EndTurn();
            room.PutCardInField(card3, room.FieldBoard.GetField($"{1}_{0}"));
            room.EndTurn();

            var roads = room.ScoreCalculator.Roads;
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
            var playerName = "bob";
            room.PlayersPool.AddPlayer(playerName, PlayerType.Human);

            var card0 = room.GetCard("FFRF(0)");
            var card1 = room.GetCard("FRRR(0)");
            card1.RotationsCount = 2;

            var chip = new Chip(room.PlayersPool.GetPlayer(playerName));
            card0.Parts.First(p => p is RoadPart).Chip = chip;

            room.PutCardInField(card0, room.FieldBoard.GetField($"{0}_{0}"));
            room.EndTurn();
            room.PutCardInField(card1, room.FieldBoard.GetField($"{0}_{-1}"));
            room.EndTurn();

            var roads = room.ScoreCalculator.Roads;
            Assert.Equal(3, roads.Count);
            var road = roads.First(r => r.IsFinished);
            Assert.Equal(4, road.GetPoints(room.CardsPool));
            Assert.True(road.IsFinished);
            Assert.Single(road.PartsIds.Select(id => room.CardsPool.GetPart(id)).Where(p => p.Flag != null));
        }
    }
}
