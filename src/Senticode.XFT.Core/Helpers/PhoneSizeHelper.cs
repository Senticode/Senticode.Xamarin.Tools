using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Senticode.Xamarin.Tools.Core.Helpers
{
    public static class PhoneSizeHelper
    {
        public const int DefaultMiniOSDeviceWidth = 370;
        public const int DefaultMinAndroidDeviceWidth = 360;
        public const int DefaultNormaliOSDeviceWidth = 414;
        public const int DefaultNormalAndroidDeviceWidth = 420;

        public static int NormalAndroidDeviceWidth { get; set; } = DefaultNormalAndroidDeviceWidth;

        public static int MinAndroidDeviceWidth { get; set; } = DefaultMinAndroidDeviceWidth;

        public static int NormaliOSDeviceWidth { get; set; } = DefaultNormaliOSDeviceWidth;

        public static int MiniOSDeviceWidth { get; set; } = DefaultMiniOSDeviceWidth;

        public static PhoneSize GetPhoneSize()
        {
            var width = Math.Min(DeviceDisplay.MainDisplayInfo.Width, DeviceDisplay.MainDisplayInfo.Height) /
                        DeviceDisplay.MainDisplayInfo.Density;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    if (width <= MiniOSDeviceWidth)
                    {
                        return PhoneSize.Small;
                    }
                    else
                    {
                        return width <= NormaliOSDeviceWidth ? PhoneSize.Normal : PhoneSize.Big;
                    }
                case Device.Android:
                    if (width <= MinAndroidDeviceWidth)
                    {
                        return PhoneSize.Small;
                    }
                    else
                    {
                        return width <= NormalAndroidDeviceWidth ? PhoneSize.Normal : PhoneSize.Big;
                    }
                default:
                    return PhoneSize.Normal;
            }
        }
    }
}