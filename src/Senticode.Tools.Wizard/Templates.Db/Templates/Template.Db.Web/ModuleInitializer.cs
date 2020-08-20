using _template.Db.Web.Services;
using Microsoft.Extensions.DependencyInjection;
using Senticode.Database.Tools.Interfaces;

namespace _template.Db.Web
{
    public class ModuleInitializer
    {
        public void Initialize(IServiceCollection services)
        {
            services
                .AddScoped<DatabaseContext>()
                .AddSingleton<IConnectionManager, ConnectionManager>()
                ;

            DatabaseInitializer.Instance.Initialize(services.BuildServiceProvider());
        }

        #region singleton

        private ModuleInitializer()
        {
        }

        public static ModuleInitializer Instance { get; } = new ModuleInitializer();

        #endregion
    }
}