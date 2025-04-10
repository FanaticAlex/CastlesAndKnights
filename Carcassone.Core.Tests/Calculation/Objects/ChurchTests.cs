using Carcassone.Core.Cards;
using Carcassone.Core.Players;
using Xunit;

namespace Carcassone.Core.Tests.Buisness
{
    public class ChurchTests
    {
        /// <summary>
        /// 9 полей с картой церкви в центальной фишка игрока.
        /// 
        ///       F               F               F
        ///   |       |       |       |       |       |
        /// F |    ___| R   R |_______| R   R |___    | F
        ///   |   |   |       |       |       |   |   |
        ///       R               F               R
        /// 
        ///       R               F               R
        ///   |   |   |       |       |       |   |   |
        /// F |   |   | F   F |   O   | F   F |   |   | F
        ///   |   |   |       |       |       |   |   |
        ///       R               F               R
        /// 
        ///       R               F               R
        ///   |   |   |       |       |       |   |   |
        /// F |   \___| R   R |_______| R   R |__/    | F
        ///   |       |       |       |       |       |
        ///       F               F               F
        /// 
        /// </summary>
        [Fact]
        public void GetPointsTest()
        {
            var room = new GameRoom();
            var name = "Jack";
            room.PlayersPool.AddPlayer(name, PlayerType.Human);
            room.PlayersPool.MoveToNextPlayer();

            var gameMove0 = new GameMove()
            {
                PlayerName = name,
                CardId = "FFFF(0)",
                CardRotation = 0,
                FieldId = $"{0}_{0}",
                PartName = "Church_0"
            };
            room.MakeMove(gameMove0);
            Assert.Single(room.ScoreCalculator.Churches);
            Assert.Equal(1, room.GetPlayerScore(room.PlayersPool.GetCurrentPlayer().Name).ChurchesScore);

            var gameMove1 = new GameMove()
            {
                PlayerName = name,
                CardId = "RFRF(0)",
                CardRotation = 1,
                FieldId = $"{0}_{1}",
                PartName = null
            };
            room.MakeMove(gameMove1);
            Assert.Single(room.ScoreCalculator.Churches);
            Assert.Equal(2, room.GetPlayerScore(room.PlayersPool.GetCurrentPlayer().Name).ChurchesScore);

            var gameMove2 = new GameMove()
            {
                PlayerName = name,
                CardId = "FFRR(0)",
                CardRotation = 0,
                FieldId = $"{1}_{1}",
                PartName = null
            };
            room.MakeMove(gameMove2);
            Assert.Single(room.ScoreCalculator.Churches);
            Assert.Equal(3, room.GetPlayerScore(room.PlayersPool.GetCurrentPlayer().Name).ChurchesScore);

            var gameMove3 = new GameMove()
            {
                PlayerName = name,
                CardId = "FFRR(1)",
                CardRotation = 3,
                FieldId = $"{-1}_{1}",
                PartName = null
            };
            room.MakeMove(gameMove3);
            Assert.Single(room.ScoreCalculator.Churches);
            Assert.Equal(4, room.GetPlayerScore(room.PlayersPool.GetCurrentPlayer().Name).ChurchesScore);

            var gameMove4 = new GameMove()
            {
                PlayerName = name,
                CardId = "RFRF(1)",
                CardRotation = 0,
                FieldId = $"{-1}_{0}",
                PartName = null
            };
            room.MakeMove(gameMove4);
            Assert.Single(room.ScoreCalculator.Churches);
            Assert.Equal(5, room.GetPlayerScore(room.PlayersPool.GetCurrentPlayer().Name).ChurchesScore);

            var gameMove5 = new GameMove()
            {
                PlayerName = name,
                CardId = "RFRF(2)",
                CardRotation = 0,
                FieldId = $"{1}_{0}",
                PartName = null
            };
            room.MakeMove(gameMove5);
            Assert.Single(room.ScoreCalculator.Churches);
            Assert.Equal(6, room.GetPlayerScore(room.PlayersPool.GetCurrentPlayer().Name).ChurchesScore);

            var gameMove6 = new GameMove()
            {
                PlayerName = name,
                CardId = "RFRF(3)",
                CardRotation = 1,
                FieldId = $"{0}_{-1}",
                PartName = null
            };
            room.MakeMove(gameMove6);
            Assert.Single(room.ScoreCalculator.Churches);
            Assert.Equal(7, room.GetPlayerScore(room.PlayersPool.GetCurrentPlayer().Name).ChurchesScore);

            var gameMove7 = new GameMove()
            {
                PlayerName = name,
                CardId = "FFRR(2)",
                CardRotation = 1,
                FieldId = $"{1}_{-1}",
                PartName = null
            };
            room.MakeMove(gameMove7);
            Assert.Single(room.ScoreCalculator.Churches);
            Assert.Equal(8, room.GetPlayerScore(room.PlayersPool.GetCurrentPlayer().Name).ChurchesScore);

            var gameMove8 = new GameMove()
            {
                PlayerName = name,
                CardId = "FFRR(3)",
                CardRotation = 2,
                FieldId = $"{-1}_{-1}",
                PartName = null
            };
            room.MakeMove(gameMove8);
            Assert.Single(room.ScoreCalculator.Churches);
            Assert.Equal(18, room.GetPlayerScore(room.PlayersPool.GetCurrentPlayer().Name).ChurchesScore);

            Assert.True(room.ScoreCalculator.Churches[0].IsFinished);
        }
    }
}
