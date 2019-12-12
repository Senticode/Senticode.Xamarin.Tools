using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Senticode.Xamarin.Tools.MVVM.Abstractions
{
    /// <summary>
    ///     Serves as base class for classes containing data for application to display.
    /// </summary>
    public abstract class ModelBase : WeakObservableObject
    {
        private readonly Lazy<PropertyInfo[]> _properties;

        protected ModelBase()
        {
            MemoryInfo.Add(this);
            _properties = new Lazy<PropertyInfo[]>(() => GetType().GetProperties());
        }

        ~ModelBase()
        {
            MemoryInfo.Remove(this);
        }

        /// <summary>
        ///     Sets property value.
        /// </summary>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="value">New value.</param>
        /// <param name="propertyName">Property name.</param>
        /// <returns><c>true</c> if new value is set, otherwise <c>false</c>.</returns>
        public bool TrySetValue<T>(T value,
            [CallerMemberName] string propertyName = "")
        {
            try
            {
                var property = _properties.Value.FirstOrDefault(x => x.Name == propertyName);
                if (property != null) property.SetValue(this, value);
                else return false;
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}