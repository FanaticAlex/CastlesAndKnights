using Carcassone.Core.Calculation.Objects;
using Carcassone.Core.Cards;
using System.Collections.Generic;
using System.Linq;
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
            var player = room.PlayersPool.AddHumanPlayer("Jack");

            var card1 = room.GetCard("CRFR(0)");
            room.RotateCard(card1.Id);
            room.RotateCard(card1.Id);
            room.PutCardInField(card1, room.FieldBoard.GetField(0, 0));
            var cornfieldPart = card1.GetPart("Cornfield_0");
            room.PutChipInCard(cornfieldPart, "Jack");
            room.EndTurn();

            var card2 = room.GetCard("CFFF(0)");
            room.PutCardInField(card2, room.FieldBoard.GetField(0, -1));
            var castlePart = card2.GetPart("Castle_0");
            room.PutChipInCard(castlePart, "Jack");
            room.EndTurn();

            Assert.Single(room.ScoreCalculator.Castles);
            Assert.True(room.ScoreCalculator.Castles[0].IsFinished);

            var score = room.GetPlayerScore(player);
            Assert.Equal(4, score.CastlesScore);
            Assert.Equal(3, score.CornfieldsScore);

            Assert.Equal(3, room.ScoreCalculator.Cornfields.Count);
            Assert.True(room.ScoreCalculator.Cornfields[0].IsPlayerOwner(player, room.CardsPool));
            Assert.True(!room.ScoreCalculator.Cornfields[1].IsPlayerOwner(player, room.CardsPool));
            Assert.True(!room.ScoreCalculator.Cornfields[2].IsPlayerOwner(player, room.CardsPool));
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
            var owner1 = room.PlayersPool.AddHumanPlayer("owner1");

            var card1 = room.GetCard("RRWW(0)");
            room.PutCardInField(card1, room.FieldBoard.GetField(0, 0));
            room.EndTurn();

            var card2 = room.GetCard("WCCW(0)");
            room.PutCardInField(card2, room.FieldBoard.GetField(0, -1));
            room.PutChipInCard(card2.GetPart("Cornfield_0"), owner1.Name);
            room.EndTurn();

            var score = room.GetPlayerScore(owner1);
            Assert.Equal(0, score.CornfieldsScore);

            Assert.Single(room.ScoreCalculator.Castles);
            Assert.Single(room.ScoreCalculator.Roads);
            Assert.Equal(3, room.ScoreCalculator.Cornfields.Count);

            Assert.Equal(2, room.ScoreCalculator.Cornfields[0].OpenBorders.Count);
            Assert.Equal(6, room.ScoreCalculator.Cornfields[1].OpenBorders.Count);
            Assert.Equal(4, room.ScoreCalculator.Cornfields[2].OpenBorders.Count);
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
            var owner1 = room.PlayersPool.AddHumanPlayer("owner1");

            var card1 = room.GetCard("FWWF(0)");
            var field1 = room.FieldBoard.GetField("0_0");
            room.PutCardInField(card1, field1);

            var card2 = room.GetCard("WFWF(0)");
            var field2 = room.FieldBoard.GetField("0_-1");
            room.PutCardInField(card2, field2);

            Assert.Equal(2, room.ScoreCalculator.Cornfields.Count);

            Assert.Equal(5, room.ScoreCalculator.Cornfields[0].OpenBorders.Count);
            Assert.Equal(7, room.ScoreCalculator.Cornfields[1].OpenBorders.Count);
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
            var owner1 = room.PlayersPool.AddHumanPlayer("owner1");

            var card1 = room.GetCard("FFWF(0)");
            var field1 = room.FieldBoard.GetField("0_0");
            room.PutCardInField(card1, field1);

            var card2 = room.GetCard("FWRW(0)");
            card2.RotateCard();
            var field2 = room.FieldBoard.GetField("0_-1");
            room.PutCardInField(card2, field2);

            Assert.Equal(2, room.ScoreCalculator.Cornfields.Count);

            Assert.Equal(2, room.ScoreCalculator.Cornfields[0].OpenBorders.Count);
            Assert.Equal(10, room.ScoreCalculator.Cornfields[1].OpenBorders.Count);
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
        /// W |++++   | F   (rotated 2)
        ///   |       |
        ///       F
        /// </summary>
        [Fact]
        public void GetScore4()
        {
            var room = new GameRoom();
            var owner1 = room.PlayersPool.AddHumanPlayer("owner1");

            var card1 = room.GetCard("FFWF(0)");
            var field1 = room.FieldBoard.GetField("0_0");
            room.PutCardInField(card1, field1);

            var card2 = room.GetCard("FWWF(0)");
            card2.RotateCard();
            card2.RotateCard();
            var field2 = room.FieldBoard.GetField("0_-1");
            room.PutCardInField(card2, field2);

            Assert.Single(room.ScoreCalculator.Cornfields);

            Assert.Equal(11, room.ScoreCalculator.Cornfields[0].OpenBorders.Count);
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
        /// R |-----| | C
        ///   |   +  \|
        ///       W
        /// </summary>
        [Fact]
        public void GetScore5()
        {
            var room = new GameRoom();
            var owner1 = room.PlayersPool.AddHumanPlayer("owner1");

            var card1 = room.GetCard("RRWW(0)");
            var field1 = room.FieldBoard.GetField("0_0");
            room.PutCardInField(card1, field1);

            var part = card1.GetPart("Cornfield_1");
            room.PutChipInCard(part, owner1.Name);

            var card2 = room.GetCard("WCWR(0)");
            var field2 = room.FieldBoard.GetField("0_-1");
            room.PutCardInField(card2, field2);

            Assert.Equal(true, card2.GetPart("Cornfield_0").IsPartOfOwnedObject);

            Assert.Equal(5, room.ScoreCalculator.Cornfields.Count);

            Assert.Equal(2, room.ScoreCalculator.Cornfields[0].OpenBorders.Count);
            Assert.Equal(5, room.ScoreCalculator.Cornfields[1].OpenBorders.Count);
            Assert.Equal(1, room.ScoreCalculator.Cornfields[2].OpenBorders.Count);
            Assert.Equal(2, room.ScoreCalculator.Cornfields[3].OpenBorders.Count);
            Assert.Equal(4, room.ScoreCalculator.Cornfields[4].OpenBorders.Count);
        }
    }
}
