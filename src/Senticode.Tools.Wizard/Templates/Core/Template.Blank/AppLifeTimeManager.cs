using System;
using System.Diagnostics;
using _template.Blank.AppStateMachine;
using _template.Blank.AppStateMachine.Strategies;
using _template.Blank.ViewModels;
using _template.Blank.Views;
using Senticode.Base.Interfaces;
using Senticode.Xamarin.Tools.Core.Abstractions.Base;
using Senticode.Xamarin.Tools.Core.Abstractions.StateMachine;
using Senticode.Xamarin.Tools.Core.Configuration;
using Unity;
using Xamarin.Forms;

namespace _template.Blank
{
    public class AppLifeTimeManager : AppLifeTimeManagerBase<AppSettings, AppCommands, AppState>
    {
        public AppLifeTimeManager(IUnityContainer container) : base(container)
        {
            if (Device.RuntimePlatform == Device.Android ||
                Device.RuntimePlatform == Device.iOS ||
                Device.RuntimePlatform == Device.UWP)
            {
                Strategies.Add(container.Resolve<InternetConnectionStrategy>());
            }
        }

        protected override async void OnExecute(AppLifeTimeState state)
        {
            AppSateMachine.DoNext(new StateTransformer<AppState>(appState => appState.AppLifeTimeState = state));

            switch (state)
            {
                case AppLifeTimeState.FirstStart:
                    await AppSettings.LoadAsync();
                    Initialize();
                    SetMainPage();
                    break;
                case AppLifeTimeState.Sleep:
                    await AppSettings.SaveAsync();
                    break;
            }
        }

        public void SetMainPage()
        {
            CurrentApplication.SetMainPage<MainPage, MainViewModel>();
        }

        #region Release

        public override IResult Release()
        {
            var baseResult = base.Release();

            return baseResult.IsSuccessful ? Strategies.Release() : baseResult;
        }

        #endregion

        #region Initialize

        public override IResult Initialize()
        {
            InitializeAppConfig();
            InitializeModelController();
            var baseResult = base.Initialize();
            return baseResult.IsSuccessful ? Strategies.Initialize() : baseResult;
        }

        private void InitializeAppConfig()
        {
            try
            {
                var configPath = $"{GetType().Assembly.GetName().Name}.{AppSettings.ConfigFile}";
                using (var config = GetType().Assembly.GetManifestResourceStream(configPath))
                {
                    ConfigurationManager.Initialize(config);
                }

                if (AppSettings.WebServiceAddress == null)
                {
                    AppSettings.WebServiceAddress = new Uri(ConfigurationManager.AppSettings["webservice.address"]);
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }

        private void InitializeModelController()
        {
        }

        #endregion
    }
}