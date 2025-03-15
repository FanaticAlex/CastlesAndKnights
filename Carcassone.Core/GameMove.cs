using Carcassone.Core.Calculation;

namespace Carcassone.Core
{
    public class GameMove
    {
        public string? PlayerName { get; set; }
        public string CardId { get; set; }
        public int CardRotation { get; set; }
        public string FieldId { get; set; }
        public string? PartName { get; set; }
        public PlayerScore ExpectedScore { get; set; }
    }
}
