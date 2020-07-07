using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Senticode.Xamarin.Tools.MVVM.Interfaces;

namespace Senticode.Xamarin.Tools.MVVM.Collections
{
    public partial class ObservableRangeCollection<T> : ExtendedObservableCollection<T>, IObservableRangeCollection<T>
    {
        private const string _COUNT = "Count";
        private const string _INDEXER = "Item[]";
        private readonly object _locker = new object();

        public ObservableRangeCollection()
        {
        }

        public ObservableRangeCollection(List<T> list) : base(list)
        {
        }


        /// <summary>
        ///     Raises collection change event.
        /// </summary>
        private void NotifyChanges(NotifyCollectionChangedEventArgs args, bool countIsChanged = true)
        {
            ResumeChangeNotifications();
            if (countIsChanged)
            {
                OnPropertyChanged(new PropertyChangedEventArgs(_COUNT));
            }

            OnPropertyChanged(new PropertyChangedEventArgs(_INDEXER));
            OnCollectionChanged(args);
        }

        #region Implementation of IObservableRangeCollection<T>

        /// <summary>
        ///     Adds new elements at the end of the collection.
        /// </summary>
        public void AddRange(IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            lock (_locker)
            {
                SuspendChangeNotifications();
                var startIndex = Count;
                var list = collection as List<T>;
                var changedItems = list ?? new List<T>(collection);
                foreach (var i in changedItems)
                {
                    Items.Add(i);
                }

                NotifyChanges(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, changedItems,
                    startIndex));
            }
        }

        /// <summary>
        ///     Removes the first occurrence of each item in the specified collection from ObservableCollection(Of T).
        /// </summary>
        public void RemoveRange(IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            lock (_locker)
            {
                SuspendChangeNotifications();
                var list = collection as IList<T> ?? collection.ToList();
                foreach (var i in list)
                {
                    Remove(i);
                }

                NotifyChanges(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, list));
            }
        }

        /// <summary>
        ///     Clears the current collection and replaces it with the specified item.
        /// </summary>
        public void Replace(T newItem, T oldItem)
        {
            ReplaceRange(new[] {newItem}, new[] {oldItem});
        }

        /// <summary>
        ///     Replace elements the current collection it with the specified collection.
        /// </summary>
        public void ReplaceRange(IEnumerable<T> newItems, IEnumerable<T> oldItems = null)
        {
            if (newItems == null)
            {
                throw new ArgumentNullException(nameof(newItems));
            }

            lock (_locker)
            {
                if (oldItems != null)
                {
                    var oldArray = oldItems as T[] ?? oldItems.ToArray();
                    var newArray = newItems as T[] ?? newItems.ToArray();
                    var oldList = new List<T>();
                    var newList = new List<T>();
                    SuspendChangeNotifications();

                    //Replace
                    var i = ReplaceElements(oldArray, oldList, newArray, newList);

                    //Remove remains old items
                    RemoveRemainsOldElements(i, oldArray);

                    //Add remains new items 
                    AddRemainsNewElements(i, newArray);

                    NotifyChanges(
                        new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newList, oldList),
                        false);
                }
                else
                {
                    AddRange(newItems);
                }
            }
        }

        private void AddRemainsNewElements(int i, T[] newArray)
        {
            if (i < newArray.Length)
            {
                var remainsItems = new T[newArray.Length - i];
                for (var j = 0; j < remainsItems.Length; j++)
                {
                    remainsItems[j] = newArray[i + j];
                }

                foreach (var item in remainsItems)
                {
                    Items.Add(item);
                }
            }
        }

        private void RemoveRemainsOldElements(int i, T[] oldArray)
        {
            if (i < oldArray.Length)
            {
                var remainsItems = new T[oldArray.Length - i];
                for (var j = 0; j < remainsItems.Length; j++)
                {
                    remainsItems[j] = oldArray[i + j];
                }

                foreach (var item in remainsItems)
                {
                    Remove(item);
                }
            }
        }

        private int ReplaceElements(T[] oldArray, List<T> oldList, T[] newArray, List<T> newList)
        {
            int i;
            for (i = 0; i < oldArray.Length; i++)
            {
                var oldItem = oldArray[i];
                oldList.Add(oldItem);
                if (i < newArray.Length)
                {
                    var newItem = newArray[i];
                    var index = Items.IndexOf(oldItem);
                    if (index >= 0)
                    {
                        Items.RemoveAt(index);
                        Items.Insert(index, newItem);
                    }
                    else
                    {
                        Items.Add(newItem);
                    }

                    newList.Add(newItem);
                }
                else
                {
                    break;
                }
            }

            return i;
        }

        /// <summary>
        ///     Clears the current collection and replaces it with the specified collection.
        /// </summary>
        public void ReplaceAll(IEnumerable<T> newItems)
        {
            if (newItems == null)
            {
                throw new ArgumentNullException(nameof(newItems));
            }

            lock (_locker)
            {
                SuspendChangeNotifications();
                Clear();
                foreach (var i in newItems)
                {
                    Items.Add(i);
                }

                NotifyChanges(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        #endregion
    }
}