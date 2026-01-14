using Carcassone.Core.Players;
using Xunit;

namespace Carcassone.Core.Tests.Buisness
{
    public class FarmsTests
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
                TileId = "CRFR(0)",
                TileRotation = 2,
                CellId = $"{0}_{0}",
                PartName = "Farm_0"
            };
            room.MakeMove(gameMove1);

            var gameMove2 = new GameMove()
            {
                PlayerName = name,
                TileId = "CFFF(0)",
                TileRotation = 0,
                CellId = $"{0}_{-1}",
                PartName = "City_0"
            };
            room.MakeMove(gameMove2);

            Assert.Single(room.ScoreCalculator.CitysManager.Objects);
            Assert.True(room.ScoreCalculator.CitysManager.Objects[0].IsFinished);

            var score = room.GetPlayerScore(gamePlayer.Name);
            Assert.Equal(4, score.CitysScore);
            Assert.Equal(3, score.FarmsScore);

            Assert.Equal(3, room.ScoreCalculator.FarmsManager.Objects.Count);
            Assert.True(room.ScoreCalculator.FarmsManager.Objects[0].IsPlayerOwner(gamePlayer, room.TileStack));
            Assert.True(!room.ScoreCalculator.FarmsManager.Objects[1].IsPlayerOwner(gamePlayer, room.TileStack));
            Assert.True(!room.ScoreCalculator.FarmsManager.Objects[2].IsPlayerOwner(gamePlayer, room.TileStack));
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
                TileId = "RRWW(0)",
                TileRotation = 0,
                CellId = $"{0}_{0}",
                PartName = null
            };
            room.MakeMove(gameMove1);

            var gameMove2 = new GameMove()
            {
                PlayerName = name,
                TileId = "WCCW(0)",
                TileRotation = 0,
                CellId = $"{0}_{-1}",
                PartName = "Farm_0"
            };
            room.MakeMove(gameMove2);

            var score = room.GetPlayerScore(owner1.Name);
            Assert.Equal(0, score.FarmsScore);

            Assert.Single(room.ScoreCalculator.CitysManager.Objects);
            Assert.Single(room.ScoreCalculator.RoadsManager.Objects);
            Assert.Equal(3, room.ScoreCalculator.FarmsManager.Objects.Count);

            Assert.Equal(2, room.ScoreCalculator.FarmsManager.Objects[0].OpenBorders.Count);
            Assert.Equal(6, room.ScoreCalculator.FarmsManager.Objects[1].OpenBorders.Count);
            Assert.Equal(4, room.ScoreCalculator.FarmsManager.Objects[2].OpenBorders.Count);
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
                TileId = "FFWF(0)",
                TileRotation = 1,
                CellId = $"{0}_{0}",
                PartName = null
            };
            room.MakeMove(gameMove0);

            var gameMove1 = new GameMove()
            {
                PlayerName = owner1.Name,
                TileId = "FWWF(0)",
                TileRotation = 0,
                CellId = $"{-1}_{0}",
                PartName = null
            };
            room.MakeMove(gameMove1);

            var gameMove2 = new GameMove()
            {
                PlayerName = owner1.Name,
                TileId = "WFWF(0)",
                TileRotation = 0,
                CellId = $"{-1}_{-1}",
                PartName = null
            };
            room.MakeMove(gameMove2);

            Assert.Equal(1, room.ScoreCalculator.FarmsManager.Objects.Count);
            Assert.Equal(17, room.ScoreCalculator.FarmsManager.Objects[0].OpenBorders.Count);
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
                TileId = "FFWF(0)",
                TileRotation = 0,
                CellId = $"{0}_{0}",
                PartName = null
            };
            room.MakeMove(gameMove1);


            var gameMove2 = new GameMove()
            {
                PlayerName = owner1.Name,
                TileId = "FWRW(0)",
                TileRotation = 1,
                CellId = $"{0}_{-1}",
                PartName = null
            };
            room.MakeMove(gameMove2);

            Assert.Equal(2, room.ScoreCalculator.FarmsManager.Objects.Count);

            Assert.Equal(2, room.ScoreCalculator.FarmsManager.Objects[0].OpenBorders.Count);
            Assert.Equal(10, room.ScoreCalculator.FarmsManager.Objects[1].OpenBorders.Count);
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
                TileId = "FFWF(0)",
                TileRotation = 0,
                CellId = $"{0}_{0}",
                PartName = null
            };
            room.MakeMove(gameMove1);

            var gameMove2 = new GameMove()
            {
                PlayerName = owner1.Name,
                TileId = "FWWF(0)",
                TileRotation = 2,
                CellId = $"{0}_{-1}",
                PartName = null
            };
            room.MakeMove(gameMove2);

            Assert.Single(room.ScoreCalculator.FarmsManager.Objects);

            Assert.Equal(11, room.ScoreCalculator.FarmsManager.Objects[0].OpenBorders.Count);
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
                TileId = "RRWW(0)",
                TileRotation = 0,
                CellId = $"{0}_{0}",
                PartName = "Farm_1"
            };
            room.MakeMove(gameMove1);

            var gameMove2 = new GameMove()
            {
                PlayerName = owner1.Name,
                TileId = "WCWR(0)",
                TileRotation = 0,
                CellId = $"{0}_{-1}",
                PartName = null
            };
            room.MakeMove(gameMove2);

            var card2 = room.GetTile("WCWR(0)");
            Assert.Equal(true, card2.GetPart("Farm_0").IsPartOfOwnedObject);

            Assert.Equal(5, room.ScoreCalculator.FarmsManager.Objects.Count);

            Assert.Equal(2, room.ScoreCalculator.FarmsManager.Objects[0].OpenBorders.Count);
            Assert.Equal(5, room.ScoreCalculator.FarmsManager.Objects[1].OpenBorders.Count);
            Assert.Equal(1, room.ScoreCalculator.FarmsManager.Objects[2].OpenBorders.Count);
            Assert.Equal(2, room.ScoreCalculator.FarmsManager.Objects[3].OpenBorders.Count);
            Assert.Equal(4, room.ScoreCalculator.FarmsManager.Objects[4].OpenBorders.Count);
        }
    }
}
