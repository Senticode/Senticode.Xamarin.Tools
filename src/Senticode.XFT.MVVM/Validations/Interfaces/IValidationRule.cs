namespace Senticode.Xamarin.Tools.MVVM.Validations.Interfaces
{
    /// <summary>
    ///     Incapsulates way of validation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IValidationRule<T>
    {
        /// <summary>
        ///      Gets or sets the ValidationMessage property.
        /// </summary>
        string ValidationMessage { get; set; }

        /// <summary>
        ///     Executes validation logic.
        /// </summary>
        /// <param name="value">Object to validate.</param>
        /// <returns><c>true</c> if object is valid, otherwise, <c>false</c>.</returns>
        bool Check(T value);
    }
}