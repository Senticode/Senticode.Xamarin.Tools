using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Senticode.Xamarin.Tools.Core.Abstractions.Staff;
using Xamarin.Forms;

namespace Template.MasterDetail.Views.Animations
{
    internal class EndMenuItemPushAnimation : ExternalObjectAnimationBase
    {
        public override async Task Play(View sender)
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                try
                {
                    var num2 = await View.FadeTo(1, 20U) ? 1 : 0;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }
    }
}