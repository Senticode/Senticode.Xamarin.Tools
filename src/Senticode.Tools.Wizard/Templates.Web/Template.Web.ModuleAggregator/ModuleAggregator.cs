using _template.Web.Common.Interfaces.Core;
using Microsoft.Extensions.DependencyInjection;

namespace _template.Web.ModuleAggregator
{
    public class ModuleAggregator : IInitializer
    {
        public bool IsRegistered { get; private set; }

        public IServiceCollection Initialize(IServiceCollection services)
        {
            if (!IsRegistered)
            {
                // Modules registration.
                IsRegistered = true;
            }

            return services;
        }

        #region singleton

        private ModuleAggregator()
        {
        }

        public static ModuleAggregator Instance { get; } = new ModuleAggregator();

        #endregion
    }
}