using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Senticode.Xamarin.Tools.MVVM.Collections;

namespace Senticode.Xamarin.Tools.MVVM.Extensions
{
    public static class SynchronizationCollectionExtension
    {
        public static ObservableRangeCollection<TOut> ToSynchronizedCollection<TOut, T>(
            this ObservableRangeCollection<T> collection,
            Func<T, TOut> translator)
        {
            var result = new ObservableRangeCollection<TOut>(collection.Select(translator).ToList());
            var reference = new WeakReference<ObservableRangeCollection<TOut>>(result);

            collection.CollectionChanged += (sender, args) =>
            {
                if (reference.TryGetTarget(out var outCollection))
                {
                    switch (args.Action)
                    {
                        case NotifyCollectionChangedAction.Reset:
                            outCollection.ReplaceAll(collection.Select(translator).ToList());
                            break;

                        case NotifyCollectionChangedAction.Add:
                            if (args.NewItems.Cast<T>() is IEnumerable<T> newItems)
                            {
                                try
                                {
                                    outCollection.AddRange(newItems.Select(translator).ToList());
                                }
                                catch
                                {
                                    //next
                                }
                            }

                            break;

                        case NotifyCollectionChangedAction.Remove:
                            if (args.OldItems.Cast<T>() is IEnumerable<T> oldItems)
                            {
                                try
                                {
                                    outCollection.RemoveRange(oldItems.Select(translator).ToList());
                                }
                                catch
                                {
                                    //next
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
                                }
                            }
                            catch
                            {
                                //next
                            }

                            break;

                        case NotifyCollectionChangedAction.Move:
                            //TODO implement this
                            outCollection.ReplaceAll(collection.Select(translator).ToList());
                            break;

                        default:
                            outCollection.ReplaceAll(collection.Select(translator).ToList());
                            break;
                    }
                }
            };

            return result;
        }

        public static ObservableRangeCollection<TOut> ToSynchronizedCollection<T, TOut>(
            this ObservableRangeCollection<T> collection,
            Func<T, IEnumerable<TOut>> translator)
        {
            var result = new ObservableRangeCollection<TOut>(collection.Translate(translator).ToList());
            var reference = new WeakReference<ObservableRangeCollection<TOut>>(result);

            collection.CollectionChanged += (sender, args) =>
            {
                if (reference.TryGetTarget(out var outCollection))
                {
                    //todo lock here
                    {
                        switch (args.Action)
                        {
                            case NotifyCollectionChangedAction.Reset:
                                outCollection.ReplaceAll(collection.Translate(translator));
                                break;

                            case NotifyCollectionChangedAction.Add:
                                if (args.NewItems.Cast<T>() is IEnumerable<T> newItems)
                                {
                                    try
                                    {
                                        outCollection.AddRange(newItems.Translate(translator));
                                    }
                                    catch
                                    {
                                        //next
                                    }
                                }

                                break;

                            case NotifyCollectionChangedAction.Remove:
                                if (args.OldItems.Cast<T>() is IEnumerable<T> oldItems)
                                {
                                    try
                                    {
                                        outCollection.RemoveRange(oldItems.Translate(translator));
                                    }
                                    catch
                                    {
                                        //next
                                    }
                                }

                                break;

                            case NotifyCollectionChangedAction.Replace:
                                try
                                {
                                    if (args.OldItems.Cast<T>() is IEnumerable<T> oldReplacedItems &&
                                        args.NewItems.Cast<T>() is IEnumerable<T> newReplacedItems)
                                    {
                                        outCollection.ReplaceRange(newReplacedItems.Translate(translator),
                                            oldReplacedItems.Translate(translator));
                                    }
                                }
                                catch
                                {
                                    //next
                                }

                                break;

                            case NotifyCollectionChangedAction.Move:
                                //TODO implement this
                                outCollection.ReplaceAll(collection.Translate(translator));
                                break;

                            default:
                                outCollection.ReplaceAll(collection.Translate(translator));
                                break;
                        }
                    }
                }
            };

            return result;
        }

        private static IEnumerable<TOut> Translate<T, TOut>(
            this IEnumerable<T> source,
            Func<T, IEnumerable<TOut>> translator)
        {
            var result = new List<TOut>();

            foreach (var nestedItems in source.Select(translator))
            {
                foreach (var item in nestedItems)
                {
                    result.Add(item);
                }
            }

            return result;
        }
    }
}