using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Senticode.Xamarin.Tools.MVVM.Interfaces
{
    /// <summary>
    ///     Interface that must implement classes to be able to notify other classes when properties in it change.
    /// </summary>
    public interface IObservableObject : INotifyPropertyChanged, INotifyPropertyChanging
    {
        /// <summary>
        ///     Sets the property value.
        /// </summary>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="backingStore">Field with value.</param>
        /// <param name="value">New value.</param>
        /// <param name="propertyName">Property name.</param>
        /// <returns><c>true</c> if new value is set, otherwise <c>false</c>.</returns>
        bool SetProperty<T>(
            ref T backingStore, T value,
            [CallerMemberName] string propertyName = "");

        /// <summary>
        ///     Sets the property value.
        /// </summary>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="backingStore">Field with value.</param>
        /// <param name="value">New value.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="onChanged">Action when property changed.</param>
        /// <returns><c>true</c> if new value is set, otherwise <c>false</c>.</returns>
        bool SetProperty<T>(
            ref T backingStore, T value,
            Action onChanged,
            [CallerMemberName] string propertyName = "");

        /// <summary>
        ///     Sets the property value.
        /// </summary>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="backingStore">Field with value.</param>
        /// <param name="value">New value.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="onChanged">Action when property changed.</param>
        /// <param name="onChanging">Action when property changing.</param>
        /// <returns><c>true</c> if new value is set, otherwise <c>false</c>.</returns>
        bool SetProperty<T>(
            ref T backingStore, T value,
            Action onChanged,
            Action onChanging,
            [CallerMemberName] string propertyName = "");
    }
}