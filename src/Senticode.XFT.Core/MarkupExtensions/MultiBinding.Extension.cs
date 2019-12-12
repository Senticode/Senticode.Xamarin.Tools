using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Runtime.CompilerServices;
using Senticode.Xamarin.Tools.Core.Interfaces.Services;
using Senticode.Xamarin.Tools.Core.Interfaces.Staff;
using Senticode.Xamarin.Tools.Core.Localize;
using Unity;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Senticode.Xamarin.Tools.Core.MarkupExtensions
{
    /// <summary>
    ///     Provides functionality for multi binding implementation.
    /// </summary>
    [ContentProperty(nameof(Bindings))]
    public class MultiBinding : WeakMarkupExtensionBase, IMarkupExtension<Binding>, IMultiBinding
    {
        private static CultureInfo _cultureInfo;

        private static readonly Lazy<ResourceManager> ResManager =
            new Lazy<ResourceManager>(() => ServiceLocator.LocalizationManager);

        private static readonly Lazy<LocalizeBase> Localize =
            new Lazy<LocalizeBase>(() => (LocalizeBase) ServiceLocator.Container.Resolve<ILocalize>());

        private readonly InternalValue _internalValue = new InternalValue();
        private readonly IList<BindableProperty> _properties = new List<BindableProperty>();
        private bool _isLocalizable;

        private BindableObject _target;

        static MultiBinding()
        {
        }

        public Binding ProvideValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrWhiteSpace(StringFormat) && Converter == null)
            {
                throw new InvalidOperationException(
                    $"{nameof(MultiBinding)} requires a {nameof(Converter)} or {nameof(StringFormat)}");
            }

            SetProvideValue(serviceProvider);
            //Get the object that the markup extension is being applied to
            var provideValueTarget = (IProvideValueTarget) serviceProvider?.GetService(typeof(IProvideValueTarget));
            _target = provideValueTarget?.TargetObject as BindableObject;

            if (_target == null) return null;

            foreach (var b in Bindings)
            {
                if (b is Binding binding)
                {
                    var property = BindableProperty.Create($"Property-{Guid.NewGuid():N}", typeof(object),
                        typeof(MultiBinding), default(object), propertyChanged: (_, o, n) => SetValue());
                    _properties.Add(property);
                    _target.SetBinding(property, binding);
                }
                else if (b is string str)
                {
                    var property = BindableProperty.Create($"Property-{Guid.NewGuid():N}", typeof(string),
                        typeof(MultiBinding), default(string), propertyChanged: (_, o, n) => SetValue());
                    _properties.Add(property);
                    _target.SetValue(property, str);
                }
                else
                {
                    throw new ArgumentException("MultiBinding can only accept string or Binding arguments \\n" +
                                                $"Impossible to accept a value of type {b.GetType().Name}");
                }
            }

            SetValue();

            var multibinding = new Binding
            {
                Path = nameof(InternalValue.Value),
                Converter = new MultiValueConverterWrapper(Converter, StringFormat),
                ConverterParameter = ConverterParameter,
                Source = _internalValue
            };

            return multibinding;
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return ProvideValue(serviceProvider);
        }

        /// <summary>
        ///     Gets or sets the Bindings property.
        /// </summary>
        public IList<object> Bindings { get; } = new List<object>();

        /// <summary>
        ///     Gets or sets the StringFormat property.
        /// </summary>
        public string StringFormat { get; set; }

        /// <summary>
        ///     Gets or sets the IsLocalizable property.
        /// </summary>
        public bool IsLocalizable
        {
            get => _isLocalizable;
            set
            {
                _isLocalizable = value;
                if (value)
                {
                    Localize.Value.LocalizeChanged += UpdateProperty;
                }
                else
                {
                    Localize.Value.LocalizeChanged -= UpdateProperty;
                }
            }
        }

        /// <summary>
        ///     Gets or sets the Converter property.
        /// </summary>
        public IMultiValueConverter Converter { get; set; }

        /// <summary>
        ///     Gets or sets the ConverterParameter property.
        /// </summary>
        public object ConverterParameter { get; set; }

        /// <summary>
        ///     Subscribes to the <see cref="LocalizeBase.LocalizeChanged"/> event.
        /// </summary>
        public override void Subscribe()
        {
            Localize.Value.LocalizeChanged += UpdateProperty;
        }

        public override void Unsubscribe()
        {
            //_localize.LocalizeChanged -= UpdateProperty;
        }

        private void UpdateProperty(CultureInfo culture)
        {
            _cultureInfo = culture;
            SetValue();
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

        private void SetValue()
        {
            if (_target == null) return;
            if (IsLocalizable)
            {
                _internalValue.Value = _properties.Select(x =>
                {
                    if (x.ReturnType == typeof(string))
                    {
                        return GetTranslation(_target.GetValue(x).ToString());
                    }

                    return _target.GetValue(x);
                }).ToArray();
            }
            else
            {
                _internalValue.Value = _properties.Select(_target.GetValue).ToArray();
            }
        }

        private sealed class InternalValue : INotifyPropertyChanged
        {
            private object _value;

            public object Value
            {
                get => _value;
                set
                {
                    if (!Equals(_value, value))
                    {
                        _value = value;
                        OnPropertyChanged();
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private sealed class MultiValueConverterWrapper : IValueConverter
        {
            private readonly IMultiValueConverter _multiValueConverter;
            private readonly string _stringFormat;

            public MultiValueConverterWrapper(IMultiValueConverter multiValueConverter, string stringFormat)
            {
                _multiValueConverter = multiValueConverter;
                _stringFormat = stringFormat;
            }

            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (_multiValueConverter != null)
                {
                    value = _multiValueConverter.Convert(value as object[], targetType, parameter, culture);
                }

                if (!string.IsNullOrWhiteSpace(_stringFormat))
                {
                    if (value is object[] array)
                    {
                        value = string.Format(_stringFormat, array);
                    }
                    else
                    {
                        value = string.Format(_stringFormat, value);
                    }
                }

                return value;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
    }
}