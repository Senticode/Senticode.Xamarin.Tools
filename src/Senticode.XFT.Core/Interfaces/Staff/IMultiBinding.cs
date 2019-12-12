using System.Collections.Generic;

namespace Senticode.Xamarin.Tools.Core.Interfaces.Staff
{
    /// <summary>
    ///     Provides ability to bind to more than one source.
    /// </summary>
    public interface IMultiBinding
    {
        /// <summary>
        ///     List of bindings.
        /// </summary>
        IList<object> Bindings { get; }

        /// <summary>
        ///    String that specifies how to format the binding if it displays the bound value as a string. 
        /// </summary>
        string StringFormat { get; set; }

        /// <summary>
        ///     <c>true</c> if value can be localized, otherwise, <c>false</c>.
        /// </summary>
        bool IsLocalizable { get; set; }

        /// <summary>
        ///     Converter for multibinding.
        /// </summary>
        IMultiValueConverter Converter { get; set; }

        /// <summary>
        ///     Parameter fot the converter.
        /// </summary>
        object ConverterParameter { get; set; }
    }
}