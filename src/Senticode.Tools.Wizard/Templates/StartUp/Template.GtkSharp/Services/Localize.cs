using System.Globalization;
using Senticode.Xamarin.Tools.Core.Interfaces.Services;
using Senticode.Xamarin.Tools.Core.Localize;
using Unity;

namespace Template.GtkSharp.Services
{
    /// <summary>
    ///     Class that provides functionality for localization.
    /// </summary>
    internal class Localize : LocalizeBase
    {
        public Localize(IUnityContainer container)
        {
            CultureContext = CultureInfo.CurrentUICulture;
            DefaultLanguage = CultureContext;
            AvailableImplementation = GetCultures();
            container.RegisterInstance<ILocalize>(this);
        }
    }
}