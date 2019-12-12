using System;
using System.Collections.Generic;
using System.Threading;

namespace Senticode.Xamarin.Tools.Core.Events
{
    public class EventAggregator : IEventAggregator
    {
        private readonly Dictionary<Type, EventBase> _events = new Dictionary<Type, EventBase>();
        private readonly SynchronizationContext _syncContext = SynchronizationContext.Current;

        /// <summary>
        ///     Gets the single instance of the event managed by this <see cref = "EventAggregator" />. Multiple calls to this
        ///     method with the same <typeparamref name = "TEventType" /> returns the same event instance.
        /// </summary>
        /// <typeparam name = "TEventType">
        ///     The type of event to get. This must inherit from
        ///     <see cref = "T:Senticode.Xamarin.Tools.Core.Events.EventBase" />.
        /// </typeparam>
        /// <returns>A singleton instance of an event object of type <typeparamref name = "TEventType" />.</returns>
        public TEventType GetEvent<TEventType>() where TEventType : EventBase, new()
        {
            var events = this._events;
            var lockTaken = false;
            try
            {
                Monitor.Enter(events, ref lockTaken);
                if (this._events.TryGetValue(typeof(TEventType), out var eventBase))
                {
                    return (TEventType) eventBase;
                }

                var instance = Activator.CreateInstance<TEventType>();
                instance.SynchronizationContext = _syncContext;
                this._events[typeof(TEventType)] = instance;
                return instance;
            }
            finally
            {
                if (lockTaken)
                {
                    Monitor.Exit(events);
                }
            }
        }
    }
}