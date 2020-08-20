using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace _template.MasterDetail.Staff.Helpers
{
    internal class PhoneSizeHelper
    {
        public static PhoneSize GetPhoneSize()
        {
            var width = Math.Min(DeviceDisplay.MainDisplayInfo.Width, DeviceDisplay.MainDisplayInfo.Height) /
                        DeviceDisplay.MainDisplayInfo.Density;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    return width < 370 ? PhoneSize.Small : width < 414 ? PhoneSize.Normal : PhoneSize.Big;
                case Device.Android:
                    return width < 360 ? PhoneSize.Small : width < 420 ? PhoneSize.Normal : PhoneSize.Big;
                default:
                    return PhoneSize.Normal;
            }
        }
    }
}