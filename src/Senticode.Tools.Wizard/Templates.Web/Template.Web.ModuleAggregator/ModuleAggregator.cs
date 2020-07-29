using Microsoft.Extensions.DependencyInjection;
using Template.Web.Common.Interfaces.Core;

namespace Template.Web.ModuleAggregator
{
    public class ModuleAggregator : IInitializer
    {
        public static ModuleAggregator Instance { get; } = new ModuleAggregator();

        public IServiceCollection Initialize(IServiceCollection services)
        {
            return services;
        }
    }
}