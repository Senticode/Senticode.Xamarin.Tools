using System.Collections.Generic;

namespace Senticode.Xamarin.Tools.Core.Interfaces.Base.StateMachine
{
    public interface IStateHistory<TState> where TState: IState
    {
        IReadOnlyList<IState> States { get; }

        IState MoveTo(IState state);
    }
}