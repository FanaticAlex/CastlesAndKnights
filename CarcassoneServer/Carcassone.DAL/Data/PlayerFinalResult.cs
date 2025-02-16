using System.ComponentModel.DataAnnotations;

namespace Carcassone.DAL.Data
{
    /// <summary>
    /// Результат игрока в одной сыгранной игре.
    /// </summary>
    public class PlayerFinalResult
    {
        [Key]
        public string Id { get; set; }
        public CarcassoneUser CarcassoneUser { get; set; }
        public int FinalScore { get; set; }
        public int Rank { get; set; }
    }
}
