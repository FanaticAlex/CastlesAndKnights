using Carcassone.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carcassone.Server.Services
{
    public class GameScoreService : IGameScoreService
    {
        private CarcassoneContext _context;

        public GameScoreService(CarcassoneContext context)
        {
            _context = context;
        }

        public IEnumerable<GameScore> GetGameScores(string gameId)
        {
            return _context.GameScores.Where(g => g.RoomId == gameId);
        }

        public IEnumerable<GameScore> GetUserScores(string userName)
        {
            return _context.GameScores.Where(g => g.UserName == userName);
        }

        public void WriteUserScore(GameScore gameScore)
        {
            _context.GameScores.Add(gameScore);
            _context.SaveChanges();
        }
    }
}
