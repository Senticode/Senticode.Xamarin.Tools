using System;
using System.Globalization;
using Senticode.Xamarin.Tools.Core.Abstractions.Staff;
using Xamarin.Forms;

namespace Senticode.Xamarin.Tools.Core.Converters
{
    /// <summary>
    ///     Class to convert <c>SelectedItemEventArgs</c> to selected item.
    /// </summary>
    public class SelectedItemEventArgsToSelectedItemConverter : ValueConverterBase
    {
        /// <summary>
        ///     Converts <c>SelectedItemEventArgs</c> to selected item.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The type to which to convert the value.</param>
        /// <param name="parameter">A parameter to use during the conversion.</param>
        /// <param name="culture">The culture to use during the conversion.</param>
        /// <returns>Object.</returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SelectedItemChangedEventArgs eventArgs)
            {
                return eventArgs.SelectedItem;
            }

            return null;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}