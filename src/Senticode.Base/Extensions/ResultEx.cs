using System;
using System.Collections.Generic;
using System.Linq;
using Senticode.Base.Interfaces;

namespace Senticode.Base.Extensions
{
    public static class ResultEx
    {
        public static IResult ToAggregateResult(this IEnumerable<IResult> results)
        {
            var exceptionList = results
                .Where(result => !result.IsSuccessful)
                .Select(result => result.Exception)
                .ToList();
            return exceptionList.Count > 0 ? new Result(new AggregateException(exceptionList)) : new Result();
        }
    }
}
