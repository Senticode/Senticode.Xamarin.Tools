using System;
using System.Collections.Generic;
using Senticode.Base.Interfaces;

namespace Senticode.Base.Services
{
    public class AppServiceCollection<TService> : List<TService>, IService where TService : IService
    {
        private readonly object _locker = new object();

        public bool IsInitialized { get; set; }

        public bool IsInitializing { get; set; }

        public bool IsReleasing { get; set; }

        public IResult Initialize()
        {
            var exceptions = new List<Exception>();
            lock (_locker)
            {
                if (!IsInitialized)
                {
                    IsInitializing = true;
                    foreach (var service in this)
                    {
                        var r = service.Initialize();
                        if (!r.IsSuccessful)
                        {
                            exceptions.Add(r.Exception);
                        }
                    }

                    IsInitializing = false;
                    IsInitialized = true;
                }
            }

            return exceptions.Count == 0
                ? new Result()
                : new Result(new AggregateException(exceptions));
        }

        public IResult Release()
        {
            var exceptions = new List<Exception>();
            lock (_locker)
            {
                if (IsInitialized)
                {
                    IsReleasing = true;
                    foreach (var service in this)
                    {
                        var r = service.Release();
                        if (!r.IsSuccessful)
                        {
                            exceptions.Add(r.Exception);
                        }
                    }

                    IsReleasing = false;
                    IsInitialized = false;
                }
            }

            return exceptions.Count == 0
                ? new Result()
                : new Result(new AggregateException(exceptions));
        }

        /// <summary>
        ///     This method try initialize service if AppServiceCollection was initialized.
        /// </summary>
        /// <param name="service"></param>
        public new void Add(TService service)
        {
            lock (_locker)
            {
                if (IsInitialized)
                {
                    if (!service.IsInitialized)
                    {
                        service.Initialize();
                    }
                }
                base.Add(service);
            }
        }
    }
}