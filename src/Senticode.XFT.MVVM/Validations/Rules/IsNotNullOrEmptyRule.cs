using Senticode.Xamarin.Tools.MVVM.Validations.Interfaces;

namespace Senticode.Xamarin.Tools.MVVM.Validations.Rules
{
    /// <summary>
    ///     Class that checks if object is not null.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class IsNotNullOrEmptyRule<T> : IValidationRule<T>
    {
        /// <summary>
        ///     Gets or sets the ValidationMessage property.
        /// </summary>
        public string ValidationMessage { get; set; }

        /// <summary>
        ///     Checks for <c>null</c>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns><c>true</c> if object is not <c>null</c>, otherwise, <c>false</c>.</returns>
        public bool Check(T value)
        {
            if (value == null)
            {
                return false;
            }

            var str = value as string;

            return !string.IsNullOrWhiteSpace(str);
        }
    }
}