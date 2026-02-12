using Carcassone.Core.Board;
using Carcassone.Core.Calculation.Base.Tiles;
using Carcassone.Core.Tiles;
using System.Drawing;
using System.Linq;
using Xunit;

namespace Carcassone.Core.Tests.Board
{
    public class GameGridTests
    {
        [Fact]
        public void GetCenterTest()
        {
            var grid = new Grid();
            var tile = new CCCC("CCCC", 0);
            grid.PutTile(new Point(0, 0), tile);

            Assert.Equal(new Point(0, 0), tile.Location);
            Assert.Equal(tile, grid.GetTile(new Point(0, 0)));
            Assert.Equal(4, grid.GetEmptyCells().Count());
        }
    }
}
