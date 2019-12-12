using System;
using System.Globalization;
using Senticode.Xamarin.Tools.Core.Abstractions.Staff;

namespace Senticode.Xamarin.Tools.Core.Converters
{
    /// <summary>
    ///     Class to convert <c>double</c> to <c>int</c> and back.
    /// </summary>
    public class DoubleToIntConverter : ValueConverterBase
    {
        /// <summary>
        ///     Converts <c>double</c> to <c>int</c>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The type to which to convert the value.</param>
        /// <param name="parameter">A parameter to use during the conversion.</param>
        /// <param name="culture">The culture to use during the conversion.</param>
        /// <returns>Object.</returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var strParam = parameter?.ToString();
            double m = 1;

            if (!string.IsNullOrEmpty(strParam?.Trim()))
            {
                double.TryParse(strParam, out m);
            }

            return (int) Math.Round(double.Parse(value.ToString()) * m);
        }

        /// <summary>
        ///     Converts <c>int</c> to <c>double</c>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The type to which to convert the value.</param>
        /// <param name="parameter">A parameter to use during the conversion.</param>
        /// <param name="culture">The culture to use during the conversion.</param>
        /// <returns>Object.</returns>
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var strParam = parameter?.ToString();
            double d = 1;

            if (!string.IsNullOrEmpty(strParam?.Trim()))
            {
                double.TryParse(strParam, out d);
            }

            return double.Parse(value.ToString()) / d;
        }
    }
}