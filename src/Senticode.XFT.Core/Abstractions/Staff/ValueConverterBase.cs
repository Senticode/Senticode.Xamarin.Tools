using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Senticode.Xamarin.Tools.Core.Abstractions.Staff
{
    public abstract class ValueConverterBase : IValueConverter, IMarkupExtension<IValueConverter>
    {
        /// <summary>
        ///     Returns the object created from the markup extension.
        /// </summary>
        /// <param name="serviceProvider">The service that provides the value.</param>
        /// <returns>The object.</returns>
        public IValueConverter ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        /// <summary>
        ///     Returns the object created from the markup extension.
        /// </summary>
        /// <param name="serviceProvider">The service that provides the value.</param>
        /// <returns>The object.</returns>
        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return ProvideValue(serviceProvider);
        }

        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);


        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
    }
}