using Carcassone.Core.Calculation.Objects;
using Carcassone.Core.Cards;
using Carcassone.Core.Players;
using System.Linq;
using Xunit;

namespace Carcassone.Core.Tests.Calculation.Objects
{
    public class CastleTests
    {

        /// <summary>
        /// 
        ///       F
        ///   |       |
        /// F | _____ | F
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
        public void CalculationTest_DoubleCastle()
        {
            var room = new GameRoom();
            var bob = room.AddHumanPlayer("bob");

            var card0 = room.GetCard("CFFF_0");
            card0.RotationsCount = 2;
            var card1 = room.GetCard("CFFF_1");
            card1.RotationsCount = 0;

            room.PutCardInField(card0, room.GetField($"{0}_{0}"));
            room.EndTurn();

            room.PutCardInField(card1, room.GetField($"{0}_{-1}"));
            var part = card1.Parts.Single(p => p is CastlePart);
            room.PutChipInCard(part, bob);
            room.EndTurn();

            var castles = room.GetCastles();
            Assert.Single(castles);
            var castle = castles.Single();
            Assert.Equal(4, castle.GetPoints());
            Assert.True(castle.IsFinished);
            Assert.True(castle.IsPlayerOwner(bob));
            Assert.Contains(castle.Parts, p => p.Flag != null);

            var score = room.GetPlayerScore(bob);
            Assert.Equal(4, score.Castles);
            Assert.Equal(7, bob.ChipCount);
        }


        /// <summary>
        /// 
        ///       F
        ///   |       |
        /// F | _____ | F
        ///   |/     \|
        ///       C
        ///       
        ///       C
        ///   |\-----/|
        /// W |+++++++| W
        ///   |/-----\|
        ///       C
        /// 
        ///       C
        ///   |\_____/|
        /// F |       | F
        ///   |       |
        ///       F
        /// </summary>
        [Fact]
        public void CalculationTest_TwoRiverCastles()
        {
            var room = new GameRoom();
            var bob = room.AddHumanPlayer("bob");

            var card0 = room.GetCard("CFFF_0");
            card0.RotationsCount = 2;
            var card1 = room.GetCard("CWCW_0");
            card1.RotationsCount = 0;
            var card2 = room.GetCard("CFFF_1");
            card2.RotationsCount = 0;


            room.PutCardInField(card1, room.GetField($"{0}_{0}"));
            room.EndTurn();

            room.PutCardInField(card0, room.GetField($"{0}_{1}"));
            room.EndTurn();

            room.PutCardInField(card2, room.GetField($"{0}_{-1}"));
            var part = card2.Parts.Single(p => p is CastlePart);
            room.PutChipInCard(part, bob);
            room.EndTurn();

            var castles = room.GetCastles();
            Assert.Equal(2, castles.Count);

            var castle = castles[1];
            Assert.Equal(4, castle.GetPoints());
            Assert.True(castle.IsFinished);
            Assert.True(castle.IsPlayerOwner(bob));
            Assert.Contains(castle.Parts, p => p.Flag != null);

            var score = room.GetPlayerScore(bob);
            Assert.Equal(4, score.Castles);
            Assert.Equal(7, bob.ChipCount);
        }
    }
}
