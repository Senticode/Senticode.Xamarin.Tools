using System;
using System.Globalization;
using Senticode.Xamarin.Tools.Core.Abstractions.Staff;
using Xamarin.Forms;

namespace Senticode.Xamarin.Tools.Core.Converters
{
    /// <summary>
    ///     Class to set content for the control.
    /// </summary>
    [ContentProperty(nameof(Default))]
    public class IsNullToDefaultValueConvertor : ValueConverterBase
    {
        public object Default { get; set; }

        /// <summary>
        ///     Converts null to content.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The type to which to convert the value.</param>
        /// <param name="parameter">A parameter to use during the conversion.</param>
        /// <param name="culture">The culture to use during the conversion.</param>
        /// <returns>Object.</returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value ?? Default;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}