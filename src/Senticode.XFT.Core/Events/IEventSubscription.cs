using System;

namespace Senticode.Xamarin.Tools.Core.Events
{
    /// <summary>
    ///     Defines a contract for an event subscription to be used by <see cref = "T:Prism.Events.EventBase" />.
    /// </summary>
    public interface IEventSubscription
    {
        /// <summary>
        ///     Gets or sets a <see cref = "P:Senticode.Xamarin.Tools.Core.Events.IEventSubscription.SubscriptionToken" /> that identifies
        ///     this <see cref = "T:Senticode.Xamarin.Tools.Core.Events.IEventSubscription" />.
        /// </summary>
        /// <value>A token that identifies this <see cref = "T:Senticode.Xamarin.Tools.Core.Events.IEventSubscription" />.</value>
        SubscriptionToken SubscriptionToken { get; set; }

        /// <summary>Gets the execution strategy to publish this event.</summary>
        /// <returns>
        ///     An <see cref = "T:System.Action`1" /> with the execution strategy, or <see langword = "null" /> if the
        ///     <see cref = "T:Senticode.Xamarin.Tools.Core.Events.IEventSubscription" /> is no longer valid.
        /// </returns>
        Action<object[]> GetExecutionStrategy();
    }
}