using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Senticode.Xamarin.Tools.Core.MarkupExtensions
{
    /// <summary>
    ///     A WeakTemplateSelector can be used to choose a DataTemplate at runtime based on the value of a data-bound property.
    /// </summary>
    public class WeakDataTemplateSelector : DataTemplateSelector
    {
        #region Overrides of DataTemplateSelector

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            throw new NotImplementedException();
        }

        #endregion

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
                    //    bo.PlatformSet += BoOnPlatformSet;
                }
            }
        }

        private void BoOnPlatformSet(object o, EventArgs eventArgs)
        {
            var element = o as Element;
            if (element is Page page)
            {
                page.Appearing += (sender, args) => { Subscribe(); };
                page.Disappearing += (sender, args) => { Unsubscribe(); };
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
                        perentpage.Appearing += (sender, args) => { Subscribe(); };
                        perentpage.Disappearing += (sender, args) => { Unsubscribe(); };
                    }
                }
            }
            //TODO EM: investigate it
         //   if (element != null) element.PlatformSet -= BoOnPlatformSet;
        }

        public virtual void Subscribe()
        {
        }

        public virtual void Unsubscribe()
        {
        }
    }
}