using Microsoft.Extensions.DependencyInjection;
using Template.Web.Common.Interfaces.Core;

namespace Template.Module.Web
{
    public class ModuleInitializer : IInitializer
    {
        public static ModuleInitializer Instance { get; } = new ModuleInitializer();

        public IServiceCollection Initialize(IServiceCollection services)
{

    return services;
}        
    }
}
