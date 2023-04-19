using Carcassone.Core.Fields;
using Carcassone.Core.Cards;
using Xunit;

namespace Carcassone.Core.Tests.Fields
{
    public class FieldBoardTests
    {
        [Fact]
        public void GetCenterTest()
        {
            var fieldBoard = new FieldBoard();
            var center = fieldBoard.GetCenter();
            Assert.Single(fieldBoard.Fields);
            Assert.NotNull(center);
            Assert.Equal(0, center.X);
            Assert.Equal(0, center.Y);

            var card = new CCCC("CCCC", 0);
            fieldBoard.PutCard(card, center);
            Assert.Equal(5, fieldBoard.Fields.Count);
        }
    }
}
