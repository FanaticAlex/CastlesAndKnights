using Carcassone.Core.Calculation;

namespace Carcassone.Core.Players.AI
{
    public class GameMove
    {
        public string FieldId { get; set; }
        public string? PartName { get; set; }
        public PlayerScore ExpectedScore { get; set; }

        public GameMove(string fieldId, string? partName, PlayerScore expectedScore)
        {
            FieldId = fieldId;
            PartName = partName;
            ExpectedScore = expectedScore;
        }
    }
}
