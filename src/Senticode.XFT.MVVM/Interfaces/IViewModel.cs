using System.ComponentModel;
using System.Threading.Tasks;
using Senticode.Xamarin.Tools.MVVM.Abstractions;
using Xamarin.Forms;

namespace Senticode.Xamarin.Tools.MVVM.Interfaces
{
    /// <summary>
    ///     ViewModel interface for MVVM implementation.
    /// </summary>
    public interface IViewModel : IObservableObject
    {
        /// <summary>
        ///     Title view model.
        /// </summary>
        string Title { get; }

        bool IsBusy { get; set; }

        bool IsInitialized { get; }

        /// <summary>
        ///     This method can be used to trigger lazy initialization of the view model properties.
        ///     It is called when the event BindingContextChanged of ViewBase.
        /// </summary>
        Task OnInitializedAsync(object sender, ViewBaseEventArgs eventArgs);

        /// <summary>
        ///     This method can be used to trigger manage completion of the view model process.
        ///     It is called when the event BindingContextChanged of ViewBase.
        /// </summary>
        Task OnClosedAsync(object sender, ViewBaseEventArgs eventArgs);


        /// <summary>
        ///     Alerts notify the ViewModel about the change of the corresponding property of the Model.
        /// </summary>
        /// <param name = "sender"></param>
        /// <param name = "e"></param>
        void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e);

        bool OnBackButtonPressed(Page sender);
    }
}