using Xamarin.Forms;

namespace Senticode.Xamarin.Tools.Core.Abstractions.Staff
{
    public abstract class ExternalObjectAnimationBase : AnimationBase
    {
        #region View

        /// <summary>
        ///     Gets or sets the View value.
        /// </summary>
        public View View
        {
            get => (View) GetValue(ViewProperty);
            set => SetValue(ViewProperty, value);
        }

        /// <summary>
        ///     View property data.
        /// </summary>
        public static readonly BindableProperty ViewProperty =
            BindableProperty.Create(nameof(View), typeof(View), typeof(ExternalObjectAnimationBase),
                default(View));

        #endregion
    }
}