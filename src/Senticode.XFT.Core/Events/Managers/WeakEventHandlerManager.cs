using System;
using System.Collections.Generic;
using System.Threading;

namespace Senticode.Xamarin.Tools.Core.Events.Managers
{
    /// <summary>
    ///     Handles management and dispatching of EventHandlers in a weak way.
    /// </summary>
    public static class WeakEventHandlerManager<TArgs> where TArgs : EventArgs
    {
        public const int DefaultListSize = 2;

        /// <summary>
        ///     Invokes the handlers
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name="args"></param>
        /// <param name = "handlers"></param>
        public static void CallHandlers(object sender, TArgs args, List<WeakReference> handlers)
        {
            if (handlers != null)
            {
                // Take a snapshot of the handlers before we call out to them since the handlers
                // could cause the array to me modified while we are reading it.
                var callers = new EventHandler<TArgs>[handlers.Count];
                var count = 0;

                //Clean up handlers
                count = CleanupOldHandlers(handlers, callers, count);

                // Call the handlers that we snapshotted
                for (var i = 0; i < count; i++)
                {
                    CallHandler(sender, args, callers[i]);
                }
            }
        }

        private static void CallHandler(object sender, TArgs args, EventHandler<TArgs> eventHandler)
        {
            if (eventHandler != null)
            {
                if (SynchronizationContext.Current != null)
                {
                    SynchronizationContext.Current.Post(o => eventHandler(sender, args), null);
                }
                else
                {
                    eventHandler(sender, args);
                }
            }
        }

        private static int CleanupOldHandlers(List<WeakReference> handlers, EventHandler<TArgs>[] callers, int count)
        {
            for (var i = handlers.Count - 1; i >= 0; i--)
            {
                var reference = handlers[i];
                if (!(reference.Target is EventHandler<TArgs> handler))
                {
                    // Clean up old handlers that have been collected
                    handlers.RemoveAt(i);
                }
                else
                {
                    callers[count] = handler;
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        ///     Adds a handler to the supplied list in a weak way.
        /// </summary>
        /// <param name = "handlers">Existing handler list.  It will be created if null.</param>
        /// <param name = "handler">Handler to add.</param>
        /// <param name = "defaultListSize">Default list size.</param>
        public static void AddHandler(ref List<WeakReference> handlers, EventHandler<TArgs> handler,
            int defaultListSize = DefaultListSize)
        {
            if (handlers == null)
            {
                handlers = defaultListSize > 0 ? new List<WeakReference>(defaultListSize) : new List<WeakReference>();
            }

            handlers.Add(new WeakReference(handler));
        }

        /// <summary>
        ///     Removes an event handler from the reference list.
        /// </summary>
        /// <param name = "handlers">Handler list to remove reference from.</param>
        /// <param name = "handler">Handler to remove.</param>
        public static void RemoveHandler(List<WeakReference> handlers, EventHandler<TArgs> handler)
        {
            if (handlers != null)
            {
                for (var i = handlers.Count - 1; i >= 0; i--)
                {
                    var reference = handlers[i];
                    if (!(reference.Target is EventHandler<TArgs> existingHandler) || existingHandler == handler)
                    {
                        // Clean up old handlers that have been collected
                        // in addition to the handler that is to be removed.
                        handlers.RemoveAt(i);
                    }
                }
            }
        }
    }

}
