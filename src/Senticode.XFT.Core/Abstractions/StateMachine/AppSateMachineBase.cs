using System;
using System.ComponentModel;
using Senticode.Xamarin.Tools.Core.Interfaces.Base.StateMachine;

namespace Senticode.Xamarin.Tools.Core.Abstractions.StateMachine
{
    public class AppSateMachineBase<TState> : IStateMachine<TState, IStateTransformer<TState>>
        where TState : AppStateBase, new()
    {
        private readonly object _locker = new object();

        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangingEventHandler PropertyChanging;

        public TState State { get; } = new TState();

        public TState DoNext(IStateTransformer<TState> transformer)
        {
            lock (_locker)
            {
                if (transformer == null)
                {
                    throw new ArgumentNullException(nameof(transformer));
                }

                PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(State)));

                transformer.Transform(State);

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(State)));

                return State;
            }
        }
    }
}