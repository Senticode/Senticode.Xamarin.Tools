using Microsoft.EntityFrameworkCore;
using Senticode.Database.Tools.Tests.Entities;

namespace Senticode.Database.Tools.Tests.Framework
{
    public class TestDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString =
                "Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=senticode-db-tools-tests;Integrated Security=True";
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
