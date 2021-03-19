using System;
using System.Diagnostics;
using Senticode.Xamarin.Tools.Core.DeviceOrientation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Senticode.Xamarin.Tools.Core.MarkupExtensions
{
    public enum TargetDevice
    {
        Phone = 0,
        Tablet = 1,
        TabletOrPhone = 2,
        All = 3
    }

    public class OnDeviceOrientationExtension : OnDeviceOrientationExtension<object>
    {
    }

    /// <summary>
    ///     Sets bindable property value based on device orientation.
    /// </summary>
    public class OnDeviceOrientationExtension<T> : WeakMarkupExtensionBase, IMarkupExtension<T>
    {
        private BindableObject _bo;
        private BindableProperty _property;

        public OnDeviceOrientationExtension()
        {
            TargetDevice = TargetDevice.All;
        }

        /// <summary>
        ///     Gets or sets the Landscape property.
        /// </summary>
        public T Landscape { get; set; }

        /// <summary>
        ///     Gets or sets the Portrait property.
        /// </summary>
        public T Portrait { get; set; }

        /// <summary>
        ///     Gets or sets the Default property.
        /// </summary>
        public T Default { get; set; }

        /// <summary>
        ///     Gets or sets the TargetDevice property.
        /// </summary>
        public TargetDevice TargetDevice { get; set; }

        public T ProvideValue(IServiceProvider serviceProvider)
        {
            SetProvideValue(serviceProvider);
            SetBindableObject(serviceProvider);
            SetProperty(DeviceOrientationInfo.Orientation);
            DeviceOrientationInfo.DeviceOrientationChanged += OnDeviceOrientation_Changed;
            return (T)_bo.GetValue(_property);
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return ProvideValue(serviceProvider);
        }

        /// <summary>
        ///     Subscribes to the <see cref="DeviceOrientationInfo.DeviceOrientationChanged" /> event.
        /// </summary>
        public override void Subscribe()
        {
            DeviceOrientationInfo.DeviceOrientationChanged += OnDeviceOrientation_Changed;
        }

        /// <summary>
        ///     Unsubscribes from the <see cref="DeviceOrientationInfo.DeviceOrientationChanged" /> event.
        /// </summary>
        public override void Unsubscribe()
        {
            DeviceOrientationInfo.DeviceOrientationChanged -= OnDeviceOrientation_Changed;
        }

        private void OnDeviceOrientation_Changed(DeviceOrientationChangedEventHandlerArgs args)
        {
            if (_bo == null || _property == null)
            {
                return;
            }

            SetProperty(args.Orientation);
        }

        private void SetProperty(DeviceOrientation.DeviceOrientation orientation)
        {
            if (_bo == null)
            {
                var ex = new NullReferenceException(
                    $"Parent bindable object is nod found for {nameof(OnDeviceOrientationExtension)}");
#if DEBUG
                throw ex;
#else
                return;
#endif
            }

            switch (TargetDevice)
            {
                case TargetDevice.Phone:
                    if (Device.Idiom != TargetIdiom.Phone)
                    {
                        _bo.SetValue(_property, Default);
                        return;
                    }

                    break;
                case TargetDevice.Tablet:
                    if (Device.Idiom != TargetIdiom.Tablet)
                    {
                        _bo.SetValue(_property, Default);
                        return;
                    }

                    break;
                case TargetDevice.TabletOrPhone:
                    if (Device.Idiom != TargetIdiom.Phone && Device.Idiom != TargetIdiom.Tablet)
                    {
                        _bo.SetValue(_property, Default);
                        return;
                    }

                    break;
                case TargetDevice.All:
                    break;
            }

            try
            {
                switch (orientation)
                {
                    case DeviceOrientation.DeviceOrientation.Landscape:
                        {
                            _bo.SetValue(_property, Landscape);
                            break;
                        }
                    case DeviceOrientation.DeviceOrientation.Portrait:
                        {
                            _bo.SetValue(_property, Portrait);
                            break;
                        }
                    default:
                        {
                            _bo.SetValue(_property, Default);
                            break;
                        }
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
                _bo.SetValue(_property, null);
            }
        }


        private void SetBindableObject(IServiceProvider serviceProvider)
        {
            var pvt = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
            _bo = pvt.TargetObject as BindableObject;
            _property = pvt.TargetProperty as BindableProperty;
        }
    }
}