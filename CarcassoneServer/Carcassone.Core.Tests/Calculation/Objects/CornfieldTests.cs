using Carcassone.Core.Calculation.Objects;
using Carcassone.Core.Cards;
using Carcassone.Core.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Carcassone.Core.Tests.Calculation.Objects
{
    public class CornfieldTests
    {
        /// <summary>
        ///       F
        ///   |_______|
        /// R | _____ | R
        ///   |/     \|
        ///       C
        /// 
        ///       C
        ///   |\_____/|
        /// F |       | F
        ///   |       |
        ///       F
        /// </summary>
        [Fact]
        public void GetScore()
        {
            var room = new GameRoom();
            var owner1 = new Player();
            var chip = new Chip();
            chip.Owner = owner1;

            var card = room.GetCard("CFFF_0");
            var cornfield = new Cornfield();
            var part1 = card.Parts.First(p => p is CornfieldPart);
            part1.Chip = chip;
            cornfield.AddPart(part1);

            var castles = new List<Castle>();
            Assert.Equal(0, cornfield.GetPoints(castles, room));

            var closedCastle = new Castle();
            var castlePart = card.Parts.First(p => p is CastlePart);
            castlePart.Chip = chip;
            closedCastle.AddPart(castlePart);
            closedCastle.TryToClose();
            castles.Add(closedCastle);

            Assert.Equal(3, cornfield.GetPoints(castles, room));
        }
    }
}
