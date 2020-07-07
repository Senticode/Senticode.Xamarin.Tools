using System.Collections.Generic;

namespace Senticode.Xamarin.Tools.MVVM.Interfaces
{
    /// <summary>
    ///     Interface for Observable collection with possibility to add ranges of items.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IObservableRangeCollection<in T>
    {
        /// <summary>
        ///     Adds new elements at the end of the collection.
        /// </summary>
        void AddRange(IEnumerable<T> collection);

        /// <summary>
        ///     Removes the first occurrence of each item in the specified collection from ObservableCollection(Of T).
        /// </summary>
        void RemoveRange(IEnumerable<T> collection);

        /// <summary>
        ///     Clears the current collection and replaces it with the specified item.
        /// </summary>
        void Replace(T newItem, T oldItem);

        /// <summary>
        ///     Replace elements the current collection it with the specified collection.
        /// </summary>
        void ReplaceRange(IEnumerable<T> newItems, IEnumerable<T> oldItems = null);

        /// <summary>
        ///     Clears the current collection and replaces it with the specified collection.
        /// </summary>
        void ReplaceAll(IEnumerable<T> newItems);
    }
}