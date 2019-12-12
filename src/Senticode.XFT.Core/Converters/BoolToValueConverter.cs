using System;
using System.Globalization;
using Senticode.Xamarin.Tools.Core.Abstractions.Staff;

namespace Senticode.Xamarin.Tools.Core.Converters
{
    /// <summary>
    ///     Class to convert bool to object  and back.
    /// </summary>
    public class BoolToValueConverter : ValueConverterBase
    {
        /// <summary>
        ///     Gets or set the FalseValue property.
        /// </summary>
        public object FalseValue { get; set; }

        /// <summary>
        ///     Gets or sets the TrueValue property.
        /// </summary>
        public object TrueValue { get; set; }


        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool) value ? TrueValue : FalseValue;
        }


        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString() == TrueValue.ToString();
        }
    }
}