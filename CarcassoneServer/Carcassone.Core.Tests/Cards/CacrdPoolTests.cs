using Carcassone.Core.Cards;
using Carcassone.Core.Extensions;
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
            var extensionsManager = new ExtensionsManager();
            var cardPool = new CardPool(extensionsManager);
            var cccc = cardPool.GetCard("CCCC_0");
            Assert.NotNull(cccc);
        }

        [Fact]
        public void GetCurrentCardTest()
        {
            var extensionsManager = new ExtensionsManager();
            var cardPool = new CardPool(extensionsManager);
            var fialdBoard = new FieldBoard();
            var card = cardPool.GetCurrentCard(fialdBoard);
            Assert.NotNull(card);
        }

        [Fact]
        public void GetCardsRemainInPoolTest()
        {
            var extensionsManager = new ExtensionsManager();
            var cardPool = new CardPool(extensionsManager);
            var card = cardPool.GetCardsRemainInPool();
            var allCards = cardPool.GetAllCards();
            Assert.Equal(card.Count, allCards.Count);
        }
    }
}
