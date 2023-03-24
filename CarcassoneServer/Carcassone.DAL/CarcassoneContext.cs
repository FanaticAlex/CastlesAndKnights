using Microsoft.EntityFrameworkCore;

namespace Carcassone.DAL
{
    public class CarcassoneContext : DbContext
    {
        public CarcassoneContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }

        public DbSet<UserGameScore> Scores { get; set; }
    }
}
