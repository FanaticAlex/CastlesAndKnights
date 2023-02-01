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

        /// <summary>
        ///       R
        ///   |    \  |
        /// W |+++  \_| R
        ///   |   +   |
        ///       W
        /// 
        ///       W
        ///   |   +  /|
        /// W |++ /   | C
        ///   |/      |
        ///       C
        /// </summary>
        [Fact]
        public void GetScore1()
        {
            var room = new GameRoom();
            var owner1 = room.AddHumanPlayer("owner1");

            var card1 = room.GetCard("RRWW_0");
            var field1 = room.GetField("0_0");
            room.PutCardInField(card1, field1);

            var card2 = room.GetCard("WCCW_0");
            var field2 = room.GetField("0_-1");
            room.PutCardInField(card2, field2);

            Assert.Single(room.GetCastles());
            Assert.Single(room.GetRoads());
            Assert.Equal(3, room.GetCornfields().Count);

            Assert.Equal(2, room.GetCornfields()[0].OpenBorders.Count);
            Assert.Equal(6, room.GetCornfields()[1].OpenBorders.Count);
            Assert.Equal(4, room.GetCornfields()[2].OpenBorders.Count);
        }

        /// <summary>
        ///       F
        ///   |       |
        /// F |   ++++| W
        ///   |   +   |
        ///       W
        /// 
        ///       W
        ///   |   +   |
        /// F |   +   | F
        ///   |   +   |
        ///       W
        /// </summary>
        [Fact]
        public void GetScore2()
        {
            var room = new GameRoom();
            var owner1 = room.AddHumanPlayer("owner1");

            var card1 = room.GetCard("FWWF_0");
            var field1 = room.GetField("0_0");
            room.PutCardInField(card1, field1);

            var card2 = room.GetCard("WFWF_0");
            var field2 = room.GetField("0_-1");
            room.PutCardInField(card2, field2);

            Assert.Equal(2, room.GetCornfields().Count);

            Assert.Equal(5, room.GetCornfields()[0].OpenBorders.Count);
            Assert.Equal(7, room.GetCornfields()[1].OpenBorders.Count);
        }

        /// <summary>
        ///       F
        ///   |       |
        /// F |   +   | F
        ///   |   +   |
        ///       W
        /// 
        ///       W
        ///   |   +   |
        /// R |---O   | F  (rotated)
        ///   |   +   |
        ///       W
        /// </summary>
        [Fact]
        public void GetScore3()
        {
            var room = new GameRoom();
            var owner1 = room.AddHumanPlayer("owner1");

            var card1 = room.GetCard("FFWF_0");
            var field1 = room.GetField("0_0");
            room.PutCardInField(card1, field1);

            var card2 = room.GetCard("FWRW_0");
            card2.RotateCard();
            var field2 = room.GetField("0_-1");
            room.PutCardInField(card2, field2);

            Assert.Equal(2, room.GetCornfields().Count);

            Assert.Equal(2, room.GetCornfields()[0].OpenBorders.Count);
            Assert.Equal(10, room.GetCornfields()[1].OpenBorders.Count);
        }
    }
}
