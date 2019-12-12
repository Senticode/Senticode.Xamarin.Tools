using System;
using System.Linq;
using System.Threading.Tasks;
using Unity;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Senticode.Xamarin.Tools.Core.MarkupExtensions
{
    public abstract class WeakMarkupExtensionBase
    {
        /// <summary>
        ///     Gets or sets the WeakReferenceProperty Page value.
        /// </summary>
        private Page Page { get; set; }

        protected void SetProvideValue(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
            var pvt = (IProvideValueTarget) serviceProvider.GetService(typeof(IProvideValueTarget));

            if (pvt.TargetObject is Element bo)
            {
                if (bo is GestureRecognizer gestureRecognizer)
                {
                    new TaskFactory().StartNew(async () =>
                    {
                        await Task.Delay(100);
                        BoOnPlatformSet(gestureRecognizer, new EventArgs());
                    });
                }
                else
                {
                    //TODO EM: investigate it
                    //  bo.PlatformSet += BoOnPlatformSet;
                }
            }
        }

        private void BoOnPlatformSet(object o, EventArgs eventArgs)
        {
            var element = o as Element;
            if (element is Page page)
            {
                Page = page;
                Page.Appearing += PerentpageOnAppearing;
                Page.Disappearing += PerentpageOnDisappearing;
            }
            else
            {
                if (element != null)
                {
                    var parent = element.Parent;
                    while (!(parent is Page) && parent != null)
                    {
                        parent = parent.Parent;
                    }

                    if (parent != null && parent is Page perentpage)
                    {
                        Page = perentpage;
                        Page.Appearing += PerentpageOnAppearing;
                        Page.Disappearing += PerentpageOnDisappearing;
                    }
                }
            }
            //TODO EM: investigate it
            // if (element != null) element.PlatformSet -= BoOnPlatformSet;
        }

        private async void PerentpageOnDisappearing(object o, EventArgs eventArgs)
        {
            Unsubscribe();
            await TryFinalUnsubscribe();
        }

        private void PerentpageOnAppearing(object o, EventArgs eventArgs)
        {
            Subscribe();
        }

        public virtual void Subscribe()
        {
        }

        public virtual void Unsubscribe()
        {
        }

        /// <summary>
        ///     Makes the final unsubscribes from all known event handlers.
        /// </summary>
        /// <returns></returns>
        private async Task TryFinalUnsubscribe()
        {
            await Task.Delay(500);

            if (Page == null)
            {
                return;
            }

            var isUnknown = ServiceLocator.Container.Resolve<INavigation>().NavigationStack
                                .FirstOrDefault(page => page.Id == Page.Id) != null;
            if (!isUnknown)
            {
                isUnknown = ServiceLocator.Container.Resolve<INavigation>().ModalStack
                                .FirstOrDefault(page => page.Id == Page.Id) != null;
            }

            if (!isUnknown)
            {
                isUnknown = ServiceLocator.Container.IsRegistered(Page?.GetType());
            }

            if (!isUnknown)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var tmpPage = Page;
                    if (tmpPage != null)
                    {
                        tmpPage.Appearing -= PerentpageOnAppearing;
                        tmpPage.Disappearing -= PerentpageOnDisappearing;
                        Page = null;
                    }
                });
            }
        }
    }
}