using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Senticode.Xamarin.Tools.MVVM.Validations.Converters
{
    /// <summary>
    ///     Shows first of validation errors.
    /// </summary>
    public class FirstValidationErrorConverter : IValueConverter, IMarkupExtension<IValueConverter>
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is ICollection<ErrorInfo> errors && errors.Count > 0 ? errors.ElementAt(0).Message : "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #region Implementation of IMarkupExtension

        public IValueConverter ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return ProvideValue(serviceProvider);
        }

        #endregion
    }
}