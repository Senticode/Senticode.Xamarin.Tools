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
                if (reference.TryGetTarget(out var outCollection))
                {
                    lock (_lockerSync)
                    {
                        switch (args.Action)
                        {
                            case NotifyCollectionChangedAction.Reset:
                                outCollection.ReplaceAll(myCollection.Select(translator).ToList());
                                break;
                            case NotifyCollectionChangedAction.Add:
                                if (args.NewItems.Cast<T>() is IEnumerable<T> newItems)
                                {
                                    try
                                    {
                                        outCollection.AddRange(newItems.Select(translator).ToList());
                                        break;
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

                                break;
                            case NotifyCollectionChangedAction.Remove:
                                if (args.OldItems.Cast<T>() is IEnumerable<T> oldItems)
                                {
                                    try
                                    {
                                        outCollection.RemoveRange(oldItems.Select(translator).ToList());
                                        break;
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
                                break;
                            case NotifyCollectionChangedAction.Replace:
                                try
                                {
                                    if (args.OldItems.Cast<T>() is IEnumerable<T> oldReplacedItems &&
                                        args.NewItems.Cast<T>() is IEnumerable<T> newReplacedItems)
                                    {
                                        outCollection.ReplaceRange(newReplacedItems.Select(translator).ToList(),
                                            oldReplacedItems.Select(translator).ToList());
                                        break;
                                    }
                                }
                                catch
                                {
                                    //next
                                }
                                outCollection.ReplaceAll(myCollection.Select(translator).ToList());
                                break;
                            case NotifyCollectionChangedAction.Move:
                                //TODO implement this
                                outCollection.ReplaceAll(myCollection.Select(translator).ToList());
                                break;
                            default:
                                outCollection.ReplaceAll(myCollection.Select(translator).ToList());
                                break;
                        }
                    }
                }
            };
            return result;
        }
    }
}