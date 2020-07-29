using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace ProjectTemplateWizard.Converters
{
    internal class StringTrimmingConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value?.ToString();

            return string.IsNullOrWhiteSpace(str) ? null : str.Trim();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();

        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}