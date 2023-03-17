namespace Carcassone.Core.Calculation
{
    public class PlayerScore
    {
        public PlayerScore(
            string playerName, int churches, int cornfields, int roads, int castles, int chipCount)
        {
            PlayerName = playerName;
            Churches = churches;
            Cornfields = cornfields;
            Roads = roads;
            Castles = castles;
            ChipCount = chipCount;
        }

        public string PlayerName { get; }
        public int Churches { get; }
        public int Cornfields { get; }
        public int Roads { get; }
        public int Castles { get; }
        public int ChipCount { get; }

        public int Rank { get; set; }

        public int GetOverallScore()
        {
            return Churches + Cornfields + Roads + Castles;
        }
    }
}
