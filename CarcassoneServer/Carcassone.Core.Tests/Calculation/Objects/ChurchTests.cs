using Carcassone.Core.Calculation.Objects;
using Carcassone.Core.Cards;
using Carcassone.Core.Fields;
using Carcassone.Core.Players;
using Xunit;

namespace Carcassone.Core.Tests.Calculation.Objects
{
    public class ChurchTests
    {
        /// <summary>
        /// 9 полей с картой церкви в центальной фишка игрока.
        /// </summary>
        [Fact]
        public void GetPointsTest()
        {
            var gameRoom = new GameRoom();
            gameRoom.PlayersPool.AddHumanPlayer("Jack");
            gameRoom.PlayersPool.MoveToNextPlayer();

            var churchCard = new FFFF("FFFF", 0);
            gameRoom.PutCardInField(churchCard, gameRoom.FieldBoard.GetCenter());
            gameRoom.PutChipInCard(churchCard.Parts[0], gameRoom.PlayersPool.GetCurrentPlayer().Name);
            gameRoom.EndTurn();
            Assert.Single(gameRoom.ScoreCalculator.Churches);
            Assert.Equal(1, gameRoom.GetPlayerScore(gameRoom.PlayersPool.GetCurrentPlayer()).ChurchesScore);

            var churchCard1 = new FFFF("FFFF", 0);
            gameRoom.PutCardInField(churchCard1, gameRoom.FieldBoard.GetField(0, 1)); // top center
            gameRoom.EndTurn();
            Assert.Equal(2, gameRoom.ScoreCalculator.Churches.Count);
            Assert.Equal(2, gameRoom.GetPlayerScore(gameRoom.PlayersPool.GetCurrentPlayer()).ChurchesScore);

            var churchCard2 = new FFFF("FFFF", 0);
            gameRoom.PutCardInField(churchCard2, gameRoom.FieldBoard.GetField(1, 1)); // top right
            gameRoom.EndTurn();
            Assert.Equal(3, gameRoom.ScoreCalculator.Churches.Count);
            Assert.Equal(3, gameRoom.GetPlayerScore(gameRoom.PlayersPool.GetCurrentPlayer()).ChurchesScore);

            var churchCard3 = new FFFF("FFFF", 0);
            gameRoom.PutCardInField(churchCard3, gameRoom.FieldBoard.GetField(-1, 1)); // top left
            gameRoom.EndTurn();
            Assert.Equal(4, gameRoom.ScoreCalculator.Churches.Count);
            Assert.Equal(4, gameRoom.GetPlayerScore(gameRoom.PlayersPool.GetCurrentPlayer()).ChurchesScore);

            var churchCard4 = new FFFF("FFFF", 0);
            gameRoom.PutCardInField(churchCard4, gameRoom.FieldBoard.GetField(-1, 0)); // mid left
            gameRoom.EndTurn();
            Assert.Equal(5, gameRoom.ScoreCalculator.Churches.Count);
            Assert.Equal(5, gameRoom.GetPlayerScore(gameRoom.PlayersPool.GetCurrentPlayer()).ChurchesScore);

            var churchCard5 = new FFFF("FFFF", 0);
            gameRoom.PutCardInField(churchCard5, gameRoom.FieldBoard.GetField(1, 0)); // mid right
            gameRoom.EndTurn();
            Assert.Equal(6, gameRoom.ScoreCalculator.Churches.Count);
            Assert.Equal(6, gameRoom.GetPlayerScore(gameRoom.PlayersPool.GetCurrentPlayer()).ChurchesScore);

            var churchCard6 = new FFFF("FFFF", 0);
            gameRoom.PutCardInField(churchCard6, gameRoom.FieldBoard.GetField(0, -1)); // bot center
            gameRoom.EndTurn();
            Assert.Equal(7, gameRoom.ScoreCalculator.Churches.Count);
            Assert.Equal(7, gameRoom.GetPlayerScore(gameRoom.PlayersPool.GetCurrentPlayer()).ChurchesScore);

            var churchCard7 = new FFFF("FFFF", 0);
            gameRoom.PutCardInField(churchCard7, gameRoom.FieldBoard.GetField(1, -1)); // bot right
            gameRoom.EndTurn();
            Assert.Equal(8, gameRoom.ScoreCalculator.Churches.Count);
            Assert.Equal(8, gameRoom.GetPlayerScore(gameRoom.PlayersPool.GetCurrentPlayer()).ChurchesScore);

            var churchCard8 = new FFFF("FFFF", 0);
            gameRoom.PutCardInField(churchCard8, gameRoom.FieldBoard.GetField(-1, -1)); // bot left
            gameRoom.EndTurn();
            Assert.Equal(9, gameRoom.ScoreCalculator.Churches.Count);
            Assert.Equal(18, gameRoom.GetPlayerScore(gameRoom.PlayersPool.GetCurrentPlayer()).ChurchesScore);

            Assert.True(gameRoom.ScoreCalculator.Churches[0].IsFinished);
        }
    }
}
