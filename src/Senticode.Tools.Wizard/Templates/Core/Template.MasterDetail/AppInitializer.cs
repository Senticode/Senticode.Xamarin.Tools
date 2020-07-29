using System.Diagnostics;
using Senticode.Base.Interfaces;
using Senticode.Xamarin.Tools.Core.Interfaces.Base;
using Template.MasterDetail.AppStateMachine.Strategies;
using Template.MasterDetail.Resources;
using Template.MasterDetail.ViewModels;
using Template.MasterDetail.ViewModels.Menu;
using Template.MasterDetail.Views.Menu;
using Template.MasterDetail.Views.Pages;
using Unity;
using Xamarin.Forms;

namespace Template.MasterDetail
{
    internal class AppInitializer : IInitializer, IInitializationTrigger
    {
        public static AppInitializer Instance { get; } = new AppInitializer();

        public bool IsInitialized { get; set; }

        public bool IsInitializing { get; set; }

        public bool IsReleasing { get; set; }

        public IUnityContainer Initialize(IUnityContainer container)
        {
            if (!IsInitialized)
            {
                IsInitializing = true;
                Debug.WriteLine("RegisterTypes");

                //0.Strategies
                if (Device.RuntimePlatform == Device.Android ||
                    Device.RuntimePlatform == Device.iOS ||
                    Device.RuntimePlatform == Device.UWP)
                {
                    container.RegisterType<InternetConnectionStrategy>();
                }

                //1. Register modules                

                ResourcesInitializer.Instance.Initialize(container);

                container
                    //2. Services
                    .RegisterType<ModelController>()

                    //3. ViewModels
                    .RegisterType<MainViewModel>()
                    .RegisterType<MainMenuViewModel>()
                    .RegisterType<AboutMenuViewModel>()
                    .RegisterType<SettingsViewModel>()

                    //4. Views
                    .RegisterType<HomePage>()
                    .RegisterType<AboutMenu>()
                    .RegisterType<SettingsPage>()
                    ;

                IsInitializing = false;
            }

            IsInitialized = true;
            return container;
        }
    }
}