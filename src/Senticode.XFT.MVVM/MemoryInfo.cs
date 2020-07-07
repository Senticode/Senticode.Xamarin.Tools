using System.Collections.Generic;

namespace Senticode.Xamarin.Tools.MVVM
{
    /// <summary>
    ///     A helper class that contains information about current objects of type ViewBase and ModelBase.
    ///     Debug Mode Only.
    /// </summary>
    public static class MemoryInfo
    {
        /// <summary>
        ///     Object Counter.
        /// </summary>
        public static Dictionary<string, ushort> MemoryObjectCountDictionary { get; } =
            new Dictionary<string, ushort>();


        /// <summary>
        ///     Increases the counter of <see cref="MemoryObjectCountDictionary" />
        /// </summary>
        /// <param name="obj"></param>
        public static void Add(object obj)
        {
            // Do nothing because this method is only intended for debugging mode
#if DEBUG
            var key = obj.GetType().FullName;
            if (MemoryObjectCountDictionary.ContainsKey(key))
            {
                MemoryObjectCountDictionary[key]++;
            }
            else
            {
                MemoryObjectCountDictionary.Add(key, 1);
            }
#endif
        }

        /// <summary>
        ///     Decreases the counter of <see cref="MemoryObjectCountDictionary" />
        /// </summary>
        /// <param name="obj"></param>
        public static void Remove(object obj)
        {
            // Do nothing because this method is only intended for debugging mode
#if DEBUG
            var key = obj.GetType().FullName;
            if (MemoryObjectCountDictionary.ContainsKey(key))
            {
                MemoryObjectCountDictionary[key]--;
            }
#endif
        }
    }
}