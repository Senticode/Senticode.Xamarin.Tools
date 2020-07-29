using System;
using System.Globalization;
using Senticode.Xamarin.Tools.Core.Abstractions.Staff;
using Template.MasterDetail.Commands.Navigation;

namespace Template.MasterDetail.Staff.Converters
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

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}