namespace Senticode.Xamarin.Tools.Core.Interfaces.Base
{
    /// <summary>
    ///     Represents a type, which initialize necessary application components. Note: Initialized components - assemblies
    ///     which contains entry point like class of the implementation <see cref = "IInitializer" /> interface.
    /// </summary>
    public interface IPlatformInitializer : IInitializer, IAppComponentLocator
    {
    }
}