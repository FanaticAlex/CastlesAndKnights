using System.ComponentModel.DataAnnotations;

namespace Carcassone.DAL.Data
{

    /// <summary>
    /// Результат сыгранной игры
    /// </summary>
    public class PlayedGame
    {
        [Key]
        public string Id { get; set; }
        public DateTime DateTime { get; set; }
        public List<PlayerFinalResult> PlayerFinalResultList { get; set; }
    }
}
