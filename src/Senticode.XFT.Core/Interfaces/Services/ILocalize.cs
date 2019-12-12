using System.Collections.Generic;
using System.Globalization;
using Senticode.Xamarin.Tools.Core.Localize;

namespace Senticode.Xamarin.Tools.Core.Interfaces.Services
{
    /// <summary>
    ///     Provides functionality for localization.
    /// </summary>
    public interface ILocalize
    {
        /// <summary>
        ///     Gets or sets culture cintext.
        /// </summary>
        CultureInfo CultureContext { get; set; }

        /// <summary>
        ///     List of available languages.
        /// </summary>
        List<CultureInfo> AvailableImplementation { get; }

        /// <summary>
        ///     Occurs when culture context changes.
        /// </summary>
        event LocalizeChangedEventHandler LocalizeChanged;

        /// <summary>
        ///     Gets default language for the application.
        /// </summary>
        CultureInfo DefaultLanguage { get; }
    }
}