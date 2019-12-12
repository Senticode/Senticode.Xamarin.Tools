using System;
using System.Globalization;
using Senticode.Xamarin.Tools.Core.Abstractions.Staff;

namespace Senticode.Xamarin.Tools.Core.Converters
{
    /// <summary>
    ///     Class to make string uppercase and back.
    /// </summary>
    public class UpCaseStringConverter : ValueConverterBase
    {
        /// <summary>
        ///     Makes string uppercase.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The type to which to convert the value.</param>
        /// <param name="parameter">A parameter to use during the conversion.</param>
        /// <param name="culture">The culture to use during the conversion.</param>
        /// <returns>Object.</returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((string) value)?.ToUpper();
        }

        /// <summary>
        ///     Makes string lowercase.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The type to which to convert the value.</param>
        /// <param name="parameter">A parameter to use during the conversion.</param>
        /// <param name="culture">The culture to use during the conversion.</param>
        /// <returns>Object.</returns>
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((string) value)?.ToLower();
        }
    }
}