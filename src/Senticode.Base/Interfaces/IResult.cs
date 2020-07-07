using System;

namespace Senticode.Base.Interfaces
{
    public interface IResult
    {
        bool IsSuccessful { get; }

        Exception Exception { get; }
    }

    public interface IResult<out T> : IResult
    {
        T Object { get; }
    }
}