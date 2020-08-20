using System;
using System.Globalization;
using Senticode.Xamarin.Tools.Core.Abstractions.Staff;
using Xamarin.Essentials;

namespace _template.Blank.Staff.Converters
{
    public class NetworkAccessToImageSourceConverter : ValueConverterBase
    {
        public const string ErrorState = "ErrorState.png";
        public const string GoodState = "GoodState.png";
        public const string WaitState = "WaitState.png";

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is NetworkAccess state)
            {
                switch (state)
                {
                    case NetworkAccess.None:
                        return ErrorState;
                    case NetworkAccess.Internet:
                        return GoodState;
                    case NetworkAccess.ConstrainedInternet:
                        return WaitState;
                    case NetworkAccess.Local:
                        return ErrorState;
                    case NetworkAccess.Unknown:
                        return WaitState;
                    default:
                        return ErrorState;
                }
            }

            throw new NotImplementedException();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}