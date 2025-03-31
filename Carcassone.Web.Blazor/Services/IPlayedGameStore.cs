using Carcassone.Core;
using Carcassone.Web.Blazor.Data;
using System.Collections.Generic;

namespace Carcassone.Web.Blazor.Services
{
    public interface IPlayedGameStore
    {
        /// <summary>
        /// Возвращает все результаты сыграных игр пользователя.
        /// </summary>
        IEnumerable<PlayedGame> GetPlayedGameList(CarcassoneUser user);

        /// <summary>
        /// Возвращает статистику игрока.
        /// </summary>
        PlayerInfo GetUserInfo(CarcassoneUser user);

        /// <summary>
        /// Сохраняет результат игры.
        /// </summary>
        public Task AddGameResults(GameRoom room);
    }
}
