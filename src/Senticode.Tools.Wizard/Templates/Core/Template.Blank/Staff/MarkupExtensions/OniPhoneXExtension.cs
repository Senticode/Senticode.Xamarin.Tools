using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace _template.Blank.Staff.MarkupExtensions
{
    public class OniPhoneXExtension<T> : BindableObject, IMarkupExtension<T>
    {
        #region Default

        /// <summary>
        ///     Gets or sets the Default value.
        /// </summary>
        public T Default
        {
            get => (T) GetValue(DefaultProperty);
            set => SetValue(DefaultProperty, value);
        }

        /// <summary>
        ///     Default property data.
        /// </summary>
        public static readonly BindableProperty DefaultProperty =
            BindableProperty.Create(nameof(Default), typeof(T), typeof(OniPhoneXExtension<T>), default(T));

        #endregion


        #region XValue

        /// <summary>
        ///     Gets or sets the XValue value.
        /// </summary>
        public T XValue
        {
            get => (T) GetValue(XValueProperty);
            set => SetValue(XValueProperty, value);
        }

        /// <summary>
        ///     XValue property data.
        /// </summary>
        public static readonly BindableProperty XValueProperty =
            BindableProperty.Create(nameof(XValue), typeof(T), typeof(OniPhoneXExtension<T>), default(T));

        #endregion

        #region Implementation of IMarkupExtension

        public T ProvideValue(IServiceProvider serviceProvider)
        {
            if (Device.Idiom == TargetIdiom.Phone && Device.RuntimePlatform == Device.iOS)
            {
                if (DeviceInfo.Name.Contains("iPhone X") || DeviceInfo.Name.Contains("iPhone 11"))
                {
                    return XValue;
                }

                return Default;
            }

            return Default;
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => ProvideValue(serviceProvider);

        #endregion
    }
}