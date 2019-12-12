using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Senticode.Xamarin.Tools.Core.Animations
{
    /// <summary>
    ///     Implements animation when item tapped.
    /// </summary>
    public abstract class TappedAnimationBase : IMarkupExtension
    {
        /// <summary>
        ///     Gets or sets the Duration property.
        /// </summary>
        public uint Duration { get; set; } = 50;

        #region Implementation of IMarkupExtension

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        #endregion
    }

    /// <summary>
    ///     Starts animation.
    /// </summary>
    public class StartTappedAnimation : TappedAnimationBase
    {
        public async Task Play(View sender)
        {
            await sender.ScaleTo(0.8, Duration);
        }
    }

    public class EndTappedAnimation : TappedAnimationBase
    {
        public async Task Play(View sender)
        {
            await sender.ScaleTo(1, Duration);
        }
    }
}