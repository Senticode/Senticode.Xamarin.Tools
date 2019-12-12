using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Senticode.Xamarin.Tools.Core.Helpers
{
    /// <summary>
    ///     Class that provides functionality for getting property information.
    /// </summary>
    public static class PropertiesSupporter
    {
        /// <summary>
        ///     Gets <c>PropertyInfo</c> for property from its accessor.
        /// </summary>
        /// <typeparam name="T">Property type.</typeparam>
        /// <param name="propertyExpression">Property accessor.</param>
        /// <returns>Returns <c>PropertyInfo</c>.</returns>
        public static PropertyInfo ExtractPropertyInfo<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException(nameof(propertyExpression));
            }

            if (!(propertyExpression.Body is MemberExpression memberExpression))
            {
                throw new ArgumentException("Not Member Access Expression");
            }

            var propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo == null)
            {
                throw new ArgumentException("Expression Not Property");
            }

            return propertyInfo;
        }

        /// <summary>
        ///     Gets property name from its accessor.
        /// </summary>
        /// <typeparam name="T">Property type.</typeparam>
        /// <param name="propertyExpression">Property accessor.</param>
        /// <returns>Returns property name.</returns>
        public static string ExtractPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException(nameof(propertyExpression));
            }

            if (!(propertyExpression.Body is MemberExpression memberExpression))
            {
                throw new ArgumentException("Not Member Access Expression");
            }

            var propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo == null)
            {
                throw new ArgumentException("Expression Not Property");
            }

            return memberExpression.Member.Name;
        }

        /// <summary>
        ///     Gets all properties for type.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <returns>Returns array of <c>PropertyInfo</c>.</returns>
        public static PropertyInfo[] GetPublicProperties(this Type type)
        {
            if (!type.IsInterface)
            {
                return type.GetProperties(BindingFlags.FlattenHierarchy
                                          | BindingFlags.Public | BindingFlags.Instance);
            }

            var propertyInfos = new List<PropertyInfo>();

            var considered = new List<Type>();
            var queue = new Queue<Type>();
            considered.Add(type);
            queue.Enqueue(type);
            while (queue.Count > 0)
            {
                var subType = queue.Dequeue();
                foreach (var subInterface in subType.GetInterfaces())
                {
                    if (considered.Contains(subInterface)) continue;

                    considered.Add(subInterface);
                    queue.Enqueue(subInterface);
                }

                var typeProperties = subType.GetProperties(
                    BindingFlags.FlattenHierarchy
                    | BindingFlags.Public
                    | BindingFlags.Instance);

                var newPropertyInfos = typeProperties
                    .Where(x => !propertyInfos.Contains(x));

                propertyInfos.InsertRange(0, newPropertyInfos);
            }

            return propertyInfos.ToArray();
        }
    }
}