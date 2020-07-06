using System;
using Senticode.Base.Interfaces;

namespace Senticode.Base
{
    public class Result : IResult
    {
        public Result(Exception exception = null)
        {
            Exception = exception;
        }

        public bool IsSuccessful => Exception == null;

        public Exception Exception { get; set; }

        public static Result Successful { get; } = new Result();
    }

    public class Result<T> : Result, IResult<T>
    {
        public Result(T result)
        {
            Object = result;
        }

        public Result(Exception exception) : base(exception)
        {
        }

        public Result(Exception exception, T result) : base(exception)
        {
            Object = result;
        }

        public T Object { get; }
    }
}