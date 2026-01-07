using Carcassone.Core.Tiles;
using Carcassone.Core.Extensions;
using Carcassone.Core.Board;
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
            var cardPool = new Stack(extensionsManager);
            var cccc = cardPool.GetCard("CCCC(0)");
            Assert.NotNull(cccc);
        }
    }
}
