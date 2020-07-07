namespace Senticode.Xamarin.Tools.Core.Interfaces.Base.StateMachine
{
    public interface IStateTransformer<in TState> where TState: IState
    {
        void Transform(TState state);
    }
}
