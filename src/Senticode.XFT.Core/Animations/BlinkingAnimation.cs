using System;
using System.Threading.Tasks;
using Senticode.Xamarin.Tools.Core.Animations.Helpers;
using Senticode.Xamarin.Tools.Core.Interfaces.Staff;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Senticode.Xamarin.Tools.Core.Animations
{
    /// <summary>
    ///     Implements blinking animation.
    /// </summary>
    public class BlinkingAnimation : IAnimation, IMarkupExtension
    {
        /// <summary>
        ///     Gets or sets the ColorFrom property.
        /// </summary>
        public string ColorFrom { get; set; }

        /// <summary>
        ///     Gets or sets the ColorTo property.
        /// </summary>
        public string ColorTo { get; set; }

        /// <summary>
        ///     Starts animation.
        /// </summary>
        public async Task Play(View sender)
        {
            var br = GetFrame(sender);

            while (true)
            {
                await Task.Delay(400);
                await br.ColorTo(Color.FromHex(ColorFrom), Color.FromHex(ColorTo), c => br.BackgroundColor = c, 150);
            }
        }

        private Frame GetFrame(View sender)
        {
            if (!(sender is Frame br))
            {
                throw new ArgumentException("BlinkingAnimation supported yet only for Frame.");
            }

            return br;
        }

        #region Implementation of IMarkupExtension

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        #endregion
    }
}