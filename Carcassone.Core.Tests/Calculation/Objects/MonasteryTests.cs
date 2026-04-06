using Carcassone.Core.Calculation.Base.Farms;
using Carcassone.Core.Calculation.Base.Monasteries;
using Carcassone.Core.Calculation.Base.Roads;
using Carcassone.Core.Calculation.Base.Tiles;
using Carcassone.Core.Players;
using Carcassone.Core.Tiles;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using Xunit;

namespace Carcassone.Core.Tests.Buisness
{
    public class MonasteryTests
    {
        /// <summary>
        /// 9 полей с картой церкви в центальной фишка игрока.
        /// 
        ///       F               F               F
        ///   |       |       |       |       |       |
        /// F |    ___| R   R |_______| R   R |___    | F
        ///   |   |   |       |       |       |   |   |
        ///       R               F               R
        /// 
        ///       R               F               R
        ///   |   |   |       |       |       |   |   |
        /// F |   |   | F   F |   O   | F   F |   |   | F
        ///   |   |   |       |       |       |   |   |
        ///       R               F               R
        /// 
        ///       R               F               R
        ///   |   |   |       |       |       |   |   |
        /// F |   \___| R   R |_______| R   R |__/    | F
        ///   |       |       |       |       |       |
        ///       F               F               F
        /// 
        /// </summary>
        [Fact]
        public void GetPointsTest()
        {
            var room = TestHelper.GetDefaultGame(TestHelper.Bob);
            room.ForTests_ResetTilesStack();
            room.ForTests_CustomTile(typeof(FFFF), 1, 0);
            room.ForTests_CustomTile(typeof(RFRF), 4, 0);
            room.ForTests_CustomTile(typeof(FFRR), 4, 0);
            var bob = room.PlayersPool.GetPlayer(TestHelper.Bob);

            var gameMove0 = new GameMove
            (
                playerName: TestHelper.Bob,
                tileId: "FFFF(0)",
                tileRotation: 0,
                tileLocation: new Point(0, 0),
                partName: "Monastery_0"
            );
            room.MakeMove(gameMove0);
            Assert.Single(room.State.Calculated.OfType<Monastery>());
            Assert.Equal(1, bob?.GetPlayerOverallScore(room.State));

            var gameMove1 = new GameMove
            (
                playerName: TestHelper.Bob,
                tileId: "RFRF(0)",
                tileRotation: 1,
                tileLocation: new Point(0, 1),
                partName: null
            );
            room.MakeMove(gameMove1);
            Assert.Single(room.State.Calculated.OfType<Monastery>());
            Assert.Equal(2, bob?.GetPlayerOverallScore(room.State));

            var gameMove2 = new GameMove
            (
                playerName: TestHelper.Bob,
                tileId: "RFRF(1)",
                tileRotation: 0,
                tileLocation: new Point(-1, 0),
                partName: null
            );
            room.MakeMove(gameMove2);
            Assert.Single(room.State.Calculated.OfType<Monastery>());
            Assert.Equal(3, bob?.GetPlayerOverallScore(room.State));

            var gameMove3 = new GameMove
            (
                playerName: TestHelper.Bob,
                tileId: "RFRF(2)",
                tileRotation: 0,
                tileLocation: new Point(1, 0),
                partName: null
            );
            room.MakeMove(gameMove3);
            Assert.Single(room.State.Calculated.OfType<Monastery>());
            Assert.Equal(4, bob?.GetPlayerOverallScore(room.State));

            var gameMove4 = new GameMove
            (
                playerName: TestHelper.Bob,
                tileId: "RFRF(3)",
                tileRotation: 1,
                tileLocation: new Point(0, -1),
                partName: null
            );
            room.MakeMove(gameMove4);
            Assert.Single(room.State.Calculated.OfType<Monastery>());
            Assert.Equal(5, bob?.GetPlayerOverallScore(room.State));

            var gameMove5 = new GameMove
            (
                playerName: TestHelper.Bob,
                tileId: "FFRR(0)",
                tileRotation: 0,
                tileLocation: new Point(1, 1),
                partName: null
            );
            room.MakeMove(gameMove5);
            Assert.Single(room.State.Calculated.OfType<Monastery>());
            Assert.Equal(6, bob?.GetPlayerOverallScore(room.State));

            var gameMove6 = new GameMove
            (
                playerName: TestHelper.Bob,
                tileId: "FFRR(1)",
                tileRotation: 3,
                tileLocation: new Point(-1, 1),
                partName: null
            );
            room.MakeMove(gameMove6);
            Assert.Single(room.State.Calculated.OfType<Monastery>());
            Assert.Equal(7, bob?.GetPlayerOverallScore(room.State));

            var gameMove7 = new GameMove
            (
                playerName: TestHelper.Bob,
                tileId: "FFRR(2)",
                tileRotation: 1,
                tileLocation: new Point(1, -1),
                partName: null
            );
            room.MakeMove(gameMove7);
            Assert.Single(room.State.Calculated.OfType<Monastery>());
            Assert.Equal(8, bob?.GetPlayerOverallScore(room.State));

            var gameMove8 = new GameMove
            (
                playerName: TestHelper.Bob,
                tileId: "FFRR(3)",
                tileRotation: 2,
                tileLocation: new Point(-1, -1),
                partName: null
            );
            room.MakeMove(gameMove8);
            Assert.Single(room.State.Calculated.OfType<Monastery>());
            Assert.Equal(18, bob?.GetPlayerOverallScore(room.State));

            var monastery = room.State.Calculated.OfType<Monastery>().ElementAt(0);
            Assert.True(monastery.IsComplete(room.State));
            Assert.Single(monastery.Parts);

            Assert.Single(room.State.Calculated.OfType<Road>());
            Assert.Equal(2, room.State.Calculated.OfType<Farm>().Count());
        }
    }
}
