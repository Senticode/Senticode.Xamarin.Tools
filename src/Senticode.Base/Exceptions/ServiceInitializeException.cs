using System;

namespace Senticode.Base.Exceptions
{
    public class ServiceInitializeException : Exception
    {
        public ServiceInitializeException(Exception exception, Type serviceType) : base(
            $"An error occurred while trying to initialize the service of {serviceType?.Name}", exception)
        {
            ServiceType = serviceType;
        }

        public Type ServiceType { get; }
    }
}