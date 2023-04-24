using Carcassone.DAL;
using System.Collections.Generic;
using System.Linq;

namespace Carcassone.Server.Services
{
    public class GameScoreService : IGameScoreService
    {
        private CarcassoneContext _context;

        public GameScoreService(CarcassoneContext context)
        {
            _context = context;
        }

        public IEnumerable<UserGameScore> GetGameScores(string gameId)
        {
            return _context.Scores.Where(g => g.RoomId == gameId);
        }

        public IEnumerable<UserGameScore> GetUserScores(string userName)
        {
            return _context.Scores.Where(g => g.UserName.ToLower() == userName.ToLower());
        }

        public UserStatistic GetUserStatistic(string userName)
        {
            var scores = GetUserScores(userName);
            var gamesCount = scores.Count();
            var winCount = scores.Where(score => score.Rank == 0).Count();

            var statistic = new UserStatistic()
            {
                UserName = userName,
                GamesCount = gamesCount,
                WinCount = winCount
            };

            return statistic;
        }

        public void SaveUserGameScore(UserGameScore gameScore)
        {
            _context.Scores.Add(gameScore);
            _context.SaveChanges();
        }
    }
}
