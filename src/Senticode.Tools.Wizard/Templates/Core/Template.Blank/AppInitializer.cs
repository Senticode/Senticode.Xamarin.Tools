using System.Diagnostics;
using Senticode.Base.Interfaces;
using Senticode.Xamarin.Tools.Core.Interfaces.Base;
using Template.Blank.AppStateMachine.Strategies;
using Template.Blank.Resources;
using Template.Blank.ViewModels;
using Unity;
using Xamarin.Forms;

namespace Template.Blank
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
                    ;

                IsInitializing = false;
            }

            IsInitialized = true;
            return container;
        }
    }
}