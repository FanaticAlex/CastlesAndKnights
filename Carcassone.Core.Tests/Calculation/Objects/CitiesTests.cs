using Carcassone.Core.Tiles;
using Carcassone.Core.Players;
using System.Linq;
using Xunit;

namespace Carcassone.Core.Tests.Buisness
{
    public class CitiesTests
    {

        /// <summary>
        ///       F
        ///   |       |
        /// F |   +   | F
        ///   |   +   |
        ///       W
        /// 
        ///       W
        ///   |\  +  /|
        /// C | | + | | C
        ///   |/  +  \|
        ///       W
        /// </summary>
        [Fact]
        public void CalculationTest_NotClosedCity()
        {
            var room = new GameRoom();
            var name = "bob";
            room.PlayersPool.AddPlayer(name, PlayerType.Human);

            var gameMove0 = new GameMove()
            {
                PlayerName = name,
                CardId = "FFWF(0)",
                CardRotation = 0,
                FieldId = $"{0}_{0}",
                PartName = null
            };
            room.MakeMove(gameMove0);

            var gameMove1 = new GameMove()
            {
                PlayerName = name,
                CardId = "CWCW(0)",
                CardRotation = 1,
                FieldId = $"{0}_{-1}",
                PartName = "Castle_0"
            };
            room.MakeMove(gameMove1);

            var castles = room.ScoreCalculator.CastlesManager.Objects;
            Assert.True(castles.Count == 2);
            Assert.False(castles[0].IsFinished);
            Assert.False(castles[1].IsFinished);
            Assert.True(castles[0].IsPlayerOwner(room.PlayersPool.GetPlayer(name), room.CardsPool));
            
            var score = room.GetPlayerScore(room.PlayersPool.GetPlayer(name).Name);
            Assert.Equal(1, score.CastlesScore);
            Assert.Equal(6, room.PlayersPool.GetPlayer(name).СhipList.Count);
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
        ///   |\_____/|
        /// F |       | F
        ///   |       |
        ///       F
        /// </summary>
        [Fact]
        public void CalculationTest_DoubleCity()
        {
            var room = new GameRoom();
            var name = "bob";
            room.PlayersPool.AddPlayer(name, PlayerType.Human);

            var gameMove0 = new GameMove()
            {
                PlayerName = name,
                CardId = "CFFF(0)",
                CardRotation = 2,
                FieldId = $"{0}_{0}",
                PartName = null
            };
            room.MakeMove(gameMove0);

            var gameMove1 = new GameMove()
            {
                PlayerName = name,
                CardId = "CFFF(1)",
                CardRotation = 0,
                FieldId = $"{0}_{-1}",
                PartName = "Castle_0"
            };
            room.MakeMove(gameMove1);

            var castles = room.ScoreCalculator.CastlesManager.Objects;
            Assert.Single(castles);
            var castle = castles.Single();
            Assert.Equal(4, castle.GetPoints(room.CardsPool));
            Assert.True(castle.IsFinished);
            Assert.True(castle.IsPlayerOwner(room.PlayersPool.GetPlayer(name), room.CardsPool));
            Assert.Contains(castle.PartsIds.Select(id => room.CardsPool.GetPart(id)), p => p.Flag != null);

            var score = room.GetPlayerScore(room.PlayersPool.GetPlayer(name).Name);
            Assert.Equal(4, score.CastlesScore);
            Assert.Equal(7, room.PlayersPool.GetPlayer(name).СhipList.Count);
        }


        /// <summary>
        /// 
        ///       F
        ///   |       |
        /// F | _____ | F
        ///   |/     \|
        ///       C
        ///       
        ///       C               F
        ///   |\-----/|       |       |   
        /// W |+++++++| W   W |++++   | F
        ///   |/-----\|       |       |
        ///       C               F 
        /// 
        ///       C
        ///   |\_____/|
        /// F |       | F
        ///   |       |
        ///       F
        /// </summary>
        [Fact]
        public void CalculationTest_TwoRiverCities()
        {
            var room = new GameRoom();
            var name = "bob";
            room.PlayersPool.AddPlayer(name, PlayerType.Human);

            var gameMove0 = new GameMove()
            {
                PlayerName = name,
                CardId = "FFWF(0)",
                CardRotation = 1,
                FieldId = $"{0}_{0}",
                PartName = null
            };
            room.MakeMove(gameMove0);

            var gameMove1 = new GameMove()
            {
                PlayerName = name,
                CardId = "CWCW(0)",
                CardRotation = 0,
                FieldId = $"{-1}_{0}",
                PartName = null
            };
            room.MakeMove(gameMove1);

            var gameMove2 = new GameMove()
            {
                PlayerName = name,
                CardId = "CFFF(0)",
                CardRotation = 2,
                FieldId = $"{-1}_{1}",
                PartName = null
            };
            room.MakeMove(gameMove2);

            var gameMove3 = new GameMove()
            {
                PlayerName = name,
                CardId = "CFFF(1)",
                CardRotation = 0,
                FieldId = $"{-1}_{-1}",
                PartName = "Castle_0"
            };
            room.MakeMove(gameMove3);

            var castles = room.ScoreCalculator.CastlesManager.Objects;
            Assert.Equal(2, castles.Count);

            var castle = castles[1];
            Assert.Equal(4, castle.GetPoints(room.CardsPool));
            Assert.True(castle.IsFinished);
            Assert.True(castle.IsPlayerOwner(room.PlayersPool.GetPlayer(name), room.CardsPool));
            Assert.Contains(castle.PartsIds.Select(id => room.CardsPool.GetPart(id)), p => p.Flag != null);

            var score = room.GetPlayerScore(room.PlayersPool.GetPlayer(name).Name);
            Assert.Equal(4, score.CastlesScore);
            Assert.Equal(7, room.PlayersPool.GetPlayer(name).СhipList.Count);
        }
    }
}
