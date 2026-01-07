using Carcassone.Core.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Carcassone.Core.Tests.Cards
{
    public class CardTest
    {
        [Fact]
        public void RotateCardTest()
        {
            var card = new CCCC("CCCC", 0);
            card.RotateCard();
            Assert.Equal(1, card.RotationsCount);
        }
    }
}
