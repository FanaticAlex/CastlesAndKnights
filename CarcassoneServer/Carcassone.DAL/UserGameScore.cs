using System.ComponentModel.DataAnnotations;

namespace Carcassone.DAL
{
    /// <summary>
    /// Результат игры одного игрока
    /// </summary>
    public class UserGameScore
    {
        [Key]
        public int Id { get; set; }
        public string RoomId { get; set; }
        public string UserName { get; set; }
        public int FinalScore { get; set; }
        public int Rank { get; set; }

    }
}
