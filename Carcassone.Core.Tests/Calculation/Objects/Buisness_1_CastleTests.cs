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

            var gameMove0 = new GameMove()
            {
                PlayerName = playerName,
                CardId = "CFFF(0)",
                CardRotation = 2,
                FieldId = $"{0}_{0}",
                PartName = null
            };
            room.MakeMove(gameMove0);

            var gameMove1 = new GameMove()
            {
                PlayerName = playerName,
                CardId = "CFFF(1)",
                CardRotation = 0,
                FieldId = $"{0}_{-1}",
                PartName = "Castle_0"
            };
            room.MakeMove(gameMove1);

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

            var gameMove0 = new GameMove()
            {
                PlayerName = playerName,
                CardId = "CWCW(0)",
                CardRotation = 0,
                FieldId = $"{0}_{0}",
                PartName = null
            };
            room.MakeMove(gameMove0);

            var gameMove1 = new GameMove()
            {
                PlayerName = playerName,
                CardId = "CFFF(0)",
                CardRotation = 2,
                FieldId = $"{0}_{1}",
                PartName = null
            };
            room.MakeMove(gameMove1);

            var gameMove2 = new GameMove()
            {
                PlayerName = playerName,
                CardId = "CFFF(1)",
                CardRotation = 0,
                FieldId = $"{0}_{-1}",
                PartName = "Castle_0"
            };
            room.MakeMove(gameMove2);

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
