using Senticode.Xamarin.Tools.Core.Animations.Helpers;
using Senticode.Xamarin.Tools.Core.Interfaces.Staff;
using Xamarin.Forms;

namespace Senticode.Xamarin.Tools.Core.Behaviors
{
    /// <summary>
    ///     Behavior to add animations.
    /// </summary>
    public class AnimationBehavior : Behavior<View>
    {
        /// <summary>
        ///     Gets or sets the Animation property.
        /// </summary>
        public IAnimation Animation { get; set; }

        private void StartAnimation(View bindable)
        {
            Device.BeginInvokeOnMainThread(async () => await Animation.Play(bindable));
        }

        private void StopAnimation(View bindable)
        {
            ViewExtensions.CancelAnimations(bindable);
            ViewExtensionsEx.CancelAnimation(bindable);
        }

        protected override void OnAttachedTo(View bindable)
        {
            base.OnAttachedTo(bindable);
            // Perform setup
            StartAnimation(bindable);
        }

        protected override void OnDetachingFrom(View bindable)
        {
            base.OnDetachingFrom(bindable);
            StopAnimation(bindable);
        }
    }
}