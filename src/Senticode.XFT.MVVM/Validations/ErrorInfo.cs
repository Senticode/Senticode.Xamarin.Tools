using System;

namespace Senticode.Xamarin.Tools.MVVM.Validations
{
    /// <summary>
    ///     Struct that contains validation errors.
    /// </summary>
    public struct ErrorInfo
    {
        public ErrorInfo(Guid handleId, string message)
        {
            HandleId = handleId;
            Message = message;
        }

        /// <summary>
        ///     Gets error description.
        /// </summary>
        public string Message { get; }

        /// <summary>
        ///     Gets ID.
        /// </summary>
        public Guid HandleId { get; }

        #region Overrides of Object

        public override string ToString()
        {
            return Message;
        }

        #endregion
    }
}