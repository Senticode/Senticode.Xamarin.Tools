using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Senticode.Xamarin.Tools.Core.Events
{
    /// <summary>
    ///     Defines a base class to publish and subscribe to events.
    /// </summary>
    public abstract class EventBase
    {
        private readonly List<IEventSubscription> _subscriptions = new List<IEventSubscription>();

        /// <summary>
        ///     Allows the SynchronizationContext to be set by the <see cref = "EventAggregator" /> for UI Thread Dispatching
        /// </summary>
        public SynchronizationContext SynchronizationContext { get; set; }

        /// <summary>Gets the list of current subscriptions.</summary>
        /// <value>The current subscribers.</value>
        protected ICollection<IEventSubscription> Subscriptions => _subscriptions;

        /// <summary>
        ///     Adds the specified <see cref = "T:Senticode.Xamarin.Tools.Core.Events.IEventSubscription" /> to the subscribers' collection.
        /// </summary>
        /// <param name = "eventSubscription">The subscriber.</param>
        /// <returns>The <see cref = "T:Senticode.Xamarin.Tools.Core.Events.SubscriptionToken" /> that uniquely identifies every subscriber.</returns>
        /// <remarks>
        ///     Adds the subscription to the internal list and assigns it a new
        ///     <see cref = "T:Senticode.Xamarin.Tools.Core.Events.SubscriptionToken" />.
        /// </remarks>
        protected virtual SubscriptionToken InternalSubscribe(IEventSubscription eventSubscription)
        {
            if (eventSubscription == null)
            {
                throw new ArgumentNullException(nameof(eventSubscription));
            }

            eventSubscription.SubscriptionToken = new SubscriptionToken(Unsubscribe);
            var subscriptions = Subscriptions;
            var lockTaken = false;
            try
            {
                Monitor.Enter(subscriptions, ref lockTaken);
                Subscriptions.Add(eventSubscription);
            }
            finally
            {
                if (lockTaken)
                {
                    Monitor.Exit(subscriptions);
                }
            }

            return eventSubscription.SubscriptionToken;
        }

        /// <summary>
        ///     Calls all the execution strategies exposed by the list of
        ///     <see cref = "T:Senticode.Xamarin.Tools.Core.Events.IEventSubscription" />.
        /// </summary>
        /// <param name = "arguments">The arguments that will be passed to the listeners.</param>
        /// <remarks>
        ///     Before executing the strategies, this class will prune all the subscribers from the
        ///     list that return a <see langword = "null" /> <see cref = "T:System.Action`1" /> when calling the
        ///     <see cref = "M:Senticode.Xamarin.Tools.Core.Events.IEventSubscription.GetExecutionStrategy" /> method.
        /// </remarks>
        protected virtual void InternalPublish(params object[] arguments)
        {
            foreach (var andReturnStrategy in PruneAndReturnStrategies())
            {
                andReturnStrategy(arguments);
            }
        }

        /// <summary>
        ///     Removes the subscriber matching the <see cref = "T:Senticode.Xamarin.Tools.Core.Events.SubscriptionToken" />.
        /// </summary>
        /// <param name = "token">
        ///     The <see cref = "T:Senticode.Xamarin.Tools.Core.Events.SubscriptionToken" /> returned by
        ///     <see cref = "T:Senticode.Xamarin.Tools.Core.Events.EventBase" /> while subscribing to the event.
        /// </param>
        public virtual void Unsubscribe(SubscriptionToken token)
        {
            var subscriptions = Subscriptions;
            var lockTaken = false;
            try
            {
                Monitor.Enter(subscriptions, ref lockTaken);
                var eventSubscription = Subscriptions.FirstOrDefault(evt => evt.SubscriptionToken == token);
                if (eventSubscription == null)
                {
                    return;
                }

                Subscriptions.Remove(eventSubscription);
            }
            finally
            {
                if (lockTaken)
                {
                    Monitor.Exit(subscriptions);
                }
            }
        }

        /// <summary>
        ///     Returns <see langword = "true" /> if there is a subscriber matching
        ///     <see cref = "T:Senticode.Xamarin.Tools.Core.Events.SubscriptionToken" />.
        /// </summary>
        /// <param name = "token">
        ///     The <see cref = "T:Senticode.Xamarin.Tools.Core.Events.SubscriptionToken" /> returned by
        ///     <see cref = "T:Senticode.Xamarin.Tools.Core.Events.EventBase" /> while subscribing to the event.
        /// </param>
        /// <returns>
        ///     <see langword = "true" /> if there is a <see cref = "T:Senticode.Xamarin.Tools.Core.Events.SubscriptionToken" /> that
        ///     matches; otherwise <see langword = "false" />.
        /// </returns>
        public virtual bool Contains(SubscriptionToken token)
        {
            var subscriptions = Subscriptions;
            var lockTaken = false;
            try
            {
                Monitor.Enter(subscriptions, ref lockTaken);
                return Subscriptions.FirstOrDefault(evt => evt.SubscriptionToken == token) != null;
            }
            finally
            {
                if (lockTaken)
                {
                    Monitor.Exit(subscriptions);
                }
            }
        }

        private List<Action<object[]>> PruneAndReturnStrategies()
        {
            var actionList = new List<Action<object[]>>();
            var subscriptions = Subscriptions;
            var lockTaken = false;
            try
            {
                Monitor.Enter(subscriptions, ref lockTaken);
                for (var index = Subscriptions.Count - 1; index >= 0; --index)
                {
                    var executionStrategy = _subscriptions[index].GetExecutionStrategy();
                    if (executionStrategy == null)
                    {
                        _subscriptions.RemoveAt(index);
                    }
                    else
                    {
                        actionList.Add(executionStrategy);
                    }
                }
            }
            finally
            {
                if (lockTaken)
                {
                    Monitor.Exit(subscriptions);
                }
            }

            return actionList;
        }
    }
}