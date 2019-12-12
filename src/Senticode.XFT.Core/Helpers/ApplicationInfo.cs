using System.Reflection;
using Xamarin.Forms;

namespace Senticode.Xamarin.Tools.Core.Helpers
{
    /// <summary>
    ///     Helper class to display application information.
    /// </summary>
    public class ApplicationInfo
    {
        private readonly Assembly _assembly;

        public ApplicationInfo()
        {
            _assembly = Application.Current.GetType().GetTypeInfo().Assembly;
        }

        /// <summary>
        ///     Get current assembly version.
        /// </summary>
        public string Version => _assembly.GetName().Version.ToString();
    }
}