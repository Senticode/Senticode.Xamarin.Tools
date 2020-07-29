using Senticode.Xamarin.Tools.Core.Interfaces.Base;
using Unity;

namespace Template.Module.Xamarin
{
    public class ModuleInitializer : IInitializer
    {
        public static ModuleInitializer Instance { get; } = new ModuleInitializer();
        public IUnityContainer Initialize(IUnityContainer container)
{
    return container;
}
    }
}
