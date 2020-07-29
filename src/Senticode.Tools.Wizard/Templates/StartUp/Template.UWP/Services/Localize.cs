using System.Globalization;
using Senticode.Xamarin.Tools.Core.Interfaces.Services;
using Senticode.Xamarin.Tools.Core.Localize;
using Unity;

namespace Template.UWP.Services
{
    /// <summary>
    ///     Class that provides functionality for localization.
    /// </summary>
    internal class Localize : LocalizeBase
    {
        public Localize(IUnityContainer container)
        {
            container.RegisterInstance<ILocalize>(this);
            CultureContext = CultureInfo.CurrentUICulture;
            DefaultLanguage = CultureContext;
            AvailableImplementation = GetCultures();
        }
    }
}