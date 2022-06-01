using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;

namespace Carcassone.DAL
{
    public class CarcassoneContext : DbContext
    {
        public string DbPath { get; }

        public CarcassoneContext()
        {
        }

        public CarcassoneContext(DbContextOptions options) : base(options)
        {
            //var folder = Environment.SpecialFolder.LocalApplicationData;
            //var path = Environment.GetFolderPath(folder);
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            DbPath = System.IO.Path.Join(path, "Carcassone.db");
            Database.EnsureCreated();
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");

        //entities
        public DbSet<User> Users { get; set; }

        public DbSet<GameScore> GameScores { get; set; }
    }
}
