using System.Linq;
using Senticode.Base.Interfaces;
using Senticode.Xamarin.Tools.Core.Abstractions.StateMachine;
using Senticode.Xamarin.Tools.Core.Interfaces.Base.StateMachine;
using Unity;
using Xamarin.Essentials;

namespace Template.MasterDetail.AppStateMachine.Strategies
{
    internal class InternetConnectionStrategy : AppStrategy<AppState>
    {
        private readonly AppSettings _appSettings;

        public InternetConnectionStrategy(IUnityContainer container, AppSettings appSettings,
            IStateMachine<AppState, IStateTransformer<AppState>> stateMachine)
            : base(new StateTransformer<AppState>(x => Transform(stateMachine.State)), stateMachine)
        {
            _appSettings = appSettings;
            container.RegisterInstance(this);
        }

        /// <summary>
        ///     Translate changed action arguments to other services.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectivityOnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            StateMachine.DoNext(Transformer);
            _appSettings.NetworkAccess = e.NetworkAccess;
        }

        private static AppState Transform(AppState state)
        {
            state.NetworkAccess = Connectivity.NetworkAccess;
            state.NetworkProfile = Connectivity.ConnectionProfiles.FirstOrDefault();

            return state;
        }

        public override IResult Initialize()
        {
            return base.Initialize(() => { Connectivity.ConnectivityChanged += ConnectivityOnConnectivityChanged; });
        }

        public override IResult Release()
        {
            return base.Release(() => { Connectivity.ConnectivityChanged -= ConnectivityOnConnectivityChanged; });
        }
    }
}