using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms.Xaml;

namespace Senticode.Xamarin.Tools.Core.MarkupExtensions
{
    /// <summary>
    ///     Return the target object.
    /// </summary>
    public class SelfExtension : IMarkupExtension
    {
        /// <summary>
        ///     Returns the target object.
        /// </summary>
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

            var pvt = serviceProvider.GetService(typeof(IProvideValueTarget));

            if (pvt == null) throw new ArgumentException("serviceProvider does not provide an IProvideValueTarget");
            
            var properties = pvt.GetType().GetProperties();
            IEnumerable<object> parentObjects = null;
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(IEnumerable<object>))
                {
                    parentObjects = property.GetValue(pvt) as IEnumerable<object>;
                    break;
                }
            }
            
            if (parentObjects == null) return null;

            var enumerable = parentObjects as object[] ?? parentObjects.ToArray();
            return enumerable[1];
        }
    }
}