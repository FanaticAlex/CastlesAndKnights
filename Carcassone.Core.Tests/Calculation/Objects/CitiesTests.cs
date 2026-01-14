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
                TileId = "FFWF(0)",
                TileRotation = 0,
                CellId = $"{0}_{0}",
                PartName = null
            };
            room.MakeMove(gameMove0);

            var gameMove1 = new GameMove()
            {
                PlayerName = name,
                TileId = "CWCW(0)",
                TileRotation = 1,
                CellId = $"{0}_{-1}",
                PartName = "City_0"
            };
            room.MakeMove(gameMove1);

            var Citys = room.ScoreCalculator.CitysManager.Objects;
            Assert.True(Citys.Count == 2);
            Assert.False(Citys[0].IsFinished);
            Assert.False(Citys[1].IsFinished);
            Assert.True(Citys[0].IsPlayerOwner(room.PlayersPool.GetPlayer(name), room.TileStack));
            
            var score = room.GetPlayerScore(room.PlayersPool.GetPlayer(name).Name);
            Assert.Equal(1, score.CitysScore);
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
                TileId = "CFFF(0)",
                TileRotation = 2,
                CellId = $"{0}_{0}",
                PartName = null
            };
            room.MakeMove(gameMove0);

            var gameMove1 = new GameMove()
            {
                PlayerName = name,
                TileId = "CFFF(1)",
                TileRotation = 0,
                CellId = $"{0}_{-1}",
                PartName = "City_0"
            };
            room.MakeMove(gameMove1);

            var Citys = room.ScoreCalculator.CitysManager.Objects;
            Assert.Single(Citys);
            var City = Citys.Single();
            Assert.Equal(4, City.GetPoints(room.TileStack));
            Assert.True(City.IsFinished);
            Assert.True(City.IsPlayerOwner(room.PlayersPool.GetPlayer(name), room.TileStack));
            Assert.Contains(City.PartsIds.Select(id => room.TileStack.GetPart(id)), p => p.Flag != null);

            var score = room.GetPlayerScore(room.PlayersPool.GetPlayer(name).Name);
            Assert.Equal(4, score.CitysScore);
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
                TileId = "FFWF(0)",
                TileRotation = 1,
                CellId = $"{0}_{0}",
                PartName = null
            };
            room.MakeMove(gameMove0);

            var gameMove1 = new GameMove()
            {
                PlayerName = name,
                TileId = "CWCW(0)",
                TileRotation = 0,
                CellId = $"{-1}_{0}",
                PartName = null
            };
            room.MakeMove(gameMove1);

            var gameMove2 = new GameMove()
            {
                PlayerName = name,
                TileId = "CFFF(0)",
                TileRotation = 2,
                CellId = $"{-1}_{1}",
                PartName = null
            };
            room.MakeMove(gameMove2);

            var gameMove3 = new GameMove()
            {
                PlayerName = name,
                TileId = "CFFF(1)",
                TileRotation = 0,
                CellId = $"{-1}_{-1}",
                PartName = "City_0"
            };
            room.MakeMove(gameMove3);

            var Citys = room.ScoreCalculator.CitysManager.Objects;
            Assert.Equal(2, Citys.Count);

            var City = Citys[1];
            Assert.Equal(4, City.GetPoints(room.TileStack));
            Assert.True(City.IsFinished);
            Assert.True(City.IsPlayerOwner(room.PlayersPool.GetPlayer(name), room.TileStack));
            Assert.Contains(City.PartsIds.Select(id => room.TileStack.GetPart(id)), p => p.Flag != null);

            var score = room.GetPlayerScore(room.PlayersPool.GetPlayer(name).Name);
            Assert.Equal(4, score.CitysScore);
            Assert.Equal(7, room.PlayersPool.GetPlayer(name).СhipList.Count);
        }
    }
}
