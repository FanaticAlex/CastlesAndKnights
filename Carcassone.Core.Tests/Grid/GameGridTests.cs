using Carcassone.Core.Board;
using Carcassone.Core.Calculation.Base.Tiles;
using Carcassone.Core.Tiles;
using Xunit;

namespace Carcassone.Core.Tests.Fields
{
    public class GameGridTests
    {
        [Fact]
        public void GetCenterTest()
        {
            var grid = new Grid();
            var center = grid.GetField(0, 0);
            Assert.Single(grid.Cells);
            Assert.NotNull(center);
            Assert.Equal(0, center.X);
            Assert.Equal(0, center.Y);

            var card = new CCCC("CCCC", 0);
            grid.PutCard(card, center);
            Assert.Equal(5, grid.Cells.Count);
        }
    }
}
