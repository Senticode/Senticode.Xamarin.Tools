using System.Globalization;
using Senticode.Xamarin.Tools.Core.Interfaces.Services;
using Unity;

namespace Senticode.Xamarin.Tools.Core.Localize
{
    /// <summary>
    ///     Class that provides functionality for localization.
    /// </summary>
    public class DefaultLocalize : LocalizeBase
    {
        public DefaultLocalize(IUnityContainer container)
        {
            CultureContext = CultureInfo.CurrentUICulture;
            DefaultLanguage = CultureContext;
            AvailableImplementation = GetCultures();
            container.RegisterInstance<ILocalize>(this);
        }
    }
}
