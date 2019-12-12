using System;
using System.Diagnostics;
using System.IO;
using System.Resources;
using Senticode.Xamarin.Tools.Core.Interfaces.Services;
using Unity;

namespace Senticode.Xamarin.Tools.Core.Localize
{
    /// <summary>
    ///     Class with extension method for string localization.
    /// </summary>
    public static class LocalizeCodeExtension
    {
        private static readonly Lazy<ResourceManager> ResManager =
            new Lazy<ResourceManager>(() => ServiceLocator.LocalizationManager);

        private static readonly Lazy<LocalizeBase> Localize =
            new Lazy<LocalizeBase>(() => (LocalizeBase) ServiceLocator.Container.Resolve<ILocalize>());

        /// <summary>
        ///     Returns string for current culture context.
        /// </summary>
        /// <param name="value">String to localize.</param>
        /// <returns>Localized string.</returns>
        public static string L(this string value)
        {
            if (value == null)
            {
                value = string.Empty;
            }


            var resourceKey = value.Replace(' ', '_');


            if (string.IsNullOrWhiteSpace(resourceKey))
            {
                return string.Empty;
            }

            try
            {
                var translate = ResManager.Value.GetString(resourceKey, Localize.Value.CultureContext) ?? resourceKey;
                return translate;
            }
            catch (FileNotFoundException ex)
            {
                Debug.WriteLine(ex);
                return ResManager.Value.GetString(resourceKey,
                    Localize.Value.DefaultLanguage);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return resourceKey;
            }
        }
    }
}