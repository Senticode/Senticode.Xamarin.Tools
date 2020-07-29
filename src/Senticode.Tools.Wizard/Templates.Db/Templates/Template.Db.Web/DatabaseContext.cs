using Microsoft.EntityFrameworkCore;
using Template.Db.Web.Models;

namespace Template.Db.Web
{
    internal class DatabaseContext : DbContext
    {
        public DbSet<SampleModel> SampleModels { get; set; }
    }
}