using System.ComponentModel;
using Xamarin.Forms;

namespace Senticode.Xamarin.Tools.Core.DeviceOrientation
{
    /// <summary>
    ///     Class that monitors device orientation.
    /// </summary>
    public static class DeviceOrientationInfo
    {
        static DeviceOrientationInfo()
        {
            Device.Info.PropertyChanged += InfoOnPropertyChanged;
        }

        /// <summary>
        ///     Gets device orientation.
        /// </summary>
        public static DeviceOrientation Orientation
        {
            get
            {
                switch (Device.Info.CurrentOrientation)
                {
                    case global::Xamarin.Forms.Internals.DeviceOrientation.Landscape:
                    case global::Xamarin.Forms.Internals.DeviceOrientation.LandscapeRight:
                    case global::Xamarin.Forms.Internals.DeviceOrientation.LandscapeLeft:
                    {
                        return DeviceOrientation.Landscape;
                    }
                    case global::Xamarin.Forms.Internals.DeviceOrientation.PortraitUp:
                    case global::Xamarin.Forms.Internals.DeviceOrientation.PortraitDown:
                    case global::Xamarin.Forms.Internals.DeviceOrientation.Portrait:
                    {
                        return DeviceOrientation.Portrait;
                    }
                    default:
                    {
                        if (Device.Idiom == TargetIdiom.Phone)
                        {
                            return DeviceOrientation.Portrait;
                        }

                        if (Device.Idiom == TargetIdiom.Tablet)
                        {
                            return DeviceOrientation.Landscape;
                        }

                        return DeviceOrientation.Undefined;
                    }
                }
            }
        }

        private static void InfoOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName != "CurrentOrientation") return;

            switch (Device.Info.CurrentOrientation)
            {
                case global::Xamarin.Forms.Internals.DeviceOrientation.Landscape:
                case global::Xamarin.Forms.Internals.DeviceOrientation.LandscapeRight:
                case global::Xamarin.Forms.Internals.DeviceOrientation.LandscapeLeft:
                {
                    DeviceOrientationChanged?.Invoke(
                        new DeviceOrientationChangedEventHandlerArgs(DeviceOrientation.Landscape));
                    break;
                }
                case global::Xamarin.Forms.Internals.DeviceOrientation.PortraitUp:
                case global::Xamarin.Forms.Internals.DeviceOrientation.PortraitDown:
                case global::Xamarin.Forms.Internals.DeviceOrientation.Portrait:
                {
                    DeviceOrientationChanged?.Invoke(
                        new DeviceOrientationChangedEventHandlerArgs(DeviceOrientation.Portrait));
                    break;
                }
                default:
                {
                    DeviceOrientationChanged?.Invoke(
                        new DeviceOrientationChangedEventHandlerArgs(DeviceOrientation.Undefined));
                    break;
                }
            }
        }

        /// <summary>
        ///     Occurs when device orientation changed.
        /// </summary>
        public static event DeviceOrientationChangedEventHandler DeviceOrientationChanged;
    }
}