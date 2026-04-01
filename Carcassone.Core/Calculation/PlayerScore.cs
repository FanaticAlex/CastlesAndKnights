namespace Carcassone.Core.Calculation
{
    public class PlayerScore
    {
        public PlayerScore(string name, int meepleCount, int overallScore)
        {
            PlayerName = name;
            MeeplesCount = meepleCount;
            OverallScore = overallScore;
        }

        public string PlayerName { get; set; }

        public int MeeplesCount { get; set; }

        public int Rank { get; set; }

        public int OverallScore {  get; set; }
    }
}
