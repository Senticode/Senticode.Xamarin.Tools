using System;
using System.Diagnostics;
using System.Globalization;
using System.Xml.Serialization;
using Senticode.Xamarin.Tools.Core.Abstractions.Base;
using Senticode.Xamarin.Tools.Core.Interfaces.Services;
using Unity;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Template.MasterDetail
{
    /// <summary>
    ///     Global Settings Singleton.
    /// </summary>
    public class AppSettings : AppSettingsBase
    {
        public const string CONFIG_FILE = "Configuration.app.config";

        public AppSettings()
        {
        }

        #region ApplySettings

        protected override bool ApplySettings(AppSettingsBase settings)
        {
            if (settings is AppSettings appSettings)
            {
                if (appSettings.Language != null)
                {
                    Language = appSettings.Language;
                }

                return true;
            }

            return false;
        }

        #endregion

        #region NetworkAccess: NetworkAccess

        /// <summary>
        ///     Gets or sets the NetworkAccess value.
        /// </summary>
        [XmlIgnore]
        public NetworkAccess NetworkAccess
        {
            get => _networkAccess;
            set
            {
                if (_networkAccess != value)
                {
                    _networkAccess = value;
                    Device.BeginInvokeOnMainThread(() => OnPropertyChanged(nameof(NetworkAccess)));
                }
            }
        }


        /// <summary>
        ///     NetworkAccess property data.
        /// </summary>
        private NetworkAccess _networkAccess;

        private string _language;

        #endregion

        #region Language : string
        public string Language
        {
            get => _language;
            set
            {
                _language = value;
                OnLanguageChanged();
            }
        }

        private void OnLanguageChanged()
        {
            if (!string.IsNullOrEmpty(Language))
            {
                try
                {
                    var localize = Container.Resolve<ILocalize>();
                    var context = new CultureInfo(Language);
                    if (context.EnglishName != localize.CultureContext.EnglishName)
                    {
                        localize.CultureContext = context;
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
        }
        #endregion

        public Uri WebServiceAddress { get; set; }

    }
}