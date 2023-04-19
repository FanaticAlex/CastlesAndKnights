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
            var extensionsManager = new ExtensionsManager(true);
            var cardPool = new CardPool(extensionsManager);
            var cccc = cardPool.GetCard("CCCC(0)");
            Assert.NotNull(cccc);
        }
    }
}
