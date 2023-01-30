using Carcassone.Core.Calculation.Objects;
using Carcassone.Core.Cards;
using Carcassone.Core.Fields;
using Carcassone.Core.Players;
using Xunit;

namespace Carcassone.Core.Tests.Calculation.Objects
{
    public class ChurchTests
    {
        [Fact]
        public void GetPointsTest()
        {
            var churchCard = GetNearCard(0, 0);
            var churchPart = new ChurchPart("partName", churchCard.CardName);
            churchPart.ChurchField = churchCard.Field;
            churchPart.Chip = new Chip();
            churchPart.Chip.Owner = new Player();

            var church = new Church(churchPart);
            Assert.Equal(1, church.GetPoints());

            church.TryAddCard(GetNearCard(0, 1)); // top center
            Assert.Equal(2, church.GetPoints());
            church.TryAddCard(GetNearCard(1, 1)); // top right
            Assert.Equal(3, church.GetPoints());
            church.TryAddCard(GetNearCard(-1, 1)); // top left
            Assert.Equal(4, church.GetPoints());
            church.TryAddCard(GetNearCard(-1, 0)); // mid left
            Assert.Equal(5, church.GetPoints());
            church.TryAddCard(GetNearCard(1, 0)); // mid right
            Assert.Equal(6, church.GetPoints());
            church.TryAddCard(GetNearCard(0, -1)); // bot center
            Assert.Equal(7, church.GetPoints());
            church.TryAddCard(GetNearCard(1, -1)); // bot right
            Assert.Equal(8, church.GetPoints());
            church.TryAddCard(GetNearCard(-1, -1)); // bot left
            Assert.Equal(18, church.GetPoints());

            church.TryToClose();

            Assert.True(church.IsFinished);
        }

        private Card GetNearCard(int x, int y)
        {
            var churchCard = new Card("FFFF_0");
            var field = new Field(null, x, y);
            churchCard.ConnectField(field);
            field.SetCardInField(churchCard);
            return churchCard;
        }
    }
}
