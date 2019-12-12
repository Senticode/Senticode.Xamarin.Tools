using System;
using Xamarin.Forms;

namespace Senticode.Xamarin.Tools.MVVM.Abstractions
{
    /// <summary>
    ///     Contain event data of ViewBase class, and provides a value to use for events handler methods of IViewModel.
    /// </summary>
    public class ViewBaseEventArgs : EventArgs
    {
        public ViewBaseEventArgs(Page page)
        {
            PageType = page.GetType();
        }

        /// <summary>
        ///     Type of parent Page which contains ViewBase (event source).
        /// </summary>
        public Type PageType { get; }
    }
}