using Microsoft.EntityFrameworkCore;
using Template.Db.Xamarin.Helpers;
using Template.Db.Xamarin.Models;

namespace Template.Db.Xamarin
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