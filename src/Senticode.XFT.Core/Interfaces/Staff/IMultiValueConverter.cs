using System;
using System.Globalization;

namespace Senticode.Xamarin.Tools.Core.Interfaces.Staff
{
    /// <summary>
    ///     Provides a way to apply custom logic in a MultiBinding.
    /// </summary>
    public interface IMultiValueConverter
    {
        /// <summary>
        ///     Converts source values to a value for the binding target. 
        /// </summary>
        /// <param name="values">The array of values that the source bindings in the MultiBinding produces.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value.</returns>
        object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);

        /// <summary>
        ///     Converts a binding target value to the source binding values.
        /// </summary>
        /// <param name="value">The value that the binding target produces.</param>
        /// <param name="targetTypes">The array of types to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>An array of values that have been converted from the target value back to the source values.</returns>
        object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture);
    }
}