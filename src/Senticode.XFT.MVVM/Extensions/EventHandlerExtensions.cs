using System;

namespace Senticode.Xamarin.Tools.MVVM.Extensions
{
    /// <summary>
    ///     Class with extension methods for event.
    /// </summary>
    public static class EventHandlerExtensions
    {
        /// <summary>
        ///     Unsubscribes all handlers from the event.
        /// </summary>
        /// <typeparam name="TEventArgs">Type of event arguments.</typeparam>
        /// <param name="handler">Event.</param>
        public static void UnsubscribeAllHandlers<TEventArgs>(this EventHandler<TEventArgs> handler)
            where TEventArgs : EventArgs
        {
            if (handler != null)
            {
                var invocationList = handler.GetInvocationList();

                foreach (var d in invocationList)
                {
                    handler -= (EventHandler<TEventArgs>) d;
                }
            }
        }
    }
}