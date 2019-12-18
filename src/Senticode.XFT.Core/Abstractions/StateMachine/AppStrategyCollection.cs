using Senticode.Base.Interfaces;
using Senticode.Base.Services;
using Senticode.Xamarin.Tools.Core.Interfaces.Base.StateMachine;

namespace Senticode.Xamarin.Tools.Core.Abstractions.StateMachine
{
    public class AppStrategyCollection<TState> : AppServiceCollection<AppStrategy<TState>>, IService where TState : IState
    {
     
    }
}