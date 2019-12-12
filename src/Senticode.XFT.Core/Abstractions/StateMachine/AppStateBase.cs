using System;
using Senticode.Xamarin.Tools.Core.Interfaces.Base.StateMachine;

namespace Senticode.Xamarin.Tools.Core.Abstractions.StateMachine
{
    public abstract class AppStateBase : IState
    {
        public abstract int CompareTo(object obj);

        public int Index { get; protected internal set; }

        public DateTime DateTime { get; protected internal set; }

        public abstract object Clone();
    }
}