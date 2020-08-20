using _template.Web.Common.Interfaces.Core;
using Microsoft.Extensions.DependencyInjection;

namespace _template.Module.Web
{
    public class ModuleInitializer : IInitializer
    {
        public IServiceCollection Initialize(IServiceCollection services) => services;

        #region singleton

        private ModuleInitializer()
        {
        }

        public static ModuleInitializer Instance { get; } = new ModuleInitializer();

        #endregion
    }
}