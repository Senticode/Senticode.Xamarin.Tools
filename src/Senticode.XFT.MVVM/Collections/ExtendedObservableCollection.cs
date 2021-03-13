using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Senticode.Xamarin.Tools.MVVM.Collections
{
    /// <summary>
    ///     Abstract class that adds thread safe event raising to the <see cref="ObservableCollection{T}"/>.
    /// </summary>
    public abstract class ExtendedObservableCollection<T> : ObservableCollection<T>
    {
        /// <summary>
        ///     This private variable holds the flag to
        ///     turn on and off the collection changed notification.
        /// </summary>
        protected bool NotificationIsSuspended { get; private set; }

        /// <summary>
        ///     Initializes a new instance of the ObservableRangeCollection class.
        /// </summary>
        protected ExtendedObservableCollection(): base()
        {
            NotificationIsSuspended = false;
        }
        protected ExtendedObservableCollection(List<T> list) : base(list)
        {
            NotificationIsSuspended = false;
        }
        /// <summary>
        ///     This event is overridden CollectionChanged event of the observable collection.
        /// </summary>
        public override event NotifyCollectionChangedEventHandler CollectionChanged;
        
        /// <summary>
        ///     Resumes collection changed notification.
        /// </summary>
        protected void ResumeChangeNotifications()
        {
            NotificationIsSuspended = false;
        }

        /// <summary>
        ///     Suspends collection changed notification.
        /// </summary>
        protected void SuspendChangeNotifications()
        {
            NotificationIsSuspended = true;
        }

        /// <summary>
        ///     This collection changed event performs thread safe event raising.
        /// </summary>
        /// <param name = "e">The event argument.</param>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            // Recommended is to avoid reentry 
            // in collection changed event while collection
            // is getting changed on other thread.
            using (BlockReentrancy())
            {
                if (!NotificationIsSuspended)
                {
                    var eventHandler = CollectionChanged;
                    if (eventHandler == null)
                    {
                        return;
                    }
                    var delegates = eventHandler.GetInvocationList();
                    foreach (var @delegate in delegates)
                    {
                        var handler = (NotifyCollectionChangedEventHandler)@delegate;
                        handler(this, e);
                    }
                }
            }
        }
    }
}
