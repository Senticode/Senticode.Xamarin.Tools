using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Senticode.Xamarin.Tools.MVVM.Interfaces;

namespace Senticode.Xamarin.Tools.MVVM.Abstractions
{
    /// <summary>
    ///     Class that can notify other classes when properties in it change.
    /// </summary>
    public abstract class ObservableObject : IObservableObject
    {
        /// <summary>
        ///     Occurs when property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Occurs when property is changing.
        /// </summary>
        public event PropertyChangingEventHandler PropertyChanging;

        /// <summary>
        ///     Sets the property value.
        /// </summary>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="backingStore">Field with value.</param>
        /// <param name="value">New value.</param>
        /// <param name="propertyName">Property name.</param>
        /// <returns><c>true</c> if new value is set, otherwise <c>false</c>.</returns>
        public bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "")
        {
            return SetProperty(ref backingStore, value, null, null, propertyName);
        }

        /// <summary>
        ///     Sets the property value.
        /// </summary>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="backingStore">Field with value.</param>
        /// <param name="value">New value.</param>
        /// <param name="propertyName">Property name.</param>
        /// <param name="onChanged">Action when property changed.</param>
        /// <returns><c>true</c> if new value is set, otherwise <c>false</c>.</returns>
        public bool SetProperty<T>(ref T backingStore, T value, Action onChanged,
            [CallerMemberName] string propertyName = "")
        {
            return SetProperty(ref backingStore, value, onChanged, null, propertyName);
        }

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
        public bool SetProperty<T>(ref T backingStore, T value, Action onChanged, Action onChanging,
            [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
            {
                return false;
            }

            OnPropertyChanging(propertyName, onChanging);
            backingStore = value;
            OnPropertyChanged(propertyName, onChanged);
            return true;
        }

        private void OnPropertyChanging(string propertyName, Action onChanging)
        {
            onChanging?.Invoke();
            OnPropertyChanging(propertyName);
        }

        protected virtual void OnPropertyChanging([CallerMemberName] string propertyName = "")
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        private void OnPropertyChanged(string propertyName, Action onChanged)
        {
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}