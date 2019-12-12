using System;
using System.Collections.Generic;
using Senticode.Base.Services;
using Senticode.Xamarin.Tools.Core.Abstractions.StateMachine;
using Senticode.Xamarin.Tools.Core.Interfaces.Base;
using Senticode.Xamarin.Tools.Core.Interfaces.Base.StateMachine;
using Unity;
using Xamarin.Forms;

namespace Senticode.Xamarin.Tools.Core.Abstractions.Base
{
    public abstract class
        AppLifeTimeManagerBase<TAppSettings, TAppCommands, TAppState> : AppLifeTimeManagerBase<TAppSettings,
            TAppCommands>
        where TAppCommands : AppCommandsBase<TAppSettings>
        where TAppSettings : AppSettingsBase
        where TAppState : AppStateBase, new()
    {
        protected AppLifeTimeManagerBase(IUnityContainer container) : base(container)
        {
            if (!container.IsRegistered<AppSateMachineBase<TAppState>>())
            {
                container.RegisterType<AppSateMachineBase<TAppState>>();
            }

            AppSateMachine = container.Resolve<AppSateMachineBase<TAppState>>();
            container.RegisterInstance(AppSateMachine);
        }

        protected IStateMachine<TAppState, IStateTransformer<TAppState>> AppSateMachine { get; }

        public TAppState AppState => AppSateMachine.State;

        protected AppStrategyCollection<TAppState> Strategies { get; } = new AppStrategyCollection<TAppState>();
    }


    public abstract class
        AppLifeTimeManagerBase<TAppSettings, TAppCommands> : ServiceBase, IAppComponentLocator<TAppSettings, TAppCommands>
        where TAppCommands : AppCommandsBase<TAppSettings>
        where TAppSettings : AppSettingsBase
    {
        private readonly List<PageLogObject> _history = new List<PageLogObject>();

        private bool _isFirstStart = true;

        protected AppLifeTimeManagerBase(IUnityContainer container)
        {
            Container = container;
            container.RegisterInstance(this);
        }

        public IReadOnlyList<PageLogObject> History => _history;

        protected XamarinApplicationBase CurrentApplication => (XamarinApplicationBase) Application.Current;

        public AppLifeTimeState CurrentState { get; private set; }

        /// <summary>
        ///     Gets the IoC Container.
        /// </summary>
        public IUnityContainer Container { get; }


        /// <summary>
        ///     Gets the AppSettings property.
        /// </summary>
        public TAppSettings AppSettings => Container.Resolve<TAppSettings>();

        /// <summary>
        ///     Gets the AppCommands property.
        /// </summary>
        public TAppCommands AppCommands => Container.Resolve<TAppCommands>();

        protected internal virtual void OnResume()
        {
            CurrentState = AppLifeTimeState.Resume;
            OnExecute(CurrentState);
        }

        protected internal virtual async void OnSleep()
        {
            CurrentState = AppLifeTimeState.Sleep;
            await AppSettings.SaveAsync();
            OnExecute(CurrentState);
        }

        protected internal virtual async void OnStart()
        {
            if (_isFirstStart)
            {
                CurrentState = AppLifeTimeState.FirstStart;
                await AppSettings.LoadAsync();
            }
            else
            {
                CurrentState = AppLifeTimeState.Start;
                _isFirstStart = false;
            }

            OnExecute(CurrentState);
        }

        protected abstract void OnExecute(AppLifeTimeState start);


        protected internal virtual void OnPageAppearing(Page page)
        {
            _history.Add(new PageLogObject(page, PageLogActionType.Appearing));
        }

        protected internal virtual void OnPageDisappearing(Page page)
        {
            _history.Add(new PageLogObject(page, PageLogActionType.Disappearing));
        }
    }

    public class PageLogObject
    {
        public PageLogObject(Page page, PageLogActionType actionType)
        {
            Type = page.GetType();
            Page = new WeakReference<Page>(page);
            ActionType = actionType;
            DateTime = DateTime.Now;
        }

        public Type Type { get; }

        public WeakReference<Page> Page { get; }

        public DateTime DateTime { get; }

        public PageLogActionType ActionType { get; }

        public override string ToString()
        {
            return $"Page {Type.Name} was {ActionType.ToString()} in {DateTime}";
        }
    }

    public enum PageLogActionType
    {
        Appearing,
        Disappearing
    }

    public enum AppLifeTimeState
    {
        FirstStart,
        Start,
        Sleep,
        Resume
    }
}