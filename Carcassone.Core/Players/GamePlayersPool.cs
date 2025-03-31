using System;
using System.Collections.Generic;
using System.Linq;

namespace Carcassone.Core.Players
{
    /// <summary>
    /// Класс управляющий пулом игроков одной игровой комнаты.
    /// </summary>
    public class GamePlayersPool
    {
        private static readonly int _maximumPlayersCount = 5;
        private static readonly int _playerChipCount = 7;

        public List<GamePlayer> GamePlayers { get; } = new List<GamePlayer>();
        public int CurrentPlayerIndex { get; set; } = -1;

        public GamePlayersPool()
        {
        }

        public GamePlayer? GetPlayer(string playerName)
        {
            var playerItems = GamePlayers.Where(_player => _player.Name == playerName);
            if (playerItems.Count() == 0)
                return null;

            if (playerItems.Count() > 1)
                throw new Exception($"Duplicate players {playerName} in PlayersPool");

            return playerItems.Single();
        }

        public GamePlayer? GetCurrentPlayer() => (CurrentPlayerIndex == -1) ? null : GamePlayers[CurrentPlayerIndex];

        public GamePlayer GetHumanPlayer(GameRoom room, string playerName)
        {
            var player = GetCurrentPlayer();
            if (player.Name != playerName)
                throw new Exception($"Its '{player.Name}' turn!");

            if (player.PlayerType != PlayerType.Human)
                throw new Exception($"Its AI '{player.Name}' turn!");

            return player;
        }

        public void DeletePlayer(string name)
        {
            var player = GamePlayers.FirstOrDefault(_player => _player.Name == name);
            if (player == null)
                return;

            GamePlayers.Remove(player);
        }

        public GamePlayer AddPlayer(Player player)
        {
            if (GamePlayers.Count >= _maximumPlayersCount)
                throw new Exception($"Cant add player. Maximum player count is {_maximumPlayersCount}");

            // если этот игрок уже подключен
            var gamePlayer = GamePlayers.FirstOrDefault(_player => _player.Name == player.Name);
            if (gamePlayer != null)
                throw new Exception($"Игрок '{player.Name}' уже подключен.");

            var color = GetFreeColor();
            var newGamePlayer = new GamePlayer(player, color, _playerChipCount);
            GamePlayers.Add(newGamePlayer);
            return newGamePlayer;
        }

        public void MoveToNextPlayer()
        {
            // передать ход
            // если ход сделан то ход передается следующему игроку
            CurrentPlayerIndex++;
            CurrentPlayerIndex %= GamePlayers.Count;
        }

        private string GetFreeColor()
        {
            var takenColors = GamePlayers.Select(player => player.Color).ToList();

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
