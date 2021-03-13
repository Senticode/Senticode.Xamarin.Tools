using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Senticode.Xamarin.Tools.MVVM.Collections
{
    //[Obsolete("Create extensions the same in WPF.Tools")]
    public partial class ObservableRangeCollection<T>
    {
        private static readonly object _lockerSync = new object();

        /// <summary>
        ///     Creates new collection synchronized with <paramref name="myCollection"/>.
        /// </summary>
        /// <typeparam name="TOut">Type of items in collection.</typeparam>
        /// <param name="myCollection">Collection with which new collection must be synchronized.</param>
        /// <param name="translator">A transform function to apply to each source element.</param>
        /// <returns>Returns new collection.</returns>
        public static ObservableRangeCollection<TOut> CreateSynchronizedCollection<TOut>(ObservableRangeCollection<T> myCollection,
            Func<T, TOut> translator)
        {
            var result = new ObservableRangeCollection<TOut>(myCollection.Select(translator).ToList());
            var reference = new WeakReference<ObservableRangeCollection<TOut>>(result);
            myCollection.CollectionChanged += (sender, args) =>
            {
                if (!reference.TryGetTarget(out var outCollection))
                {
                    return;
                }

                lock (_lockerSync)
                {
                    switch (args.Action)
                    {
                        case NotifyCollectionChangedAction.Reset:
                            OnReset(myCollection, translator, outCollection);
                            break;
                        case NotifyCollectionChangedAction.Add:
                            OnAdd(translator, args, outCollection);
                            break;
                        case NotifyCollectionChangedAction.Remove:
                            OnRemove(translator, args, outCollection);
                            break;
                        case NotifyCollectionChangedAction.Replace:
                            OnReplace(myCollection, translator, args, outCollection);
                            break;
                        case NotifyCollectionChangedAction.Move:
                            OnMove(myCollection, translator, outCollection);
                            break;
                        default:
                            outCollection.ReplaceAll(myCollection.Select(translator).ToList());
                            break;
                    }
                }
            };
            return result;
        }

        private static void OnMove<TOut>(ObservableRangeCollection<T> myCollection, Func<T, TOut> translator,
            ObservableRangeCollection<TOut> outCollection)
        {
            if (myCollection == null)
            {
                throw new ArgumentNullException(nameof(myCollection));
            }

            if (outCollection == null)
            {
                throw new ArgumentNullException(nameof(outCollection));
            }

            //TODO implement this
            outCollection.ReplaceAll(myCollection.Select(translator).ToList());
        }

        private static void OnReplace<TOut>(ObservableRangeCollection<T> myCollection, Func<T, TOut> translator,
            NotifyCollectionChangedEventArgs args, ObservableRangeCollection<TOut> outCollection)
        {
            if (myCollection == null)
            {
                throw new ArgumentNullException(nameof(myCollection));
            }

            if (outCollection == null)
            {
                throw new ArgumentNullException(nameof(outCollection));
            }

            try
            {
                if (args.OldItems.Cast<T>() is IEnumerable<T> oldReplacedItems &&
                    args.NewItems.Cast<T>() is IEnumerable<T> newReplacedItems)
                {
                    outCollection.ReplaceRange(newReplacedItems.Select(translator).ToList(),
                        oldReplacedItems.Select(translator).ToList());
                    return;
                }
            }
            catch
            {
                //next
            }

            outCollection.ReplaceAll(myCollection.Select(translator).ToList());
        }

        private static void OnRemove<TOut>(Func<T, TOut> translator, NotifyCollectionChangedEventArgs args,
            ObservableRangeCollection<TOut> outCollection)
        {
            if (outCollection == null)
            {
                throw new ArgumentNullException(nameof(outCollection));
            }

            if (args.OldItems.Cast<T>() is IEnumerable<T> oldItems)
            {
                try
                {
                    outCollection.RemoveRange(oldItems.Select(translator).ToList());
                    return;
                }
                catch
                {
                    //next
                }
            }

            foreach (var caster in args.OldItems)
            {
                if (caster is IEnumerable<T> items)
                {
                    outCollection.RemoveRange(items.Select(translator).ToList());
                }
            }
        }

        private static void OnAdd<TOut>(Func<T, TOut> translator, NotifyCollectionChangedEventArgs args,
            ObservableRangeCollection<TOut> outCollection)
        {
            if (outCollection == null)
            {
                throw new ArgumentNullException(nameof(outCollection));
            }

            if (args.NewItems.Cast<T>() is IEnumerable<T> newItems)
            {
                try
                {
                    outCollection.AddRange(newItems.Select(translator).ToList());
                    return;
                }
                catch
                {
                    //next
                }
            }

            foreach (var caster in args.NewItems)
            {
                if (caster is IEnumerable<T> items)
                {
                    outCollection.AddRange(items.Select(translator).ToList());
                }
            }
        }

        private static void OnReset<TOut>(ObservableRangeCollection<T> myCollection, Func<T, TOut> translator,
            ObservableRangeCollection<TOut> outCollection)
        {
            if (myCollection == null)
            {
                throw new ArgumentNullException(nameof(myCollection));
            }

            if (outCollection == null)
            {
                throw new ArgumentNullException(nameof(outCollection));
            }

            outCollection.ReplaceAll(myCollection.Select(translator).ToList());
        }
    }
}