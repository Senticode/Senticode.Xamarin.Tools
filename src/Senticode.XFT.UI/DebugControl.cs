using Xamarin.Forms;

namespace Senticode.Xamarin.Tools.UI
{
    /// <summary>
    ///     Makes content visible only in DEBUG configuration.
    /// </summary>
    [ContentProperty("Content")]
    public class DebugControl : ContentView
    {
        public DebugControl()
        {
            IsVisible = false;
#if DEBUG
            IsVisible = true;
#endif
        }
    }
}