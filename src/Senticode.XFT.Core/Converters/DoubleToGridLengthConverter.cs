using System;
using System.Globalization;
using Senticode.Xamarin.Tools.Core.Abstractions.Staff;
using Xamarin.Forms;

namespace Senticode.Xamarin.Tools.Core.Converters
{
    /// <summary>
    ///     Class to convert double to GridLength Struct and back.
    /// </summary>
    public class DoubleToGridLengthConverter : ValueConverterBase
    {
        /// <summary>
        ///     Converts double to GridLength Struct.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The type to which to convert the value.</param>
        /// <param name="parameter">A parameter to use during the conversion.</param>
        /// <param name="culture">The culture to use during the conversion.</param>
        /// <returns>Object.</returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value as double? > 0 ? new GridLength((double) value) : GridLength.Auto;
        }

        /// <summary>
        ///     Converts GridLength Struct to double.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The type to which to convert the value.</param>
        /// <param name="parameter">A parameter to use during the conversion.</param>
        /// <param name="culture">The culture to use during the conversion.</param>
        /// <returns>Object.</returns>
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as GridLength?)?.Value ?? double.NaN;
        }
    }
}