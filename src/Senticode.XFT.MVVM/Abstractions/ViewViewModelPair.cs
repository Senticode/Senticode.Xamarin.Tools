using Xamarin.Forms;

namespace Senticode.Xamarin.Tools.MVVM.Abstractions
{
    /// <summary>
    ///     Struct that holds View and ViewModel.
    /// </summary>
    public struct ViewViewModelPair
    {
        public ViewViewModelPair(View view, ViewModelBase viewModel)
        {
            View = view;
            ViewModel = viewModel;
        }

        /// <summary>
        ///     Gets the Page property.
        /// </summary>
        public View View { get; }

        /// <summary>
        ///     Gets the ViewModel property.
        /// </summary>
        public ViewModelBase ViewModel { get; }
    }
}
