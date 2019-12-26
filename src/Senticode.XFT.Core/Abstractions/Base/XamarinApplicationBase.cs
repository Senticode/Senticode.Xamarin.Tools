using Senticode.Xamarin.Tools.Core.Interfaces.Base;
using Unity;
using Xamarin.Forms;

namespace Senticode.Xamarin.Tools.Core.Abstractions.Base
{
    public abstract class XamarinApplicationBase<TAppSettings, TAppCommands, TLifeTimeManager> :
        XamarinApplicationBase<TAppSettings, TAppCommands>,
        IAppComponentLocator<TAppSettings, TAppCommands, TLifeTimeManager>
        where TAppCommands : AppCommandsBase<TAppSettings>
        where TAppSettings : AppSettingsBase
        where TLifeTimeManager : AppLifeTimeManagerBase<TAppSettings, TAppCommands>
    {

        protected XamarinApplicationBase(IPlatformInitializer initializer) : base(initializer)
        {
            InternalInitialize(initializer);
        }

        /// <summary>
        ///     Gets the AppCommands property.
        /// </summary>
        public TLifeTimeManager AppLifeTimeManager => Container.Resolve<TLifeTimeManager>();

        /// <summary>
        ///     Registers types in IoC container.
        /// </summary>
        /// <param name="initializer"></param>
        private void InternalInitialize(IPlatformInitializer initializer)
        {
            if (!initializer.Container.IsRegistered<TLifeTimeManager>())
            {
                initializer.Container.RegisterType<TLifeTimeManager>()
                    .RegisterType<AppLifeTimeManagerBase<TAppSettings, TAppCommands>, TLifeTimeManager>();

                initializer.Container
                    .RegisterInstance(AppLifeTimeManager)
                    .RegisterInstance<AppLifeTimeManagerBase<TAppSettings, TAppCommands>>(AppLifeTimeManager);
            }
        }

        protected override void OnStart()
        {
            AppLifeTimeManager.OnStart();
            base.OnStart();
        }

        protected override void OnSleep()
        {
            AppLifeTimeManager.OnSleep();
            base.OnSleep();
        }

        protected override void OnResume()
        {
            AppLifeTimeManager.OnResume();
            base.OnResume();
        }


        protected override void OnPageAppearing(object sender, Page page)
        {
            if (Current == this)
            {
                AppLifeTimeManager.OnPageAppearing(page);
            }

            base.OnPageAppearing(sender, page);
        }

        protected override void OnPageDisappearing(object sender, Page page)
        {
            if (Current == this)
            {
                AppLifeTimeManager.OnPageDisappearing(page);
            }

            base.OnPageDisappearing(sender, page);
        }
    }


    /// <summary>
    ///     Class that represents a cross-platform mobile application.
    /// </summary>
    public abstract class XamarinApplicationBase<TAppSettings, TAppCommands> : XamarinApplicationBase,
        IAppComponentLocator<TAppSettings, TAppCommands>
        where TAppSettings : AppSettingsBase where TAppCommands : AppCommandsBase<TAppSettings>
    {
        protected XamarinApplicationBase(IPlatformInitializer initializer) : base(initializer)
        {
            InternalInitialize(initializer);
        }

        /// <summary>
        ///     Gets the AppSettings property.
        /// </summary>
        public TAppSettings AppSettings => Container.Resolve<TAppSettings>();

        /// <summary>
        ///     Gets the AppCommands property.
        /// </summary>
        public TAppCommands AppCommands => Container.Resolve<TAppCommands>();

        /// <summary>
        ///     Registers types in IoC container.
        /// </summary>
        /// <param name="initializer"></param>
        private void InternalInitialize(IPlatformInitializer initializer)
        {
            if (!initializer.Container.IsRegistered<TAppSettings>() && !initializer.Container.IsRegistered<TAppCommands>())
            {
                initializer.Container.RegisterType<TAppSettings>()
                    .RegisterType<TAppCommands>();

                initializer.Container
                    .RegisterInstance(AppSettings)
                    .RegisterInstance(AppCommands)
                    .RegisterInstance<AppSettingsBase>(AppSettings)
                    .RegisterInstance<AppCommandsBase<TAppSettings>>(AppCommands)
                    .RegisterInstance<AppCommandsBase>(AppCommands);

                AppCommands.RegisterTypes(initializer.Container);
            }
        }
    }

    /// <summary>
    ///     Class that represents a cross-platform mobile application.
    /// </summary>
    public abstract class XamarinApplicationBase : Application, IAppComponentLocator
    {
        protected XamarinApplicationBase(IPlatformInitializer initializer)
        {
            Container = initializer.Container;
            InternalInitialize(initializer);
        }

        /// <summary>
        ///     Gets or sets the Navigation property.
        /// </summary>
        public INavigation Navigation => Container.Resolve<INavigation>();

        /// <summary>
        ///     Gets the Container property.
        /// </summary>
        public IUnityContainer Container { get; }

        /// <summary>
        ///     Registers types in IoC container.
        /// </summary>
        /// <param name="initializer"></param>
        private void InternalInitialize(IPlatformInitializer initializer)
        {
            initializer.Initialize(Container);
            RegisterTypes(Container);
            OnInitialized();
        }

        protected virtual void OnInitialized()
        {
            PageAppearing += OnPageAppearing;
            PageDisappearing += OnPageDisappearing;
        }

        protected virtual void OnPageAppearing(object sender, Page page)
        {
        }

        protected virtual void OnPageDisappearing(object sender, Page page)
        {
        }


        protected abstract void RegisterTypes(IUnityContainer container);
    }
}