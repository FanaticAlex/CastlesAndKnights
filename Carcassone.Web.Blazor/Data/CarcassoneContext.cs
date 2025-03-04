using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Carcassone.Web.Blazor.Data
{
    public class CarcassoneContext : IdentityDbContext<CarcassoneUser>
    {
        public CarcassoneContext() { }

        public CarcassoneContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<PlayedGame> PlayedGameList { get; set; }
        public DbSet<PlayerFinalResult> PlayerFinalResultList { get; set; }
    }
}
