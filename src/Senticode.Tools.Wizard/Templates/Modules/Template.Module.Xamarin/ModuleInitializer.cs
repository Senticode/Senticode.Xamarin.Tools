using Senticode.Xamarin.Tools.Core.Interfaces.Base;
using Unity;

namespace _template.Module.Xamarin
{
    public class ModuleInitializer : IInitializer
    {
        public IUnityContainer Initialize(IUnityContainer container) => container;

        #region singleton

        private ModuleInitializer()
        {
        }

        public static ModuleInitializer Instance { get; } = new ModuleInitializer();

        #endregion
    }
}