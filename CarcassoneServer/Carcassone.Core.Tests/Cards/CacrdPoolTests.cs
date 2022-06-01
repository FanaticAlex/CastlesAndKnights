using Carcassone.Core.Cards;
using Carcassone.Core.Fields;
using System;
using Xunit;

namespace Carcassone.Core.Tests.Cards
{
    public class CardPoolTests
    {
        [Fact]
        public void GetCardTest()
        {
            var cardPool = new CardPool();
            var cccc = cardPool.GetCard("CCCC_0");
            Assert.NotNull(cccc);
        }

        [Fact]
        public void GetCurrentCardTest()
        {
            var cardPool = new CardPool();
            var fialdBoard = new FieldBoard();
            var card = cardPool.GetCurrentCard(fialdBoard);
            Assert.NotNull(card);
        }

        [Fact]
        public void GetCardsRemainInPoolTest()
        {
            var cardPool = new CardPool();
            var card = cardPool.GetCardsRemainInPool();
            var allCards = cardPool.GetAllCards();
            Assert.Equal(card.Count, allCards.Count);
        }
    }
}
