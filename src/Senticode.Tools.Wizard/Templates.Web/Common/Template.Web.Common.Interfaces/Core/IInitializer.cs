using Microsoft.Extensions.DependencyInjection;

namespace Template.Web.Common.Interfaces.Core
{
    public interface IInitializer
    {
        IServiceCollection Initialize(IServiceCollection services);
    }
}
