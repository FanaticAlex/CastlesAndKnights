using System.ComponentModel.DataAnnotations;

namespace Carcassone.DAL
{
    public class GameScore
    {
        [Key]
        public int Id { get; set; }
        public string RoomId { get; set; }
        public string UserName { get; set; }
        public int FinalScore { get; set; }
    }
}
