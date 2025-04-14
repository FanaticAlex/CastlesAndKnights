using Carcassone.Core.Players;
using Xunit;

namespace Carcassone.Core.Tests.Buisness
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
            var name = "Jack";
            var gamePlayer = room.PlayersPool.AddPlayer(name, PlayerType.Human);

            var gameMove1 = new GameMove()
            {
                PlayerName = name,
                CardId = "CRFR(0)",
                CardRotation = 2,
                FieldId = $"{0}_{0}",
                PartName = "Cornfield_0"
            };
            room.MakeMove(gameMove1);

            var gameMove2 = new GameMove()
            {
                PlayerName = name,
                CardId = "CFFF(0)",
                CardRotation = 0,
                FieldId = $"{0}_{-1}",
                PartName = "Castle_0"
            };
            room.MakeMove(gameMove2);

            Assert.Single(room.ScoreCalculator.Castles);
            Assert.True(room.ScoreCalculator.Castles[0].IsFinished);

            var score = room.GetPlayerScore(gamePlayer.Name);
            Assert.Equal(4, score.CastlesScore);
            Assert.Equal(3, score.CornfieldsScore);

            Assert.Equal(3, room.ScoreCalculator.Cornfields.Count);
            Assert.True(room.ScoreCalculator.Cornfields[0].IsPlayerOwner(gamePlayer, room.CardsPool));
            Assert.True(!room.ScoreCalculator.Cornfields[1].IsPlayerOwner(gamePlayer, room.CardsPool));
            Assert.True(!room.ScoreCalculator.Cornfields[2].IsPlayerOwner(gamePlayer, room.CardsPool));
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
            var name = "owner1";
            var owner1 = room.PlayersPool.AddPlayer(name, PlayerType.Human);

            var gameMove1 = new GameMove()
            {
                PlayerName = name,
                CardId = "RRWW(0)",
                CardRotation = 0,
                FieldId = $"{0}_{0}",
                PartName = null
            };
            room.MakeMove(gameMove1);

            var gameMove2 = new GameMove()
            {
                PlayerName = name,
                CardId = "WCCW(0)",
                CardRotation = 0,
                FieldId = $"{0}_{-1}",
                PartName = "Cornfield_0"
            };
            room.MakeMove(gameMove2);

            var score = room.GetPlayerScore(owner1.Name);
            Assert.Equal(0, score.CornfieldsScore);

            Assert.Single(room.ScoreCalculator.Castles);
            Assert.Single(room.ScoreCalculator.Roads);
            Assert.Equal(3, room.ScoreCalculator.Cornfields.Count);

            Assert.Equal(2, room.ScoreCalculator.Cornfields[0].OpenBorders.Count);
            Assert.Equal(6, room.ScoreCalculator.Cornfields[1].OpenBorders.Count);
            Assert.Equal(4, room.ScoreCalculator.Cornfields[2].OpenBorders.Count);
        }

        /// <summary>
        ///       F                F
        ///   |       |        |       |
        /// F |   ++++| W    W |++++   | F
        ///   |   +   |        |       |
        ///       W                F
        ///       
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
            var name = "owner1";
            var owner1 = room.PlayersPool.AddPlayer(name, PlayerType.Human);
            
            var gameMove0 = new GameMove()
            {
                PlayerName = owner1.Name,
                CardId = "FFWF(0)",
                CardRotation = 1,
                FieldId = $"{0}_{0}",
                PartName = null
            };
            room.MakeMove(gameMove0);

            var gameMove1 = new GameMove()
            {
                PlayerName = owner1.Name,
                CardId = "FWWF(0)",
                CardRotation = 0,
                FieldId = $"{-1}_{0}",
                PartName = null
            };
            room.MakeMove(gameMove1);

            var gameMove2 = new GameMove()
            {
                PlayerName = owner1.Name,
                CardId = "WFWF(0)",
                CardRotation = 0,
                FieldId = $"{-1}_{-1}",
                PartName = null
            };
            room.MakeMove(gameMove2);

            Assert.Equal(1, room.ScoreCalculator.Cornfields.Count);
            Assert.Equal(17, room.ScoreCalculator.Cornfields[0].OpenBorders.Count);
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
            var name = "owner1";
            var owner1 = room.PlayersPool.AddPlayer(name, PlayerType.Human);

            var gameMove1 = new GameMove()
            {
                PlayerName = owner1.Name,
                CardId = "FFWF(0)",
                CardRotation = 0,
                FieldId = $"{0}_{0}",
                PartName = null
            };
            room.MakeMove(gameMove1);


            var gameMove2 = new GameMove()
            {
                PlayerName = owner1.Name,
                CardId = "FWRW(0)",
                CardRotation = 1,
                FieldId = $"{0}_{-1}",
                PartName = null
            };
            room.MakeMove(gameMove2);

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
            var name = "owner1";
            var owner1 = room.PlayersPool.AddPlayer(name, PlayerType.Human);

            var gameMove1 = new GameMove()
            {
                PlayerName = owner1.Name,
                CardId = "FFWF(0)",
                CardRotation = 0,
                FieldId = $"{0}_{0}",
                PartName = null
            };
            room.MakeMove(gameMove1);

            var gameMove2 = new GameMove()
            {
                PlayerName = owner1.Name,
                CardId = "FWWF(0)",
                CardRotation = 2,
                FieldId = $"{0}_{-1}",
                PartName = null
            };
            room.MakeMove(gameMove2);

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
            var name = "owner1";
            var owner1 = room.PlayersPool.AddPlayer(name, PlayerType.Human);

            var gameMove1 = new GameMove()
            {
                PlayerName = owner1.Name,
                CardId = "RRWW(0)",
                CardRotation = 0,
                FieldId = $"{0}_{0}",
                PartName = "Cornfield_1"
            };
            room.MakeMove(gameMove1);

            var gameMove2 = new GameMove()
            {
                PlayerName = owner1.Name,
                CardId = "WCWR(0)",
                CardRotation = 0,
                FieldId = $"{0}_{-1}",
                PartName = null
            };
            room.MakeMove(gameMove2);

            var card2 = room.GetCard("WCWR(0)");
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
