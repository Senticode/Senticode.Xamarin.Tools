using _template.Db.Xamarin.Helpers;
using _template.Db.Xamarin.Models;
using Microsoft.EntityFrameworkCore;

namespace _template.Db.Xamarin
{
    internal class DatabaseContext : DbContext
    {
        public DbSet<SampleModel> SampleModels { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = LocalStorageHelper.GetDatabasePath("_connectionstring_");
            /*_sqlite_*/ /*optionsBuilder.UseSqlite(connectionString);*/
        }
    }
}