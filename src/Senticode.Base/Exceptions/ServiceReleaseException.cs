using System;

namespace Senticode.Base.Exceptions
{
    [Serializable]
    public class ServiceReleaseException : Exception
    {
        public ServiceReleaseException(Exception exception, Type serviceType) : base(
            $"An error occurred while trying to release the service of {serviceType?.Name}", exception)
        {
            ServiceType = serviceType;
        }

        public Type ServiceType { get; }
    }
}