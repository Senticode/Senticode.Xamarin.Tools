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

        public virtual IResult Initialize()
        {
            return Initialize(null);
        }

        public virtual IResult Release()
        {
            return Release(null);
        }

        protected virtual IResult Initialize(Action initAction)
        {
            Result result = null;
            lock (_locker)
            {
                if (!IsInitialized)
                {
                    try
                    {
                        IsInitializing = true;
                        initAction?.Invoke();
                        IsInitialized = true;
                    }
                    catch (Exception ex)
                    {
                        result = new Result(new ServiceInitializeException(ex, GetType()));
                    }
                    finally
                    {
                        IsInitializing = false;
                    }
                }
            }

            return result ?? new Result();
        }

        protected virtual IResult Release(Action releaseAction)
        {
            Result result = null;
            lock (_locker)
            {
                if (IsInitialized)
                {
                    try
                    {
                        IsReleasing = true;
                        releaseAction?.Invoke();
                        IsInitialized = false;
                    }
                    catch (Exception ex)
                    {
                        result = new Result(new ServiceReleaseException(ex, GetType()));
                    }
                    finally
                    {
                        IsReleasing = false;
                    }
                }
            }

            return result ?? new Result();
        }
    }
}