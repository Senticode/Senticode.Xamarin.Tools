using System.ComponentModel;

namespace Senticode.Xamarin.Tools.Core.Interfaces.Base.StateMachine
{
    public interface IStateMachine<TState, in TTransformer> : IStateMachine<TState>
        where TState : IState
        where TTransformer : IStateTransformer<TState>
    {
        TState DoNext(TTransformer transformer);
    }

    public interface IStateMachine<TState> : INotifyPropertyChanged, INotifyPropertyChanging
        where TState : IState
    {
        TState State { get; }
    }
}