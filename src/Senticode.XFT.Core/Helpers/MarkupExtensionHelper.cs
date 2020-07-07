using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using Xamarin.Forms;

namespace Senticode.Xamarin.Tools.Core.Helpers
{
    /// <summary>
    ///     
    /// </summary>
    internal static class MarkupExtensionHelper
    {
        /// <summary>
        ///     Gets the source value of the binding.
        /// </summary>
        /// <param name="target">Binding target.</param>
        /// <param name="binding">binding.</param>
        /// <returns>Returns binding source if found, otherwise, null.</returns>
        public static object ExtractMember(BindableObject target, Binding binding)
        {
            var container = target.BindingContext;

            if (container == null)
            {
                Debug.WriteLine($"BindableObject {target} was not found to BindingContext");
                return null;
            }

            var property = binding.Path;
            var type = container.GetType();
            var info = type.GetRuntimeProperty(property);

            Debug.WriteLineIf(info == null, $"Property {property} was not found to BindingContext {type.Name} ");

            if (binding.Converter != null)
            {
                return binding.Converter.Convert(info?.GetValue(container), info?.PropertyType,
                    binding.ConverterParameter, CultureInfo.CurrentCulture);
            }

            return info?.GetValue(container);
        }
    }
}