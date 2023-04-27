using Carcassone.Core.Extensions.River.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Carcassone.Core.Tests.River
{
    public class RiverCardsTests
    {
        /// <summary>
        ///       F
        ///   |       |
        /// F |   +   | F
        ///   |   +   |
        ///       W
        ///       
        ///       F
        ///   |       |
        /// W |+++O+++| W   (поворот на 1)
        ///   |   |   |
        ///       R
        /// </summary>

        [Fact]
        public void RiverCardPut()
        {
            var game = new GameRoom();
            game.PlayersPool.AddHumanPlayer("bob");
            game.Start();

            var card = game.CardsPool.GetCard("FWRW(0)");
            card.RotateCard();
            game.PutCardInField(card, game.FieldBoard.GetField(0, -1));
            game.EndTurn();

            Assert.Equal(1, game.CardsPool.GetCard(card.Id).RotationsCount);
        }
    }
}
