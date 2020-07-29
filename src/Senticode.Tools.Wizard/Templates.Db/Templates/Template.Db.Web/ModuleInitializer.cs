using Microsoft.Extensions.DependencyInjection;
using Senticode.Database.Tools.Interfaces;
using Template.Db.Web.Services;

namespace Template.Db.Web
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