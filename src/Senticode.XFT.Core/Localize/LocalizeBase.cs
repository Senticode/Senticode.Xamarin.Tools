using System.Collections.Generic;
using System.Globalization;
using Senticode.Xamarin.Tools.Core.Interfaces.Services;

namespace Senticode.Xamarin.Tools.Core.Localize
{
    public delegate void LocalizeChangedEventHandler(CultureInfo culture);

    /// <summary>
    ///     Class that provides functionality for localization.
    /// </summary>
    public abstract class LocalizeBase : ILocalize
    {
        private static readonly List<CultureInfo> _cultures = new List<CultureInfo>
        {
            CultureInfo.CurrentCulture
        };

        private static readonly object _locker = new object();

        private CultureInfo _cultureContext;

        /// <summary>
        ///     Gets or sets culture cintext.
        /// </summary>
        public CultureInfo CultureContext
        {
            get => _cultureContext;
            set
            {
                _cultureContext = value;
                LocalizeChanged?.Invoke(value);
                CultureInfo.CurrentUICulture = value;
            }
        }

        /// <summary>
        ///     List of available languages.
        /// </summary>
        public List<CultureInfo> AvailableImplementation { get; set; }

        /// <summary>
        ///     Occurs when culture context changes.
        /// </summary>
        public event LocalizeChangedEventHandler LocalizeChanged;

        /// <summary>
        ///     Gets or sets default language for the application.
        /// </summary>
        public CultureInfo DefaultLanguage { get; protected set; }

        /// <summary>
        ///     Sets available languages.
        /// </summary>
        /// <param name="cultures">Available languages.</param>
        public static void Init(IEnumerable<CultureInfo> cultures)
        {
            lock (_locker)
            {
                _cultures.Clear();
                _cultures.AddRange(cultures);
            }
        }

        /// <summary>
        ///     Gets list of availablel languages.
        /// </summary>
        /// <returns>List of languages.</returns>
        public static List<CultureInfo> GetCultures()
        {
            lock (_locker)
            {
                return _cultures;
            }
        }
    }
}