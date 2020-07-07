using System;
using Senticode.Xamarin.Tools.Core.Interfaces.Base.StateMachine;

namespace Senticode.Xamarin.Tools.Core.Abstractions.StateMachine
{
    public class StateTransformer<T> : IStateTransformer<T>, IDisposable where T : AppStateBase
    {
        private Action<T> _transformFunc;

        public StateTransformer(Action<T> transformFunc)
        {
            _transformFunc = transformFunc;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        public void Transform(T state)
        {
            if (_transformFunc != null)
            {
                state.Index++;
                state.DateTime = DateTime.UtcNow;
                _transformFunc.Invoke(state);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _transformFunc = null;
            }
        }
    }
}