using _template.Db.Xamarin.Services;
using Senticode.Database.Tools.Interfaces;
using Senticode.Xamarin.Tools.Core.Interfaces.Base;
using Unity;

namespace _template.Db.Xamarin
{
    public class ModuleInitializer : IInitializer
    {
        public IUnityContainer Initialize(IUnityContainer container)
        {
            container
                .RegisterSingleton<DatabaseContext>()
                .RegisterType<IConnectionManager, ConnectionManager>()
                ;

            DatabaseInitializer.Instance.Initialize(container);

            return container;
        }

        #region singleton

        private ModuleInitializer()
        {
        }

        public static ModuleInitializer Instance { get; } = new ModuleInitializer();

        #endregion
    }
}