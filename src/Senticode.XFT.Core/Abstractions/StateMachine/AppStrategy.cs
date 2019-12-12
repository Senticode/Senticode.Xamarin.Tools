using Senticode.Base.Services;
using Senticode.Xamarin.Tools.Core.Interfaces.Base.StateMachine;

namespace Senticode.Xamarin.Tools.Core.Abstractions.StateMachine
{
    public abstract class AppStrategy<TState>: ServiceBase where TState : IState
    {
        protected AppStrategy(IStateTransformer<TState> transformer,
            IStateMachine<TState, IStateTransformer<TState>> stateMachine)
        {
            Transformer = transformer;
            StateMachine = stateMachine;
        }

        public IStateTransformer<TState> Transformer { get; }

        public IStateMachine<TState, IStateTransformer<TState>> StateMachine { get; }
    }
}