using Carcassone.Core.Tiles;
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
            var baseExt = new BaseRules(null);
            baseExt.AddTiles(stack);

            var cccc = stack.GetTile("CCCC(0)");
            Assert.NotNull(cccc);
        }
    }
}
