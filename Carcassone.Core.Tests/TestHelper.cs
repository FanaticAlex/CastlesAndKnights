using Carcassone.Core.Players;
using System;
using System.Collections.Generic;


namespace Carcassone.Core.Tests
{
    internal class TestHelper
    {
        public static string Bob = "bob";

        public static string AI_1 = "AI_1";
        public static string AI_2 = "AI_2";

        public static GameRoom GetDefaultGame(string player)
        {
            var ext = new List<Extension>() { Extension.River };
            var players = new List<PlayerInfo>() { new PlayerInfo(player, PlayerType.Human, PlayerColor.Red, 7) };
            var room = new GameRoom(ext, players);
            return room;
        }

        public static GameRoom GetDefaultGame(string player1, string player2)
        {
            var ext = new List<Extension>() { Extension.River };
            var players = new List<PlayerInfo>()
            {
                new PlayerInfo(player1, PlayerType.Human, PlayerColor.Blue, 7),
                new PlayerInfo(player2, PlayerType.Human, PlayerColor.Green, 7),
            };
            var room = new GameRoom(ext, players);
            return room;
        }
    }
}
