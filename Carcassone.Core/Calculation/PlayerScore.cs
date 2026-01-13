namespace Carcassone.Core.Calculation
{
    public class PlayerScore
    {
        public string PlayerName { get; set; }
        
        public int ChurchesScore { get; set; }
        public int FarmsScore { get; set; }
        public int RoadsScore { get; set; }
        public int CitysScore { get; set; }

        public int ChurchesCount { get; set; }
        public int FarmsCount { get; set; }
        public int RoadsCount { get; set; }
        public int CitysCount { get; set; }

        public int ChipCount { get; set; }

        public int Rank { get; set; }

        public int GetOverallScore()
        {
            return ChurchesScore + FarmsScore + RoadsScore + CitysScore;
        }
    }
}
