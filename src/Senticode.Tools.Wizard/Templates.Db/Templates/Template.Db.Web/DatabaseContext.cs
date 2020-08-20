using _template.Db.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace _template.Db.Web
{
    internal class DatabaseContext : DbContext
    {
        public DbSet<SampleModel> SampleModels { get; set; }
    }
}