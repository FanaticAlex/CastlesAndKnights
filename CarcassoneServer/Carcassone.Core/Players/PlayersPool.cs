using Carcassone.Core.Cards;
using Carcassone.Core.Players.AI;
using Newtonsoft.Json;
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
        public const string EasyBotName = "Esquire(Easy)";
        public const string MiddleBotName = "Vikont(Middle)";
        public const string HardBotName = "King(Hard)";

        private static readonly int _maximumPlayersCount = 5;
        private static readonly int _playerChipCount = 7;

        public PlayersPool()
        {
        }

        public BasePlayer? GetPlayer(string? playerName) => Players.FirstOrDefault(_player => _player.Name == playerName);

        public int CurrentPlayerIndex { get; set; } = -1;

        public BasePlayer GetCurrentPlayer() => (CurrentPlayerIndex == -1) ? null : Players[CurrentPlayerIndex];

        [JsonProperty(ItemConverterType = typeof(PlayerConverter))]
        public List<BasePlayer> Players { get; } = new List<BasePlayer>();

        public void DeletePlayer(string name)
        {
            var player = GetPlayer(name);
            if (player == null)
                return;

            Players.Remove(player);
        }

        public Player AddHumanPlayer(string name)
        {
            // если этот игрок уже подключен
            var player = GetPlayer(name);
            if (player != null)
                throw new Exception($"Игрок '{name}' уже подключен.");

            var color = GetFreeColor();
            var player1 = new Player(name, color, _playerChipCount);
            Players.Add(player1);
            return player1;
        }

        public void AddAIPlayerEasy()
        {
            if (Players.Count == _maximumPlayersCount)
                throw new Exception($"Cant add player. Maximum player count is {_maximumPlayersCount}");

            var botName = string.Empty;
            var botIndex = 0;
            while(true)
            {
                botIndex++;
                botName = $"{EasyBotName}_{botIndex}";
                if (!Players.Select(player => player.Name).Contains(botName))
                    break;
            }

            var color = GetFreeColor();
            var player1 = new PlayerAI(botName, color, _playerChipCount);
            Players.Add(player1);
        }

        public void MoveNextPlayer(GameRoom room)
        {
            // передать ход
            // если ход сделан то ход передается следующему игроку
            CurrentPlayerIndex++;
            CurrentPlayerIndex %= Players.Count;

            // ходят AI игроки
            var player = GetCurrentPlayer();
            if (player is PlayerAI aI)
                aI.ProcessMove(room);
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
