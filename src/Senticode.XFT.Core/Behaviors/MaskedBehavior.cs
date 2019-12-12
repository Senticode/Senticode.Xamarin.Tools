using System.Collections.Generic;
using Xamarin.Forms;

namespace Senticode.Xamarin.Tools.Core.Behaviors
{
    /// <summary>
    ///     This behavior can used for set mask on an entry field, as way to format the input into something more human
    ///     readable.
    ///     <remarks>
    ///         <code lang = "C#"><![CDATA[
    /// <Entry Keyboard = "Numeric">
    ///     <Entry.Behaviors>
    ///         <behavior:MaskedBehavior Mask = "(XXX) XXX-XXX"/>
    ///     </Entry.Behaviors>
    /// </Entry >
    ///              ]]></code>
    ///     </remarks>
    /// </summary>
    public class MaskedBehavior : BehaviorBase<Entry>
    {

        private IDictionary<int, char> _positions;

        #region Mask
        /// <summary>
        /// Get or set Mask of string on an entry field.
        /// </summary>
        public string Mask
        {
            get => (string)GetValue(MaskProperty);
            set => SetValue(MaskProperty, value);
        }

        /// <summary>
        /// Mask property data.
        /// </summary>
        public static readonly BindableProperty MaskProperty =
            BindableProperty.Create(nameof(Mask), typeof(string), typeof(MaskedBehavior), default(string), BindingMode.Default, null, OnMaskPropertyChanged);

        private static void OnMaskPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is MaskedBehavior behavior)
            {
                behavior.SetPositions();
            }
        }
        #endregion

        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        protected void SetPositions()
        {
            if (string.IsNullOrEmpty(Mask))
            {
                _positions = null;
                return;
            }

            var list = new Dictionary<int, char>();
            for (var i = 0; i < Mask.Length; i++)
            {
                if (Mask[i] != 'X')
                {
                    list.Add(i, Mask[i]);
                }
            }
            _positions = list;
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            if (sender is Entry entry)
            {
                var text = entry.Text;

                if (string.IsNullOrWhiteSpace(text) || _positions == null)
                {
                    return;
                }

                if (text.Length > Mask.Length)
                {
                    entry.Text = text.Remove(text.Length - 1);
                    return;
                }

                foreach (var position in _positions)
                {
                    if (text.Length >= position.Key + 1)
                    {
                        var value = position.Value.ToString();
                        if (text.Substring(position.Key, 1) != value)
                        {
                            text = text.Insert(position.Key, value);
                        }
                    }
                }

                if (entry.Text != text)
                {
                    entry.Text = text;
                }
            }
        }
    }
}