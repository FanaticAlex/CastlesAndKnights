using Carcassone.Core.Calculation.Base.Cities;
using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.Base.Tiles;
using Carcassone.Core.Tiles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Xunit;

namespace Carcassone.Core.Tests.Board
{
    public class GameStateTests
    {
        [Fact]
        public void GetCenterTest()
        {
            var room = TestHelper.GetDefaultGame(TestHelper.Bob);
            room.ForTests_ResetTilesStack();
            room.ForTests_CustomTile(typeof(CCCC), 2, 0);

            var gameMove0 = new GameMove
            (
                playerName: TestHelper.Bob,
                tileId: "CCCC(0)",
                tileRotation: 0,
                tileLocation: new Point(0, 0),
                partName: null
            );
            room.MakeMove(gameMove0);

            var move = room.State.GetMove(new Point(0, 0));
            Assert.Equal(new Point(0, 0), room.State.GetTileLocation(gameMove0.TileId));
            Assert.Equal(gameMove0.TileId, move?.TileId);
            Assert.Equal(4, room.State.GetEmptyCells().Count());
        }

        /// <summary>
        /// 
        ///                    F
        ///                |       |
        ///              F | _____ | F
        ///                |/     \|
        ///                    C
        /// 
        ///       C
        ///   |\_____/|
        /// F |       | F
        ///   |       |
        ///       F
        /// </summary>
        [Fact]
        public void MisplaceTile()
        {
            var room = TestHelper.GetDefaultGame(TestHelper.Bob);
            room.ForTests_ResetTilesStack();
            room.ForTests_CustomTile(typeof(CFFF), 2, 0);

            var gameMove0 = new GameMove
            (
                playerName: TestHelper.Bob,
                tileId: "CFFF(0)",
                tileRotation: 2,
                tileLocation: new Point(0, 0),
                partName: null
            );
            room.MakeMove(gameMove0);

            var gameMove1 = new GameMove
            (
                playerName: TestHelper.Bob,
                tileId: "CFFF(1)",
                tileRotation: 0,
                tileLocation: new Point(1, 1),
                partName: "City_0"
            );
            Assert.Throws<Exception>(() => room.MakeMove(gameMove1));
        }

        /// <summary>
        ///       F                 F
        ///   |    __/|        |\      |
        /// F | __/   | C    C | |   --| R
        ///   |/      |        |/   /  |
        ///       C                 R
        /// 
        ///       C                 R
        ///   |\_____/|        |   |   |
        /// F |       | F    F |   |   | F
        ///   |       |        |   |   |
        ///       F                 R
        ///       
        ///       F
        ///   |        |
        /// F |        | F
        ///   |        |
        ///       F
        /// </summary>
        [Fact]
        public void CalculationTest_MultipleOwners()
        {
            var room = TestHelper.GetDefaultGame(TestHelper.Bob, TestHelper.Jack);
            room.ForTests_ResetTilesStack();
            room.ForTests_CustomTile(typeof(CFFF), 1, 0);
            room.ForTests_CustomTile(typeof(RFRF), 1, 0);
            room.ForTests_CustomTile(typeof(FFFF), 1, 0);
            room.ForTests_CustomTile(typeof(CFRR), 1, 0);
            room.ForTests_CustomTile(typeof(CFFC), 1, 0);

            var gameMove0 = new GameMove
            (
                playerName: TestHelper.Bob,
                tileId: "CFFF(0)",
                tileRotation: 0,
                tileLocation: new Point(0, 0),
                partName: "City_0"
            );
            room.MakeMove(gameMove0);

            var gameMove1 = new GameMove
            (
                playerName: TestHelper.Jack,
                tileId: "RFRF(0)",
                tileRotation: 0,
                tileLocation: new Point(1, 0),
                partName: null
            );
            room.MakeMove(gameMove1);

            var gameMove2 = new GameMove
            (
                playerName: TestHelper.Bob,
                tileId: "FFFF(0)",
                tileRotation: 0,
                tileLocation: new Point(0, -1),
                partName: null
            );
            room.MakeMove(gameMove2);

            var gameMove3 = new GameMove
            (
                playerName: TestHelper.Jack,
                tileId: "CFRR(0)",
                tileRotation: 3,
                tileLocation: new Point(1, 1),
                partName: "City_0"
            );
            room.MakeMove(gameMove3);

            var gameMove4 = new GameMove
            (
                playerName: TestHelper.Bob,
                tileId: "CFFC(0)",
                tileRotation: 2,
                tileLocation: new Point(0, 1),
                partName: "Farm_0"
            );
            room.MakeMove(gameMove4);


            var cities = room.State.Calculated.OfType<City>();
            Assert.Single(cities);
            var city = cities.First();
            Assert.True(city.IsComplete(room.State));

            var scores = room.PlayersPool.GetPlayersScores(room.State).ToList();
            Assert.Equal(TestHelper.Bob, scores[0].PlayerName);
            Assert.Equal(6, scores[0].MeeplesCount);
            Assert.Equal(9, scores[0].OverallScore);
            Assert.Equal(0, scores[0].Rank);

            Assert.Equal(TestHelper.Jack, scores[1].PlayerName);
            Assert.Equal(7, scores[1].MeeplesCount);
            Assert.Equal(6, scores[1].OverallScore);
            Assert.Equal(1, scores[1].Rank);
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
        ///   |\     /|     
        /// F | |   | | F  
        ///   |/     \|      
        ///       C           
        ///
        /// </summary>
        [Fact]
        public void CalculationTest_DisablePutMeepleAtOwnedObject()
        {
            var room = TestHelper.GetDefaultGame(TestHelper.Bob, TestHelper.Jack);
            room.ForTests_ResetTilesStack();
            room.ForTests_CustomTile(typeof(FCFC1), 1, 0);
            room.ForTests_CustomTile(typeof(CFFF), 1, 0);

            var gameMove0 = new GameMove
            (
                playerName: TestHelper.Bob,
                tileId: "FCFC1(0)",
                tileRotation: 1,
                tileLocation: new Point(0, 0),
                partName: "City_0"
            );
            room.MakeMove(gameMove0);

            var gameMove2 = new GameMove
            (
                playerName: TestHelper.Jack,
                tileId: "CFFF(0)",
                tileRotation: 2,
                tileLocation: new Point(0, 1),
                partName: "City_0"
            );
            Assert.Throws<Exception>(() => room.MakeMove(gameMove2));
        }
    }
}
