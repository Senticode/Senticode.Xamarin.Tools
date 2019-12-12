using Unity;

namespace Senticode.Xamarin.Tools.Core.Interfaces.Base
{
    /// <summary>
    ///     The interface for dependency injection of the application module in the common <see cref = "IUnityContainer" />.
    /// </summary>
    public interface IInitializer
    {
        /// <summary>
        ///     The method injects dependencies of the application module in the common <see cref = "IUnityContainer" />.
        /// </summary>
        IUnityContainer Initialize(IUnityContainer container);
    }
}