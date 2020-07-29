using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Senticode.Xamarin.Tools.Core.Interfaces.Staff;
using Xamarin.Forms;

namespace Template.Blank.Staff.Controls
{
    /// <summary>
    ///     TODO EM: need refactor this
    /// </summary>
    public class LinkLabel : StackLayout
    {
        /// <summary>
        ///     Text property data.
        /// </summary>
        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(LinkLabel), default(string),
                BindingMode.TwoWay, null, HandleTextChanged);


        /// <summary>
        ///     Uri property data.
        /// </summary>
        public static readonly BindableProperty UriProperty =
            BindableProperty.Create(nameof(Uri), typeof(string), typeof(LinkLabel), default(string), BindingMode.TwoWay,
                null,
                HandleUriChanged);

        private readonly SimpleLinkLabel _label;
        private readonly BoxView _line;

        public LinkLabel()
        {
            Padding = new Thickness(Padding.Left, Padding.Top, Padding.Right, 0);
            VerticalOptions = LayoutOptions.Center;
            HorizontalOptions = LayoutOptions.Start;
            Children.Add(_label = new SimpleLinkLabel());
            _line = new BoxView
            {
                HeightRequest = 1,
                Margin = new Thickness(0, -8, 0, 0),
                BackgroundColor = _label.TextColor
            };
            //  Children.Add(_line);
            GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () =>
                {
                    IAnimation animationToPlay = new DefaultAnimation();
                    try
                    {
                        await animationToPlay.Play(this);
                        Device.OpenUri(_label.Uri);
                    }
                    catch
                    {
                        Debug.WriteLine("LinkLabel.StartAnimation Exception");
                    }
                })
            });
        }

        /// <summary>
        ///     Gets or sets the Text value.
        /// </summary>
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set
            {
                _label.Text = value;
                SetValue(TextProperty, value);
            }
        }

        public Color TextColor
        {
            get => (Color)_label.GetValue(Label.TextColorProperty);
            set
            {
                _label.SetValue(Label.TextColorProperty, value);
                _line.SetValue(BackgroundColorProperty, value);
            }
        }

        /// <summary>
        ///     Gets or sets the Uri value.
        /// </summary>
        public string Uri
        {
            get => (string)GetValue(UriProperty);
            set
            {
                if (value != null)
                {
                    _label.Uri = new Uri(value);
                    SetValue(UriProperty, value);
                }
            }
        }

        public TextAlignment HorizontalTextAlignment
        {
            get => _label.HorizontalTextAlignment;
            set => _label.HorizontalTextAlignment = value;
        }


        private static void HandleTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((LinkLabel)bindable).Text = newValue?.ToString();
        }

        private static void HandleUriChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            ((LinkLabel)bindable).Uri = newvalue?.ToString();
        }

        private class DefaultAnimation : IAnimation
        {
            public async Task Play(View sender)
            {
                try
                {
                    var num1 = await sender.ScaleTo(0.8, 50U) ? 1 : 0;
                    var num2 = await sender.ScaleTo(1.0, 50U) ? 1 : 0;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }

        public class SimpleLinkLabel : Label
        {
            /// <summary>
            ///     Uri property data.
            /// </summary>
            public static readonly BindableProperty UriProperty =
                BindableProperty.Create(nameof(Uri), typeof(Uri), typeof(SimpleLinkLabel), default(Uri));

            private readonly Color _textColor = Color.Blue;

            public SimpleLinkLabel()
            {
                TextColor = _textColor;
                VerticalTextAlignment = TextAlignment.Center;
            }

            /// <summary>
            ///     Gets or sets the Uri value.
            /// </summary>
            public Uri Uri
            {
                get => (Uri)GetValue(UriProperty);
                set => SetValue(UriProperty, value);
            }
        }
    }
}