using Unity;

namespace Senticode.Xamarin.Tools.Core.Interfaces.Base
{
    public interface IAppComponentLocator<out TAppSettings, out TAppCommands, out TLifeTimeManager>: IAppComponentLocator<TAppSettings, TAppCommands>
    {
        TLifeTimeManager AppLifeTimeManager { get; }
    }

    public interface IAppComponentLocator<out TAppSettings, out TAppCommands>: IAppComponentLocator<TAppSettings>
    {
        TAppCommands AppCommands { get; }
    }
    
    public interface IAppComponentLocator<out TAppSettings> : IAppComponentLocator
    {
        TAppSettings AppSettings { get; }
    }

    /// <summary>
    ///     Base interface to the classes witch provide access to the IUnityContainer like property.
    /// </summary>
    public interface IAppComponentLocator
    {
        /// <summary>
        ///     Gets IoC Container proxy.
        /// </summary>
        IUnityContainer Container { get; }
    }
}
