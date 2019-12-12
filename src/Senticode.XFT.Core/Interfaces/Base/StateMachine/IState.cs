using System;

namespace Senticode.Xamarin.Tools.Core.Interfaces.Base.StateMachine
{
    public interface IState : ICloneable, IComparable
    {
        int Index { get;  }

        DateTime DateTime { get;  }
    }
}