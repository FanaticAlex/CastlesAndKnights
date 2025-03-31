using System;
using System.Collections.Generic;
using System.Text;

namespace Carcassone.Core.Players
{
    /// <summary>
    /// Базовые свойства игрока, сохраняемые глобально
    /// </summary>
    public class Player
    {
        public string Name { get; set; }
        public PlayerType PlayerType { get; set; }
        public int GamesCount { get; set; }
        public int WinCount { get; set; }
    }
}
