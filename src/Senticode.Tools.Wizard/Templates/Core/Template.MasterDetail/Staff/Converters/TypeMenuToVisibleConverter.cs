using System;
using System.Globalization;
using _template.MasterDetail.Commands.Navigation;
using Senticode.Xamarin.Tools.Core.Abstractions.Staff;

namespace _template.MasterDetail.Staff.Converters
{
    internal class TypeMenuToVisibleConverter : ValueConverterBase
    {
        #region Overrides of ValueConverterBase

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MenuKind)
            {
                return true;
            }

            return false;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();

        #endregion
    }
}