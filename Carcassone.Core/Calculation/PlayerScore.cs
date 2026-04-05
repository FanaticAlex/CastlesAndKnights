using System.Collections.Generic;
using System.Linq;

namespace Carcassone.Core.Calculation
{
    public class ScoreObjDetails
    {
        public string ObjectName { get; set; }
        public int ObjectCount { get; set; }
        public int ObjectScore { get; set; }
    }

    public class PlayerScore
    {
        public PlayerScore(string name, int meeplesCount, int rank, int overallScore, MoveState state)
        {
            PlayerName = name;
            MeeplesCount = meeplesCount;
            Rank = rank;
            OverallScore = overallScore;

            scoreObjs = GetObjectsScores(state.Calculated, state);
        }

        public string PlayerName { get; set; }

        public int MeeplesCount { get; set; }

        public int Rank { get; set; }

        public int OverallScore {  get; set; }

        public List<ScoreObjDetails> scoreObjs { get; set; }

        private List<ScoreObjDetails> GetObjectsScores(List<BaseGameObject> objs, MoveState state)
        {
            var allPlayerObjects = objs.Where(obj => obj.IsPlayerOwner(PlayerName, state));
            var sorted = allPlayerObjects
                .GroupBy(obj => obj.GetType())
                .Select(g => new ScoreObjDetails()
                {
                    ObjectName = g.Key.Name,
                    ObjectCount = g.Count(),
                    ObjectScore = g.Select(obj => obj.GetScore(state)).Sum()
                });
            return sorted.ToList();
        }
    }
}
