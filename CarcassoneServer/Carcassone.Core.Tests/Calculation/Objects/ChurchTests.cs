using Carcassone.Core.Cards;
using Carcassone.Core.Players;
using Xunit;

namespace Carcassone.Core.Tests.Calculation.Objects
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
            var gameRoom = new GameRoom();
            gameRoom.PlayersPool.AddPlayer("Jack", PlayerType.Human);
            gameRoom.PlayersPool.MoveToNextPlayer();

            var churchCard = gameRoom.CardsPool.GetCard(Card.GetCardId("FFFF", 0));
            gameRoom.PutCardInField(churchCard, gameRoom.FieldBoard.GetField(0, 0));
            gameRoom.PutChipInCard(churchCard.Parts[0], gameRoom.PlayersPool.GetCurrentPlayer().Name);
            gameRoom.EndTurn();
            Assert.Single(gameRoom.ScoreCalculator.Churches);
            Assert.Equal(1, gameRoom.GetPlayerScore(gameRoom.PlayersPool.GetCurrentPlayer()).ChurchesScore);

            var churchCard1 = gameRoom.CardsPool.GetCard(Card.GetCardId("RFRF", 0));
            churchCard1.RotateCard();
            gameRoom.PutCardInField(churchCard1, gameRoom.FieldBoard.GetField(0, 1)); // top center
            gameRoom.EndTurn();
            Assert.Single(gameRoom.ScoreCalculator.Churches);
            Assert.Equal(2, gameRoom.GetPlayerScore(gameRoom.PlayersPool.GetCurrentPlayer()).ChurchesScore);

            var churchCard2 = gameRoom.CardsPool.GetCard(Card.GetCardId("FFRR", 0));
            gameRoom.PutCardInField(churchCard2, gameRoom.FieldBoard.GetField(1, 1)); // top right
            gameRoom.EndTurn();
            Assert.Single(gameRoom.ScoreCalculator.Churches);
            Assert.Equal(3, gameRoom.GetPlayerScore(gameRoom.PlayersPool.GetCurrentPlayer()).ChurchesScore);

            var churchCard3 = gameRoom.CardsPool.GetCard(Card.GetCardId("FFRR", 1));
            churchCard3.RotateCard();
            churchCard3.RotateCard();
            churchCard3.RotateCard();
            gameRoom.PutCardInField(churchCard3, gameRoom.FieldBoard.GetField(-1, 1)); // top left
            gameRoom.EndTurn();
            Assert.Single(gameRoom.ScoreCalculator.Churches);
            Assert.Equal(4, gameRoom.GetPlayerScore(gameRoom.PlayersPool.GetCurrentPlayer()).ChurchesScore);

            var churchCard4 = gameRoom.CardsPool.GetCard(Card.GetCardId("RFRF", 1));
            gameRoom.PutCardInField(churchCard4, gameRoom.FieldBoard.GetField(-1, 0)); // mid left
            gameRoom.EndTurn();
            Assert.Single(gameRoom.ScoreCalculator.Churches);
            Assert.Equal(5, gameRoom.GetPlayerScore(gameRoom.PlayersPool.GetCurrentPlayer()).ChurchesScore);

            var churchCard5 = gameRoom.CardsPool.GetCard(Card.GetCardId("RFRF", 2));
            gameRoom.PutCardInField(churchCard5, gameRoom.FieldBoard.GetField(1, 0)); // mid right
            gameRoom.EndTurn();
            Assert.Single(gameRoom.ScoreCalculator.Churches);
            Assert.Equal(6, gameRoom.GetPlayerScore(gameRoom.PlayersPool.GetCurrentPlayer()).ChurchesScore);

            var churchCard6 = gameRoom.CardsPool.GetCard(Card.GetCardId("RFRF", 3));
            churchCard6.RotateCard();
            gameRoom.PutCardInField(churchCard6, gameRoom.FieldBoard.GetField(0, -1)); // bot center
            gameRoom.EndTurn();
            Assert.Single(gameRoom.ScoreCalculator.Churches);
            Assert.Equal(7, gameRoom.GetPlayerScore(gameRoom.PlayersPool.GetCurrentPlayer()).ChurchesScore);

            var churchCard7 = gameRoom.CardsPool.GetCard(Card.GetCardId("FFRR", 2));
            churchCard7.RotateCard();
            gameRoom.PutCardInField(churchCard7, gameRoom.FieldBoard.GetField(1, -1)); // bot right
            gameRoom.EndTurn();
            Assert.Single(gameRoom.ScoreCalculator.Churches);
            Assert.Equal(8, gameRoom.GetPlayerScore(gameRoom.PlayersPool.GetCurrentPlayer()).ChurchesScore);

            var churchCard8 = gameRoom.CardsPool.GetCard(Card.GetCardId("FFRR", 3));
            churchCard8.RotateCard();
            churchCard8.RotateCard();
            gameRoom.PutCardInField(churchCard8, gameRoom.FieldBoard.GetField(-1, -1)); // bot left
            gameRoom.EndTurn();
            Assert.Single(gameRoom.ScoreCalculator.Churches);
            Assert.Equal(18, gameRoom.GetPlayerScore(gameRoom.PlayersPool.GetCurrentPlayer()).ChurchesScore);

            Assert.True(gameRoom.ScoreCalculator.Churches[0].IsFinished);
        }
    }
}
