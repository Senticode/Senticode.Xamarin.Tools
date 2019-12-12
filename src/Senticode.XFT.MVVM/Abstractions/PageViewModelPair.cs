using Xamarin.Forms;

namespace Senticode.Xamarin.Tools.MVVM.Abstractions
{
    /// <summary>
    ///     Struct that holds Page and ViewModel.
    /// </summary>
    public struct PageViewModelPair
    {
        public PageViewModelPair(Page page, ViewModelBase viewModel)
        {
            Page = page;
            ViewModel = viewModel;
        }

        /// <summary>
        ///     Gets the Page property.
        /// </summary>
        public Page Page { get; }

        /// <summary>
        ///     Gets the ViewModel property.
        /// </summary>
        public ViewModelBase ViewModel { get; }
    }
}