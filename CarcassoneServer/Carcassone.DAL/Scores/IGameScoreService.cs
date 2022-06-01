using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carcassone.DAL
{
    public interface IGameScoreService
    {
        IEnumerable<GameScore> GetUserScores(string userName);
        IEnumerable<GameScore> GetGameScores(string gameId);
        void WriteUserScore(GameScore gameScore);
    }
}
