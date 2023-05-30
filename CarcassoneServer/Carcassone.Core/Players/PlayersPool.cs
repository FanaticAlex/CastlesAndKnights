using System;
using System.Collections.Generic;
using System.Linq;

namespace Carcassone.Core.Players
{
    /// <summary>
    /// Класс управляющий пулом игроков одной игровой комнаты.
    /// </summary>
    public class PlayersPool
    {
        private static readonly int _maximumPlayersCount = 5;
        private static readonly int _playerChipCount = 7;

        public PlayersPool()
        {
        }

        public BasePlayer GetPlayer(string playerName)
        {
            var playerItems = Players.Where(_player => _player.Name == playerName);

            if (playerItems.Count() == 0)
                throw new Exception($"Player {playerName} not found");

            if (playerItems.Count() > 1)
                throw new Exception($"Duplicate players {playerName} in PlayersPool");

            return playerItems.Single();
        }

        public int CurrentPlayerIndex { get; set; } = -1;

        public BasePlayer? GetCurrentPlayer() => (CurrentPlayerIndex == -1) ? null : Players[CurrentPlayerIndex];

        public List<BasePlayer> Players { get; } = new List<BasePlayer>();

        public void DeletePlayer(string name)
        {
            var player = Players.FirstOrDefault(_player => _player.Name == name);
            if (player == null)
                return;

            Players.Remove(player);
        }

        public BasePlayer AddPlayer(string name, PlayerType type)
        {
            if (Players.Count >= _maximumPlayersCount)
                throw new Exception($"Cant add player. Maximum player count is {_maximumPlayersCount}");

            // если этот игрок уже подключен
            var player = Players.FirstOrDefault(_player => _player.Name == name);
            if (player != null)
                throw new Exception($"Игрок '{name}' уже подключен.");

            var color = GetFreeColor();
            var player1 = new BasePlayer(name, color, _playerChipCount, type);
            Players.Add(player1);
            return player1;
        }

        public void MoveToNextPlayer()
        {
            // передать ход
            // если ход сделан то ход передается следующему игроку
            CurrentPlayerIndex++;
            CurrentPlayerIndex %= Players.Count;
        }

        private string GetFreeColor()
        {
            var takenColors = Players.Select(player => player.Color).ToList();

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
