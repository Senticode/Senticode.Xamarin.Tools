using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace Senticode.Xamarin.Tools.Core.Behaviors
{
    /// <summary>
    ///     Behavior for entry that expects numbers.
    /// </summary>
    public class NumericEntryBehavior : Behavior<Entry>
    {
        private Entry _entry;

        /// <summary>
        /// Gets or sets the IsEnabled value.
        /// </summary>
        public bool IsEnabled
        {
            get => (bool)GetValue(IsEnabledProperty);
            set => SetValue(IsEnabledProperty, value);
        }

        /// <summary>
        /// IsEnabled property data.
        /// </summary>
        public static readonly BindableProperty IsEnabledProperty =
            BindableProperty.Create(nameof(IsEnabled), typeof(bool), typeof(NumericEntryBehavior), default(bool));

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
            AttachHandler(IsEnabled);
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
                _entry.Completed += Entry_Completed;
            }
            else
            {
                _entry.TextChanged -= Entry_TextChanged;
                _entry.Completed -= Entry_Completed;
            }
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            try
            {
                bool isEnabled;
                lock (textChangedEventArgs)
                {
                    isEnabled = IsEnabled;
                }
                if (isEnabled)
                {
                    if (textChangedEventArgs.NewTextValue != null && !IsInteger(textChangedEventArgs.NewTextValue))
                    {
                        ((Entry)sender).Text = textChangedEventArgs.OldTextValue;
                        return;
                    }

                    if ((MaxValue > 0 && double.TryParse(textChangedEventArgs.NewTextValue, out var value)) && value > MaxValue)
                    {
                        ((Entry)sender).Text = textChangedEventArgs.OldTextValue;
                    }
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
            AttachHandler(IsEnabled);
        }

        #endregion

        private void Entry_Completed(object sender, EventArgs eventArgs)
        {
            _entry.Text = int.Parse(_entry.Text).ToString();
        }

        /// <summary>
        /// Condition on the Integer.
        /// </summary>
        private bool IsInteger(string input)
        {
            if (input == null)
            {
                return false;
            }
            foreach (var c in input)
            {
                if (c < '0' || c > '9')
                {
                    return false;
                }
            }
            return true;
        }

    }
}
