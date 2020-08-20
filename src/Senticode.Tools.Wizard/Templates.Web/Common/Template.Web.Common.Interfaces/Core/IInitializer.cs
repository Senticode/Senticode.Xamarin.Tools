using Microsoft.Extensions.DependencyInjection;

namespace _template.Web.Common.Interfaces.Core
{
    public interface IInitializer
    {
        IServiceCollection Initialize(IServiceCollection services);
    }
}