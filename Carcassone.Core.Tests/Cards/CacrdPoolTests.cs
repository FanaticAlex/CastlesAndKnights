using Carcassone.Core.Tiles;
using Carcassone.Core.Board;
using Carcassone.Core.Calculation.Base;
using System;
using Xunit;

namespace Carcassone.Core.Tests.Cards
{
    public class CardPoolTests
    {
        [Fact]
        public void GetCardTest()
        {
            var stack = new TileStack();
            var baseExt = new BaseRules();
            baseExt.AddTiles(stack);

            var cccc = stack.GetCard("CCCC(0)");
            Assert.NotNull(cccc);
        }
    }
}
