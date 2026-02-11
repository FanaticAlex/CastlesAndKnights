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
            Assert.Equal(0, center.Location.X);
            Assert.Equal(0, center.Location.Y);

            var tile = new CCCC("CCCC", 0);
            grid.PutTile(center, tile);
            Assert.Equal(5, grid.Cells.Count);
        }
    }
}
