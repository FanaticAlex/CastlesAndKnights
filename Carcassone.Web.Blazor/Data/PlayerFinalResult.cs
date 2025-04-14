using System.ComponentModel.DataAnnotations;

namespace Carcassone.Web.Blazor.Data
{
    /// <summary>
    /// Результат игрока в одной сыгранной игре.
    /// </summary>
    public class PlayerFinalResult
    {
        [Key]
        public string Id { get; set; }
        public CarcassoneUser? CarcassoneUser { get; set; }
        public required int FinalScore { get; set; }
        public required int Rank { get; set; }
    }
}
