using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private int _currentPlayerIndex;

        public PlayersPool()
        {
            _currentPlayerIndex = 0;
        }

        public Player? GetPlayer(string playerName) => Players.FirstOrDefault(_player => _player.Name == playerName);

        public Player CurrentPlayer => Players[_currentPlayerIndex];

        public List<Player> Players { get; } = new List<Player>();

        public void DeletePlayer(string name)
        {
            var player = Players.Single(p => p.Name == name);
            Players.Remove(player);
        }

        public Player AddHumanPlayer(string name)
        {
            // если этот игрок уже подключен
            var player = GetPlayer(name);
            if (player != null)
                return player;

            var color = GetFreeColor();
            var player1 = new Player(name, color, false, 7);
            Players.Add(player1);
            return player1;
        }

        public void AddAIPlayerEasy()
        {
            if (Players.Select(player => player.Name).Contains(EasyBotName))
                throw new Exception("Easy bot already added");

            var color = GetFreeColor();
            var player1 = new Player(EasyBotName, color, true, 7);
            Players.Add(player1);
        }

        public void MoveNextPlayer()
        {
            // передать ход
            // если ход сделан то ход передается следующему игроку
            _currentPlayerIndex++;
            _currentPlayerIndex = _currentPlayerIndex % Players.Count;
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
