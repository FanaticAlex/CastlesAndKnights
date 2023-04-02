using Carcassone.DAL;
using System.Collections.Generic;

namespace Carcassone.Server.Services
{
    public interface IGameScoreService
    {
        /// <summary>
        /// Возвращает все результаты сыграных игр.
        /// </summary>
        /// <param name="userName">Имя игрока.</param>
        /// <returns>Результаты игрока.</returns>
        IEnumerable<UserGameScore> GetUserScores(string userName);

        /// <summary>
        /// Возвращает результаты игроков одной игры.
        /// </summary>
        /// <param name="gameId">Id игры.</param>
        /// <returns>Результаты игроков.</returns>
        IEnumerable<UserGameScore> GetGameScores(string gameId);

        /// <summary>
        /// Сохраняет результат игрока.
        /// </summary>
        /// <param name="gameScore">Результат игрока.</param>
        void SaveUserGameScore(UserGameScore gameScore);

        /// <summary>
        /// Возвращает статистику игрока.
        /// </summary>
        /// <param name="userName">Имя игрока.</param>
        /// <returns>Статистика игрока.</returns>
        UserStatistic GetUserStatistic(string userName);
    }
}
