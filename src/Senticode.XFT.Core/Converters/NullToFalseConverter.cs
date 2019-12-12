using System;
using System.Globalization;
using Senticode.Xamarin.Tools.Core.Abstractions.Staff;

namespace Senticode.Xamarin.Tools.Core.Converters
{
    /// <summary>
    ///     Class to convert null value to false.
    /// </summary>
    public class NullToFalseConverter : ValueConverterBase
    {
        /// <summary>
        ///     Returns false if <paramref name="value"/> is null.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The type to which to convert the value.</param>
        /// <param name="parameter">A parameter to use during the conversion.</param>
        /// <param name="culture">The culture to use during the conversion.</param>
        /// <returns>Object.</returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}