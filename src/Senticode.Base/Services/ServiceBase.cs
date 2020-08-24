using System;
using Senticode.Base.Exceptions;
using Senticode.Base.Interfaces;

namespace Senticode.Base.Services
{
    public abstract class ServiceBase : IService
    {
        private readonly object _locker = new object();

        public bool IsInitialized { get; private set; }

        public bool IsInitializing { get; private set; }

        public bool IsReleasing { get; private set; }

        public virtual IResult Initialize() => Initialize(null);

        public virtual IResult Release() => Release(null);

        protected virtual IResult Initialize(Action initAction)
        {
            Exception exception = null;

            lock (_locker)
            {
                if (!IsInitialized && !IsInitializing)
                {
                    IsInitializing = true;

                    try
                    {
                        initAction?.Invoke();
                    }
                    catch (Exception e)
                    {
                        exception = new ServiceInitializeException(e, GetType());
                    }

                    IsInitializing = false;
                    IsInitialized = exception == null;
                }
            }

            return new Result(exception);
        }

        protected virtual IResult Release(Action releaseAction)
        {
            Exception exception = null;

            lock (_locker)
            {
                if (IsInitialized && !IsReleasing)
                {
                    IsReleasing = true;

                    try
                    {
                        releaseAction?.Invoke();
                    }
                    catch (Exception e)
                    {
                        exception = new ServiceReleaseException(e, GetType());
                    }

                    IsReleasing = false;
                    IsInitialized = exception != null;
                }
            }

            return new Result(exception);
        }
    }
}