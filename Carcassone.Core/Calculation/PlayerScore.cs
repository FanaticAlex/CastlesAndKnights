namespace Carcassone.Core.Calculation
{
    public class PlayerScore
    {
        public string PlayerName { get; set; }
        
        public int ChurchesScore { get; set; }
        public int CornfieldsScore { get; set; }
        public int RoadsScore { get; set; }
        public int CastlesScore { get; set; }

        public int ChurchesCount { get; set; }
        public int CornfieldsCount { get; set; }
        public int RoadsCount { get; set; }
        public int CastlesCount { get; set; }

        public int ChipCount { get; set; }

        public int Rank { get; set; }

        public int GetOverallScore()
        {
            return ChurchesScore + CornfieldsScore + RoadsScore + CastlesScore;
        }
    }
}
