using System.Collections.Generic;

namespace Senticode.Xamarin.Tools.Core.Helpers
{
    /// <summary>
    ///     Class that contains LINQ extensions.
    /// </summary>
    public static class LinqExtensions
    {
        /// <summary>
        ///     Converts <c>IEnumerable</c> to <c>HashSet</c>.
        /// </summary>
        /// <typeparam name="T">Type of items.</typeparam>
        /// <param name="source">Source collection.</param>
        /// <param name="comparer">Defines methods to support the comparison of objects for equality.</param>
        /// <returns>Returns c>HashSet</c>.</returns>
        public static HashSet<T> ToHashSet<T>(
            this IEnumerable<T> source,
            IEqualityComparer<T> comparer = null)
        {
            return new HashSet<T>(source, comparer);
        }
    }
}