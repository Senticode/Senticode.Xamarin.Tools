using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Senticode.Xamarin.Tools.Core.Interfaces.Base;
using Unity;
using Xamarin.Forms;

namespace Senticode.Xamarin.Tools.Core.Abstractions.Base
{
    /// <summary>
    ///     Class that contains application commands.
    /// </summary>
    public abstract class AppCommandsBase : IAppComponentLocator
    {
        protected AppCommandsBase(IUnityContainer container)
        {
            Container = container;
        }

        public IUnityContainer Container { get; }

        /// <summary>
        ///     Registers commands in the IoC container.
        /// </summary>
        /// <param name="container">IoC container.</param>
        public abstract void RegisterTypes(IUnityContainer container);
    }

    /// <summary>
    ///     Class that contains application commands.
    /// </summary>
    public abstract class AppCommandsBase<TAppSettings> : AppCommandsBase, IAppComponentLocator<TAppSettings>
        where TAppSettings : AppSettingsBase
    {
        protected AppCommandsBase(IUnityContainer container) : base(container)
        {
        }

        public TAppSettings AppSettings => Container.Resolve<TAppSettings>();


        #region SaveAppSettingsCommand

        /// <summary>
        ///     Gets the SaveAppSettings async command.
        /// </summary>
        public Command SaveAppSettingsCommand => _saveAppSettingsCommand ??
                                                 (_saveAppSettingsCommand =
                                                     new Command(async () => await ExecuteSaveAppSettingsAsync()));

        private Command _saveAppSettingsCommand;

        /// <summary>
        ///     Method to invoke when the SaveAppSettings command is executed.
        /// </summary>
        private async Task ExecuteSaveAppSettingsAsync()
        {
            try
            {
                await AppSettings.SaveAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        #endregion

        #region LoadAppSettingsCommand

        /// <summary>
        ///     Gets the LoadAppSettings async command.
        /// </summary>
        public Command LoadAppSettingsCommand => _loadAppSettingsCommand ??
                                                 (_loadAppSettingsCommand =
                                                     new Command(async () => await ExecuteLoadAppSettingsAsync()));

        private Command _loadAppSettingsCommand;

        /// <summary>
        ///     Method to invoke when the LoadAppSettings command is executed.
        /// </summary>
        protected virtual async Task ExecuteLoadAppSettingsAsync()
        {
            try
            {
                await AppSettings.LoadAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        #endregion
    }
}