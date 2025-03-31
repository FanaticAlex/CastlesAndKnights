using Carcassone.Core.Calculation.Objects;
using Carcassone.Core.Cards;
using Carcassone.Core.Players;
using System.Linq;
using Xunit;

namespace Carcassone.Core.Tests.Buisness
{
    public class CastleTests
    {

        /// <summary>
        ///       C
        ///   |\-----/|
        /// W |+++++++| W
        ///   |/-----\|
        ///       C
        /// </summary>
        [Fact]
        public void CalculationTest_NotClosedCastle()
        {
            var room = new GameRoom();
            var player = new Player() { Name = "bob", PlayerType = PlayerType.Human };
            room.PlayersPool.AddPlayer(player);

            var gameMove0 = new GameMove()
            {
                PlayerName = player.Name,
                CardId = "CWCW(0)",
                CardRotation = 0,
                FieldId = $"{0}_{0}",
                PartName = "Castle_0"
            };
            room.MakeMove(gameMove0);

            var castles = room.ScoreCalculator.Castles;
            Assert.True(castles.Count == 2);
            Assert.False(castles[0].IsFinished);
            Assert.False(castles[1].IsFinished);
            Assert.True(castles[0].IsPlayerOwner(room.PlayersPool.GetPlayer(player.Name), room.CardsPool));
            
            var score = room.GetPlayerScore(room.PlayersPool.GetPlayer(player.Name));
            Assert.Equal(1, score.CastlesScore);
            Assert.Equal(6, room.PlayersPool.GetPlayer(player.Name).ChipCount);
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
        public void CalculationTest_DoubleCastle()
        {
            var room = new GameRoom();
            var player = new Player() { Name = "bob", PlayerType = PlayerType.Human };
            room.PlayersPool.AddPlayer(player);

            var gameMove0 = new GameMove()
            {
                PlayerName = player.Name,
                CardId = "CFFF(0)",
                CardRotation = 2,
                FieldId = $"{0}_{0}",
                PartName = null
            };
            room.MakeMove(gameMove0);

            var gameMove1 = new GameMove()
            {
                PlayerName = player.Name,
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
            Assert.True(castle.IsPlayerOwner(room.PlayersPool.GetPlayer(player.Name), room.CardsPool));
            Assert.Contains(castle.PartsIds.Select(id => room.CardsPool.GetPart(id)), p => p.Flag != null);

            var score = room.GetPlayerScore(room.PlayersPool.GetPlayer(player.Name));
            Assert.Equal(4, score.CastlesScore);
            Assert.Equal(7, room.PlayersPool.GetPlayer(player.Name).ChipCount);
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
            var player = new Player() { Name = "bob", PlayerType = PlayerType.Human };
            room.PlayersPool.AddPlayer(player);

            var gameMove0 = new GameMove()
            {
                PlayerName = player.Name,
                CardId = "CWCW(0)",
                CardRotation = 0,
                FieldId = $"{0}_{0}",
                PartName = null
            };
            room.MakeMove(gameMove0);

            var gameMove1 = new GameMove()
            {
                PlayerName = player.Name,
                CardId = "CFFF(0)",
                CardRotation = 2,
                FieldId = $"{0}_{1}",
                PartName = null
            };
            room.MakeMove(gameMove1);

            var gameMove2 = new GameMove()
            {
                PlayerName = player.Name,
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
            Assert.True(castle.IsPlayerOwner(room.PlayersPool.GetPlayer(player.Name), room.CardsPool));
            Assert.Contains(castle.PartsIds.Select(id => room.CardsPool.GetPart(id)), p => p.Flag != null);

            var score = room.GetPlayerScore(room.PlayersPool.GetPlayer(player.Name));
            Assert.Equal(4, score.CastlesScore);
            Assert.Equal(7, room.PlayersPool.GetPlayer(player.Name).ChipCount);
        }
    }
}
