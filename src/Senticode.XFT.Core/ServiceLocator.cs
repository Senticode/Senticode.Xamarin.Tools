using System;
using System.Resources;
using Senticode.Xamarin.Tools.Core.Events;
using Unity;

namespace Senticode.Xamarin.Tools.Core
{
    /// <summary>
    ///     Class that provides global access to IoC container, ResourseManager and EventAggregator.
    /// </summary>
    public static class ServiceLocator
    {
        private static Lazy<IEventAggregator> _eventAggregator = new Lazy<IEventAggregator>(() => new EventAggregator());

        /// <summary>
        ///     Gets or sets the Container property,
        /// </summary>
        public static IUnityContainer Container { get; private set; }

        /// <summary>
        ///     Gets the LocalizationManager property,
        /// </summary>
        public static ResourceManager LocalizationManager => Container.Resolve<ResourceManager>();

        /// <summary>
        ///     Gets the EventAggregator property,
        /// </summary>
        public static IEventAggregator EventAggregator => _eventAggregator.Value;

        /// <summary>
        ///     Sets the Container property.
        /// </summary>
        /// <param name="container"></param>
        public static void SetContainer(IUnityContainer container)
        {
            Container = container;
            RegisterEventAggregator();
        }
        
        private static void RegisterEventAggregator()
        {
            if (_eventAggregator.IsValueCreated)
            {
                Container.RegisterInstance(_eventAggregator.Value);
            }
            else
            {
                Container.RegisterType<IEventAggregator, EventAggregator>();
                _eventAggregator = new Lazy<IEventAggregator>(() => Container.Resolve<IEventAggregator>());
            }
        }
    }
}