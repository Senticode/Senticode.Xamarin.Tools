using System.Threading.Tasks;
using Xamarin.Forms;

namespace Senticode.Xamarin.Tools.Core.Interfaces.Staff
{
    /// <summary>
    ///     Represents an animation.
    /// </summary>
    public interface IAnimation
    {
        /// <summary>
        ///     Starts animation.
        /// </summary>        
        Task Play(View sender);
    }
}