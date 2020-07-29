using Senticode.Xamarin.Tools.Core.Interfaces.Base;
using Unity;

namespace Template.Mobile.ModuleAggregator
{
    public class ModuleAggregator : IInitializer
    {
        public bool IsRegistered { get; private set; }

        public static ModuleAggregator Instance { get; } = new ModuleAggregator();

        public IUnityContainer Initialize(IUnityContainer container)
        {
            if (!IsRegistered) {
				
                IsRegistered = true;
            }

            return container;
        }
    }
}
