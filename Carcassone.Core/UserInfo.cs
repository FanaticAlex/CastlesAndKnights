
namespace Carcassone.Core
{
    /// <summary>
    /// Хранит статистику игр одного игрока
    /// </summary>
    public class UserInfo
    {
        public string? Name { get; set; }
        public int GamesCount { get; set; }
        public int WinCount { get; set; }
    }
}
