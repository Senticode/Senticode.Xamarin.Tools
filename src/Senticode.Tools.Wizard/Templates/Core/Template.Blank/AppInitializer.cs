using System.Diagnostics;
using _template.Blank.AppStateMachine.Strategies;
using _template.Blank.Resources;
using _template.Blank.ViewModels;
using Senticode.Base.Interfaces;
using Senticode.Xamarin.Tools.Core.Interfaces.Base;
using Unity;
using Xamarin.Forms;

namespace _template.Blank
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