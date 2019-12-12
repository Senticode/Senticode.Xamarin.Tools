using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Resources;
using Senticode.Xamarin.Tools.Core.Helpers;
using Senticode.Xamarin.Tools.Core.Interfaces.Services;
using Senticode.Xamarin.Tools.Core.Localize;
using Unity;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Senticode.Xamarin.Tools.Core.MarkupExtensions
{
    /// <summary>
    ///     Provides functionality for localization.
    /// </summary>
    [ContentProperty(nameof(ResourceKey))]
    public class LocalizeExtension : WeakMarkupExtensionBase, IMarkupExtension
    {
        private static CultureInfo _cultureInfo;

        private static readonly Lazy<ResourceManager> ResManager =
            new Lazy<ResourceManager>(() => ServiceLocator.LocalizationManager);

        private static readonly Lazy<LocalizeBase> Localize =
            new Lazy<LocalizeBase>(() => (LocalizeBase) ServiceLocator.Container.Resolve<ILocalize>());

        private BindableObject _bo;
        private BindableProperty _property;

        /// <summary>
        ///     Gets or sets the ResourceKey property.
        /// </summary>
        public string ResourceKey { get; set; }

        /// <summary>
        ///     Gets or sets the Binding property.
        /// </summary>
        public BindingBase Binding { get; set; }

        /// <summary>
        ///     Gets or sets a string that specifies how to format the binding if it displays the bound value as a string.
        /// </summary>
        public string StringFormat { get; set; }

        /// <summary>
        ///     Gets localized string.
        /// </summary>
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            SetProvideValue(serviceProvider);
            SetBindableObject(serviceProvider);
            Localize.Value.LocalizeChanged += UpdateProperty;
            if (!string.IsNullOrEmpty(ResourceKey))
            {
                return GetStringFormatValue(GetTranslation(ResourceKey));
            }

            if (Binding != null)
            {
                if (!(Binding is Binding binding) || string.IsNullOrWhiteSpace(binding.Path))
                {
                    throw new InvalidOperationException($"'{nameof(Binding)}' must be properly set.");
                }

                SetBindableObject(serviceProvider);
                if (_bo != null) _bo.BindingContextChanged += LocalizeExtension_BindingContextChanged;
                return null;
            }

            if (_bo != null)
            {
                _bo.BindingContextChanged -= LocalizeExtension_BindingContextChanged;
            }

            return null;
        }

        /// <summary>
        ///     Subscribes to the events.
        /// </summary>
        public override void Subscribe()
        {
            if (Localize != null) Localize.Value.LocalizeChanged += UpdateProperty;
            if (Binding != null && _bo != null) _bo.BindingContextChanged += LocalizeExtension_BindingContextChanged;
        }

        public override void Unsubscribe()
        {
            // TODO EM: iOS not work correctly with Unsubscribe
            //    if (_localize != null) _localize.LocalizeChanged -= UpdateProperty;
            //    if (Binding != null && _bo != null) _bo.BindingContextChanged -= LocalizeExtension_BindingContextChanged;
        }

        private void LocalizeExtension_BindingContextChanged(object sender, EventArgs e)
        {
            var bo = (BindableObject) sender;
            try
            {
                //EM: BUG_FIX -> this code is bug_fixing can we see to only UWP
                //When working with for example ListView, UWP first sets parent 
                //BindingContext and then assigns the true BindingContext for ListItem.
                //The solution for the fix for the _bug is to wait for the right binding context.
                bo.SetValue(_property,
                    GetStringFormatValue(
                        GetTranslation((string) MarkupExtensionHelper.ExtractMember(bo, (Binding) Binding))));
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
                bo.SetValue(_property, null);
            }
        }

        private void SetBindableObject(IServiceProvider serviceProvider)
        {
            var pvt = (IProvideValueTarget) serviceProvider.GetService(typeof(IProvideValueTarget));
            _bo = pvt.TargetObject as BindableObject;
            _property = pvt.TargetProperty as BindableProperty;
        }

        private string GetTranslation(string resourceKey)
        {
            if (string.IsNullOrWhiteSpace(resourceKey))
            {
                return string.Empty;
            }

            try
            {
                if (_cultureInfo == null)
                {
                    _cultureInfo = Localize.Value.CultureContext;
                }

                var translate = ResManager.Value.GetString(resourceKey, _cultureInfo) ?? resourceKey;
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

        private void UpdateProperty(CultureInfo culture)
        {
            _cultureInfo = culture;
            if (!string.IsNullOrEmpty(ResourceKey))
            {
                _bo.SetValue(_property, GetStringFormatValue(GetTranslation(ResourceKey)));
            }
            else
            {
                _bo.SetValue(_property,
                    GetStringFormatValue(
                        GetTranslation((string) MarkupExtensionHelper.ExtractMember(_bo, (Binding) Binding))));
            }
        }

        internal string GetStringFormatValue(string value)
        {
            switch (StringFormat)
            {
                case "%u":
                    return value.ToUpper();
                case "%l":
                    return value.ToLower();
                case null:
                    return value;
                default:
                    return string.Format(StringFormat, value);
            }
        }
    }
}