using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Carcassone.DAL
{
    public class CarcassoneContext : IdentityDbContext<IdentityUser>
    {
        public CarcassoneContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<UserGameScore> Scores { get; set; }
    }
}
