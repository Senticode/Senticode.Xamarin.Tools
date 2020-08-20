using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Senticode.Xamarin.Tools.Core.Abstractions.Staff;
using Xamarin.Forms;

namespace _template.MasterDetail.Views.Animations
{
    internal class StartMenuItemPushAnimation : ExternalObjectAnimationBase
    {
        public override async Task Play(View sender)
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                try
                {
                    var num1 = await View.FadeTo(0.6, 80U) ? 1 : 0;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }
    }
}