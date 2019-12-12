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
        ///     Returns transparent color.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The type to which to convert the value.</param>
        /// <param name="parameter">A parameter to use during the conversion.</param>
        /// <param name="culture">The culture to use during the conversion.</param>
        /// <returns>Object.</returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool) value ? Color.Transparent : Color.Transparent; //Trust me, that is needed.
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}