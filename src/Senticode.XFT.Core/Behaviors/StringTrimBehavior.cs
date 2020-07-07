using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace Senticode.Xamarin.Tools.Core.Behaviors
{
    /// <summary>
    ///     Behavior to limit number of characters in an entry.
    /// </summary>
    public class StringTrimBehavior : Behavior<Entry>
    {
        public StringTrimBehavior()
        {
            MaxValue = 0;
        }
        private Entry _entry;
        /// <summary>
        /// Gets or sets the MaxValue value.
        /// </summary>
        public int MaxValue
        {
            get => (int)GetValue(MaxValueProperty);
            set => SetValue(MaxValueProperty, value);
        }

        /// <summary>
        /// MaxValue property data.
        /// </summary>
        public static readonly BindableProperty MaxValueProperty =
            BindableProperty.Create(nameof(MaxValue), typeof(int), typeof(NumericEntryBehavior), default(int));

        protected override void OnAttachedTo(Entry bindable)
        {
            _entry = bindable;
            _entry.BindingContextChanged += (sender, args) =>
            {
                BindingContext = _entry.BindingContext;
            };
            AttachHandler(true);
            base.OnAttachedTo(bindable);
        }



        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);
            AttachHandler(false);
        }

        /// <summary>
        /// Attaches or detaches the event handlers.
        /// </summary>
        /// <param name="value"></param>
        private void AttachHandler(bool value)
        {
            if (_entry == null) return;
            if (value)
            {
                _entry.TextChanged += Entry_TextChanged;
            }
            else
            {
                _entry.TextChanged -= Entry_TextChanged;
            }
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            try
            {
                if (textChangedEventArgs.NewTextValue.Length > MaxValue && MaxValue != 0)
                {
                    _entry.Text = textChangedEventArgs.OldTextValue;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        #region Overrides of BindableObject

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            AttachHandler(true);
        }

        #endregion

      


    }
}
