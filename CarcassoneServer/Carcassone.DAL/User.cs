using System.ComponentModel.DataAnnotations;

namespace Carcassone.DAL
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string Password { get; set; }
    }
}
