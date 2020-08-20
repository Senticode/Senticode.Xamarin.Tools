using Senticode.Xamarin.Tools.Core.Interfaces.Base;
using Unity;

namespace _template.Mobile.ModuleAggregator
{
    public class ModuleAggregator : IInitializer
    {
        public bool IsRegistered { get; private set; }

        public IUnityContainer Initialize(IUnityContainer container)
        {
            if (!IsRegistered)
            {
                IsRegistered = true;
            }

            return container;
        }

        #region singleton

        private ModuleAggregator()
        {
        }

        public static ModuleAggregator Instance { get; } = new ModuleAggregator();

        #endregion
    }
}