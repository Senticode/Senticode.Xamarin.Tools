using _template.Web.Common.Interfaces.Core;
using Microsoft.Extensions.DependencyInjection;

namespace _template.Web.ModuleAggregator
{
    public class ModuleAggregator : IInitializer
    {
        public IServiceCollection Initialize(IServiceCollection services) => services;

        #region singleton

        private ModuleAggregator()
        {
        }

        public static ModuleAggregator Instance { get; } = new ModuleAggregator();

        #endregion
    }
}