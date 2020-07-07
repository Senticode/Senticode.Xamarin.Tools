using System;
using System.Globalization;
using Senticode.Xamarin.Tools.Core.Abstractions.Staff;
using Xamarin.Forms;

namespace Senticode.Xamarin.Tools.Core.Converters
{
    /// <summary>
    ///     Class to make color transparent if selected.
    /// </summary>
    public class SelectedToColorConverter : ValueConverterBase
    {
        /// <summary>
        ///     Color which returns when value is true.
        /// </summary>
        public Color Color { get; set; } = Color.Transparent;

        /// <summary>
        ///     Returns Color value = when true or transparent color when value = false.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The type to which to convert the value.</param>
        /// <param name="parameter">A parameter to use during the conversion.</param>
        /// <param name="culture">The culture to use during the conversion.</param>
        /// <returns>Object.</returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool) value ? Color : Color.Transparent;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Color color && color == Color;
        }
    }
}