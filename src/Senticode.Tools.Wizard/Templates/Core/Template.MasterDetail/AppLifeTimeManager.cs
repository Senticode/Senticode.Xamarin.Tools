using System;
using System.Diagnostics;
using Senticode.Base.Interfaces;
using Senticode.Xamarin.Tools.Core.Abstractions.Base;
using Senticode.Xamarin.Tools.Core.Abstractions.StateMachine;
using Senticode.Xamarin.Tools.Core.Configuration;
using Template.MasterDetail.AppStateMachine;
using Template.MasterDetail.AppStateMachine.Strategies;
using Template.MasterDetail.Commands.Navigation;
using Template.MasterDetail.ViewModels;
using Template.MasterDetail.Views;
using Unity;
using Xamarin.Forms;

namespace Template.MasterDetail
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
            var command = Container.Resolve<NavigateToPageCommand>();
            command.Execute(PageKind.Home);
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
                var configPath = $"{GetType().Assembly.GetName().Name}.{AppSettings.CONFIG_FILE}";
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