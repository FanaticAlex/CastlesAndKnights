using Carcassone.DAL.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Carcassone.DAL
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
