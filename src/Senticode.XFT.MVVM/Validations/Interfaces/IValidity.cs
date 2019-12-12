using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Senticode.Xamarin.Tools.MVVM.Validations.Interfaces
{
    /// <summary>
    ///     Represents a type that can be validated.
    /// </summary>
    public interface IValidity
    {
        /// <summary>
        ///     Gets ID/
        /// </summary>
        Guid HandleId { get; }

        /// <summary>
        ///     <c>true</c> if valid, otherwise, <c>false</c>.
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        ///     List of validation errors.
        /// </summary>
        List<ErrorInfo> Errors { get; }

        /// <summary>
        ///     Command used for validation.
        /// </summary>
        Command ValidateCommand { get; }

        /// <summary>
        ///     Method used for validation.
        /// </summary>
        /// <returns><c>true</c> if object is valid, otherwise, <c>false</c>.</returns>
        bool OnValidate();
    }
}