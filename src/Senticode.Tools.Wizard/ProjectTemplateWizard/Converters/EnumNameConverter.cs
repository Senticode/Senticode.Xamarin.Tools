using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using ProjectTemplateWizard.ExtensionMethods;

namespace ProjectTemplateWizard.Converters
{
    internal class EnumNameConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is Enum en ? en.GetDisplayName() : value;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();

        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}