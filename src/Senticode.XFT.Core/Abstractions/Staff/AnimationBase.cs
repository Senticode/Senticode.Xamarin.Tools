using System;
using System.Threading.Tasks;
using Senticode.Xamarin.Tools.Core.Interfaces.Staff;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Senticode.Xamarin.Tools.Core.Abstractions.Staff
{
    public abstract class AnimationBase: BindableObject, IAnimation, IMarkupExtension
    {

        public abstract Task Play(View sender);
        
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
