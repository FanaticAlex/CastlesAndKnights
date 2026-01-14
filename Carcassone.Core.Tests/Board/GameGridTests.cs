using Carcassone.Core.Board;
using Carcassone.Core.Calculation.Base.Tiles;
using Carcassone.Core.Tiles;
using Xunit;

namespace Carcassone.Core.Tests.Board
{
    public class GameGridTests
    {
        [Fact]
        public void GetCenterTest()
        {
            var grid = new Grid();
            var center = grid.GetCell(0, 0);
            Assert.Single(grid.Cells);
            Assert.NotNull(center);
            Assert.Equal(0, center.X);
            Assert.Equal(0, center.Y);

            var card = new CCCC("CCCC", 0);
            grid.PutTile(card, center);
            Assert.Equal(5, grid.Cells.Count);
        }
    }
}
