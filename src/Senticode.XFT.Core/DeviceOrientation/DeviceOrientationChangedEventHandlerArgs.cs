namespace Senticode.Xamarin.Tools.Core.DeviceOrientation
{
    /// <summary>
    ///     Arguments for the DeviceOrientationChanged event
    /// </summary>
    public class DeviceOrientationChangedEventHandlerArgs
    {
        public DeviceOrientationChangedEventHandlerArgs(DeviceOrientation orientation)
        {
            Orientation = orientation;
        }

        /// <summary>
        ///     Gets the Orientation property.
        /// </summary>
        public DeviceOrientation Orientation { get; }
    }
}