using System;
using System.Threading.Tasks;
using Senticode.Xamarin.Tools.Core.Interfaces.Staff;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Senticode.Xamarin.Tools.Core.Animations
{
    /// <summary>
    ///     Implements rotation animation.
    /// </summary>
    public class RotationAnimation : IAnimation, IMarkupExtension
    {
        /// <summary>
        ///     Gets or sets the RotateTo property.
        /// </summary>
        public double RotateTo { get; set; }

        /// <summary>
        ///     Gets or sets the RotateXTo property.
        /// </summary>
        public double RotateXTo { get; set; }

        /// <summary>
        ///     Gets or sets the RotateYTo property.
        /// </summary>
        public double RotateYTo { get; set; }

        /// <summary>
        ///     Gets or sets the Duration property.
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        ///     Starts the animation.
        /// </summary>
        public async Task Play(View sender)
        {
            await Task.WhenAll(
                sender.RotateTo(RotateTo, (uint) Duration),
                sender.RotateXTo(RotateXTo, (uint) Duration),
                sender.RotateYTo(RotateYTo, (uint) Duration)
            );
        }

        #region Implementation of IMarkupExtension

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        #endregion
    }
}