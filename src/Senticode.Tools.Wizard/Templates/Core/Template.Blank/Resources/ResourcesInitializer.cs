using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using Senticode.Xamarin.Tools.Core.Interfaces.Base;
using Senticode.Xamarin.Tools.Core.Localize;
using Unity;

namespace Template.Blank.Resources
{
    public class ResourcesInitializer : IInitializer
    {
        public static ResourcesInitializer Instance { get; } = new ResourcesInitializer();

        public IUnityContainer Initialize(IUnityContainer container)
        {
            var defaultLanguage = new CultureInfo("en");
            LocalizeBase.Init(new List<CultureInfo>
            {
                defaultLanguage,
                new CultureInfo("de"),
            });
            ResourceManagerInitialize(container);
            return container;
        }

        private void ResourceManagerInitialize(IUnityContainer container)
        {
            container.RegisterInstance(
                new ResourceManager($"{typeof(ResourcesInitializer).Namespace}.Localizations.Resource",
                    GetType().Assembly));
        }
    }
}