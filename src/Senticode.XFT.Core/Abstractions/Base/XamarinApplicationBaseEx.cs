using System;
using System.Diagnostics;
using Unity;
using Xamarin.Forms;

namespace Senticode.Xamarin.Tools.Core.Abstractions.Base
{
    public static class XamarinApplicationBaseEx
    {
        public static void SetMainPage<TPage, TViewModel>(this XamarinApplicationBase application)
            where TPage : Page
        {
            Debug.WriteLine("SetMainPage");
            //Set main page
            TPage page;
            if (application.Container.IsRegistered<TPage>())
            {
                page = (TPage) application.Container.Resolve(typeof(TPage));
            }
            else
            {
                page = (TPage) Activator.CreateInstance(typeof(TPage));
            }

            application.MainPage = page;
            application.Container.RegisterInstance(page);

            //Set main view model
            if (!application.Container.IsRegistered<TViewModel>())
            {
                application.Container.RegisterType<TViewModel>();
            }

            var viewModel = application.Container.Resolve<TViewModel>();

            application.MainPage.BindingContext = viewModel;

            application.Container.RegisterInstance(viewModel);
            //Set navigation
            application.Container.RegisterInstance(application.MainPage.Navigation);
        }
    }
}