using Carcassone.Core.Calculation.Objects;
using Carcassone.Core.Cards;
using Carcassone.Core.Players;
using System.Linq;
using Xunit;

namespace Carcassone.Core.Tests.Buisness
{
    public class Buisness_1_CastleTests
    {

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
        public void CalculationTest_DoubleCastle()
        {
            var room = new GameRoom();
            var playerName = "bob";
            room.PlayersPool.AddPlayer(playerName, PlayerType.Human);

            var card0 = room.GetCard("CFFF(0)");
            card0.RotationsCount = 2;
            var card1 = room.GetCard("CFFF(1)");
            card1.RotationsCount = 0;

            room.PutCardInField(card0, room.FieldBoard.GetField($"{0}_{0}"));
            room.EndTurn();

            room.PutCardInField(card1, room.FieldBoard.GetField($"{0}_{-1}"));
            var part = card1.Parts.Single(p => p is CastlePart);
            room.PutChipInCard(part, playerName);
            room.EndTurn();

            var castles = room.ScoreCalculator.Castles;
            Assert.Single(castles);
            var castle = castles.Single();
            Assert.Equal(4, castle.GetPoints(room.CardsPool));
            Assert.True(castle.IsFinished);
            Assert.True(castle.IsPlayerOwner(room.PlayersPool.GetPlayer(playerName), room.CardsPool));
            Assert.Contains(castle.PartsIds.Select(id => room.CardsPool.GetPart(id)), p => p.Flag != null);

            var score = room.GetPlayerScore(room.PlayersPool.GetPlayer(playerName));
            Assert.Equal(4, score.CastlesScore);
            Assert.Equal(7, room.PlayersPool.GetPlayer(playerName).ChipCount);
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
        ///   |\-----/|
        /// W |+++++++| W
        ///   |/-----\|
        ///       C
        /// 
        ///       C
        ///   |\_____/|
        /// F |       | F
        ///   |       |
        ///       F
        /// </summary>
        [Fact]
        public void CalculationTest_TwoRiverCastles()
        {
            var room = new GameRoom();
            var playerName = "bob";
            room.PlayersPool.AddPlayer(playerName, PlayerType.Human);

            var card0 = room.GetCard("CFFF(0)");
            card0.RotationsCount = 2;
            var card1 = room.GetCard("CWCW(0)");
            card1.RotationsCount = 0;
            var card2 = room.GetCard("CFFF(1)");
            card2.RotationsCount = 0;


            room.PutCardInField(card1, room.FieldBoard.GetField($"{0}_{0}"));
            room.EndTurn();

            room.PutCardInField(card0, room.FieldBoard.GetField($"{0}_{1}"));
            room.EndTurn();

            room.PutCardInField(card2, room.FieldBoard.GetField($"{0}_{-1}"));
            var part = card2.Parts.Single(p => p is CastlePart);
            room.PutChipInCard(part, playerName);
            room.EndTurn();

            var castles = room.ScoreCalculator.Castles;
            Assert.Equal(2, castles.Count);

            var castle = castles[1];
            Assert.Equal(4, castle.GetPoints(room.CardsPool));
            Assert.True(castle.IsFinished);
            Assert.True(castle.IsPlayerOwner(room.PlayersPool.GetPlayer(playerName), room.CardsPool));
            Assert.Contains(castle.PartsIds.Select(id => room.CardsPool.GetPart(id)), p => p.Flag != null);

            var score = room.GetPlayerScore(room.PlayersPool.GetPlayer(playerName));
            Assert.Equal(4, score.CastlesScore);
            Assert.Equal(7, room.PlayersPool.GetPlayer(playerName).ChipCount);
        }
    }
}
