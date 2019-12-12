using System;
using System.Globalization;
using System.Windows.Input;
using Senticode.Xamarin.Tools.Core.Abstractions.Staff;

namespace Senticode.Xamarin.Tools.Core.Converters
{
    /// <summary>
    ///     Class to convert the <c>ICommand.CanExecute()</c> method to bool.
    /// </summary>
    public class CanExecuteCommandToVisibleConverter : ValueConverterBase
    {
        /// <summary>
        ///     Converts the <c>ICommand.CanExecute()</c> method to bool.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The type to which to convert the value.</param>
        /// <param name="parameter">A parameter to use during the conversion.</param>
        /// <param name="culture">The culture to use during the conversion.</param>
        /// <returns>Object.</returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value != null && ((ICommand) value).CanExecute(parameter))
                {
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}