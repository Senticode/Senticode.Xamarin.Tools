using Senticode.Xamarin.Tools.Core.Interfaces.Base;
using Unity;

namespace Senticode.Xamarin.Tools.Core
{
    /// <summary>
    ///     Represents a type, which initialize necessary application components
    /// </summary>
    public abstract class PlatformInitializerBase : IPlatformInitializer
    {
        protected PlatformInitializerBase()
        {
            ServiceLocator.SetContainer(Container);
        }

        #region Implementation of IInitializer

        /// <summary>
        ///     Registers types in IoC container.
        /// </summary>
        /// <param name="container">IoC container.</param>
        /// <returns>IoC container.</returns>
        public abstract IUnityContainer Initialize(IUnityContainer container);

        #endregion

        #region Implementation of IAppComponentLocator

        /// <summary>
        ///     Gets the Container property.
        /// </summary>
        public IUnityContainer Container { get; } = new UnityContainer();

        #endregion
    }
}