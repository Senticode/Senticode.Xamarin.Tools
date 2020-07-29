using System;
using Template.Blank.Staff.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Template.Blank.Staff.MarkupExtensions
{
    public class OnPhoneSizeExtension<T> : BindableObject, IMarkupExtension<T>
    {
        #region Implementation of IMarkupExtension

        #endregion

        #region SmallSizeValue

        /// <summary>
        ///     Gets or sets the SmallSizeValue value.
        /// </summary>
        public T SmallSizeValue
        {
            get => (T)GetValue(SmallSizeValueProperty);
            set => SetValue(SmallSizeValueProperty, value);
        }

        /// <summary>
        ///     SmallSizeValue property data.
        /// </summary>
        public static readonly BindableProperty SmallSizeValueProperty =
            BindableProperty.Create(nameof(SmallSizeValue), typeof(T), typeof(OnPhoneSizeExtension<T>));

        #endregion

        #region DefaultSizeValue

        /// <summary>
        ///     Gets or sets the DefaultSizeValue value.
        /// </summary>
        public T DefaultSizeValue
        {
            get => (T)GetValue(DefaultSizeValueProperty);
            set => SetValue(DefaultSizeValueProperty, value);
        }

        /// <summary>
        ///     DefaultSizeValue property data.
        /// </summary>
        public static readonly BindableProperty DefaultSizeValueProperty =
            BindableProperty.Create(nameof(DefaultSizeValue), typeof(T), typeof(OnPhoneSizeExtension<T>));

        #endregion

        #region BigSizeValue

        /// <summary>
        ///     Gets or sets the BigSizeValue value.
        /// </summary>
        public T BigSizeValue
        {
            get => (T)GetValue(BigSizeValueProperty);
            set => SetValue(BigSizeValueProperty, value);
        }

        /// <summary>
        ///     BigSizeValue property data.
        /// </summary>
        public static readonly BindableProperty BigSizeValueProperty =
            BindableProperty.Create(nameof(BigSizeValue), typeof(T), typeof(OnPhoneSizeExtension<T>));


        #endregion

        #region Implementation of IMarkupExtension

        public T ProvideValue(IServiceProvider serviceProvider)
        {
            var size = PhoneSizeHelper.GetPhoneSize();
            switch (size)
            {
                case PhoneSize.Small:
                    return IsSet(SmallSizeValueProperty) ? SmallSizeValue : DefaultSizeValue;
                case PhoneSize.Normal:
                    return DefaultSizeValue;
                case PhoneSize.Big:
                    return IsSet(BigSizeValueProperty) ? BigSizeValue : DefaultSizeValue;
                default:
                    return DefaultSizeValue;
            }
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return ProvideValue(serviceProvider);
        }

        #endregion
    }
}