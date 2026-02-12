using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Carcassone.Core.Players
{
    public static class ListHelper
    {
        public static bool HasDuplicates<T>(this List<T> list)
        {
            // If the count of distinct items is less than the total count, there are duplicates.
            return list.Count() != list.Distinct().Count();
        }
    }

    /// <summary>
    /// Класс управляющий пулом игроков одной игровой комнаты.
    /// </summary>
    public class GamePlayersPool
    {
        private static readonly int _maximumPlayersCount = 5;
        private static readonly int _playerMeeplesCount = 7;

        public List<GamePlayer> GamePlayers { get; } = new List<GamePlayer>();
        public int CurrentPlayerIndex { get; set; }

        public GamePlayersPool(List<PlayerInfo> playerInfos)
        {
            if (playerInfos.Count >= _maximumPlayersCount)
                throw new Exception($"Maximum player count is {_maximumPlayersCount}");

            if (playerInfos.Select(i => i.Name).ToList().HasDuplicates())
                throw new Exception($"There is a duplicate player's name");

            if (playerInfos.Select(i => i.Color).ToList().HasDuplicates())
                throw new Exception($"There is a duplicate player's color");

            foreach (var playerInfo in playerInfos)
            {
                GamePlayers.Add(new GamePlayer(playerInfo));
            }
        }

        public GamePlayer GetPlayer(string playerName)
        {
            var playerItems = GamePlayers.Where(_player => _player.Info.Name == playerName);
            if (playerItems.Count() == 0)
                return null;

            if (playerItems.Count() > 1)
                throw new Exception($"Duplicate players {playerName} in PlayersPool");

            return playerItems.Single();
        }

        public GamePlayer GetCurrentPlayer() => GamePlayers[CurrentPlayerIndex];

        public void MoveToNextPlayer()
        {
            // передать ход
            // если ход сделан то ход передается следующему игроку
            CurrentPlayerIndex++;
            CurrentPlayerIndex %= GamePlayers.Count;
        }

        private string GetFreeColor()
        {
            var takenColors = GamePlayers.Select(player => player.Info.Color).ToList();

            if (!takenColors.Contains("Blue"))
                return "Blue";

            if (!takenColors.Contains("Gray"))
                return "Gray";

            if (!takenColors.Contains("Green"))
                return "Green";

            if (!takenColors.Contains("Red"))
                return "Red";

            /*if (!takenColors.Contains("White"))
                return "White";*/

            if (!takenColors.Contains("Yellow"))
                return "Yellow";

            throw new System.Exception("нет свободных ячеек для игроков");
        }
    }
}
